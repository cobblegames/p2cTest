using UnityEngine;

[CreateAssetMenu(fileName = "DetectorData", menuName = "Settings/DetectorsSettings", order = 100)]

public class DetectorsData : ScriptableObject
{
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float rotationAngle = 90f;

    [SerializeField][Range(1, 179)] private float detectionAngle = 60f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask detectionMask;



    [SerializeField] private Color detectionColor = Color.red;
    [SerializeField] private Color idleColor = Color.green;

    public float RotationSpeed => rotationSpeed;
    public float RotationAngle => rotationAngle;
    public float DetectionRange => detectionRange;
    public float DetectionAngle => detectionAngle;
    public LayerMask ObstacleMask => obstacleMask;
    public LayerMask DetectionMask => detectionMask;
    public Color DetectionColor => detectionColor;
    public Color IdleColor => idleColor;

}
