using UnityEngine;

public class CameraDetectionSystem : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] DetectorsData detectorsData;




    [SerializeField] VisibleDetectionCone detectionCone;

    [Header("Gizmos Settings")]
    [SerializeField] private bool showGizmos = true;

    [SerializeField] private float gizmoHeight = 0.5f;

    private float initialLocalYRotation;
    private float minLocalRotation;
    private float maxLocalRotation;
    private float currentLocalRotation;
    private bool rotatingRight = true;
    private bool playerDetected = false;
    private Transform playerTransform;

    [SerializeField] int cameraID;
    [SerializeField] AudioSource audioSource;
    private void Start()
    {
        // Store initial LOCAL y rotation
        initialLocalYRotation = transform.localEulerAngles.y;
        currentLocalRotation = initialLocalYRotation;

        // Calculate min and max LOCAL rotation angles
        minLocalRotation = initialLocalYRotation - detectorsData.RotationAngle / 2f;
        maxLocalRotation = initialLocalYRotation + detectorsData.RotationAngle / 2f;

        detectionCone.InitCone(detectorsData);
    }

    private void Update()
    {
        RotateCamera();
        CheckForPlayer();
    }

    private void RotateCamera()
    {
        if(playerDetected) return; // Skip rotation if player is detected
        float rotationStep = detectorsData.RotationSpeed * Time.deltaTime;

        if (rotatingRight)
        {
            currentLocalRotation += rotationStep;

            if (currentLocalRotation >= maxLocalRotation)
            {
                currentLocalRotation = maxLocalRotation;
                rotatingRight = false;
            }
        }
        else
        {
            currentLocalRotation -= rotationStep;

            if (currentLocalRotation <= minLocalRotation)
            {
                currentLocalRotation = minLocalRotation;
                rotatingRight = true;
            }
        }

        // Apply the rotation using localEulerAngles to maintain parent rotation
        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x,
            currentLocalRotation,
            transform.localEulerAngles.z
        );

        detectionCone.transform.position = transform.position;
        detectionCone.transform.rotation = transform.rotation;
        detectionCone.GenerateCompleteCone();
    }

    private void CheckForPlayer()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, detectorsData.DetectionRange, detectorsData.DetectionMask);

        if (rangeChecks.Length > 0)
        {
            playerTransform = rangeChecks[0].transform;
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer < detectorsData.DetectionAngle / 2)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, detectorsData.ObstacleMask))
                {
                    playerDetected = true;
                    Debug.Log("Player detected by camera!");

                    GameEvents.PostOnPlayerDetected(true);
                    detectionCone.UpdateConeColor(PlayerStatus.Detected);
                    if (!audioSource.isPlaying)
                        audioSource.Play();
                    return;
                }
            }
        }
        GameEvents.PostOnPlayerDetected(false);
        detectionCone.UpdateConeColor(PlayerStatus.NotDetected);
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        playerDetected = false;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = playerDetected ? detectorsData.DetectionColor : detectorsData.IdleColor;

        // Draw detection range sphere
        Gizmos.DrawWireSphere(transform.position, detectorsData.DetectionRange);

        // Draw detection cone
        Vector3 forward = transform.forward * detectorsData.DetectionRange;
        Vector3 left = Quaternion.Euler(0, -detectorsData.DetectionAngle / 2, 0) * forward;
        Vector3 right = Quaternion.Euler(0, detectorsData.DetectionAngle / 2, 0) * forward;

        Vector3 gizmoPos = transform.position + Vector3.up * gizmoHeight;

        Gizmos.DrawRay(gizmoPos, left);
        Gizmos.DrawRay(gizmoPos, right);

        // Draw connecting lines to visualize the cone
        int segments = 10;
        float angleStep = detectorsData.DetectionAngle / segments;
        Vector3 prevPoint = gizmoPos + left;

        for (int i = 1; i <= segments; i++)
        {
            float angle = -detectorsData.DetectionAngle / 2 + angleStep * i;
            Vector3 nextPoint = gizmoPos + Quaternion.Euler(0, angle, 0) * forward;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }

        // Draw line to player if detected
        if (playerDetected && playerTransform != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(gizmoPos, playerTransform.position + Vector3.up * gizmoHeight);
        }

        // Draw rotation range in editor
        if (!Application.isPlaying) return;

        Gizmos.color = Color.blue;
        Vector3 centerDirection = Quaternion.Euler(0, initialLocalYRotation, 0) * Vector3.forward * detectorsData.DetectionRange;
        Gizmos.DrawRay(transform.position + Vector3.up * gizmoHeight, transform.parent != null ?
            transform.parent.rotation * centerDirection : centerDirection);

        Gizmos.color = Color.cyan;
        Vector3 minDirection = Quaternion.Euler(0, minLocalRotation, 0) * Vector3.forward * detectorsData.DetectionRange;
        Vector3 maxDirection = Quaternion.Euler(0, maxLocalRotation, 0) * Vector3.forward * detectorsData.DetectionRange;
        Gizmos.DrawRay(transform.position + Vector3.up * gizmoHeight, transform.parent != null ?
            transform.parent.rotation * minDirection : minDirection);
        Gizmos.DrawRay(transform.position + Vector3.up * gizmoHeight, transform.parent != null ?
            transform.parent.rotation * maxDirection : maxDirection);
    }
}