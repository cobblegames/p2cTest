using UnityEngine;

public class CameraDetectionSystem : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float rotationAngle = 90f; // Total rotation angle (half to each side)
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float detectionAngle = 60f;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private LayerMask obstructionMask;

    [Header("Gizmos Settings")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private Color detectionColor = Color.red;
    [SerializeField] private Color idleColor = Color.green;
    [SerializeField] private float gizmoHeight = 0.5f;

    private float initialLocalYRotation;
    private float minLocalRotation;
    private float maxLocalRotation;
    private float currentLocalRotation;
    private bool rotatingRight = true;
    private bool playerDetected = false;
    private Transform playerTransform;

    private void Start()
    {
        // Store initial LOCAL y rotation
        initialLocalYRotation = transform.localEulerAngles.y;
        currentLocalRotation = initialLocalYRotation;

        // Calculate min and max LOCAL rotation angles
        minLocalRotation = initialLocalYRotation - rotationAngle / 2f;
        maxLocalRotation = initialLocalYRotation + rotationAngle / 2f;
    }

    private void Update()
    {
        RotateCamera();
        CheckForPlayer();
    }

    private void RotateCamera()
    {
        float rotationStep = rotationSpeed * Time.deltaTime;

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
    }

    private void CheckForPlayer()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, detectionRange, detectionMask);

        if (rangeChecks.Length > 0)
        {
            playerTransform = rangeChecks[0].transform;
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer < detectionAngle / 2)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstructionMask))
                {
                    playerDetected = true;
                    Debug.Log("Player detected by camera!");
                    return;
                }
            }
        }

        playerDetected = false;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = playerDetected ? detectionColor : idleColor;

        // Draw detection range sphere
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw detection cone
        Vector3 forward = transform.forward * detectionRange;
        Vector3 left = Quaternion.Euler(0, -detectionAngle / 2, 0) * forward;
        Vector3 right = Quaternion.Euler(0, detectionAngle / 2, 0) * forward;

        Vector3 gizmoPos = transform.position + Vector3.up * gizmoHeight;

        Gizmos.DrawRay(gizmoPos, left);
        Gizmos.DrawRay(gizmoPos, right);

        // Draw connecting lines to visualize the cone
        int segments = 10;
        float angleStep = detectionAngle / segments;
        Vector3 prevPoint = gizmoPos + left;

        for (int i = 1; i <= segments; i++)
        {
            float angle = -detectionAngle / 2 + angleStep * i;
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
        Vector3 centerDirection = Quaternion.Euler(0, initialLocalYRotation, 0) * Vector3.forward * detectionRange;
        Gizmos.DrawRay(transform.position + Vector3.up * gizmoHeight, transform.parent != null ?
            transform.parent.rotation * centerDirection : centerDirection);

        Gizmos.color = Color.cyan;
        Vector3 minDirection = Quaternion.Euler(0, minLocalRotation, 0) * Vector3.forward * detectionRange;
        Vector3 maxDirection = Quaternion.Euler(0, maxLocalRotation, 0) * Vector3.forward * detectionRange;
        Gizmos.DrawRay(transform.position + Vector3.up * gizmoHeight, transform.parent != null ?
            transform.parent.rotation * minDirection : minDirection);
        Gizmos.DrawRay(transform.position + Vector3.up * gizmoHeight, transform.parent != null ?
            transform.parent.rotation * maxDirection : maxDirection);
    }
}