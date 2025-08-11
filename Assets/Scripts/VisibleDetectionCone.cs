
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisibleDetectionCone : MonoBehaviour
{

    [SerializeField] private int rayCount = 30;
    DetectorsData detectorsData;

    [Header("Visual Settings")]
    [SerializeField] private Material coneMaterial;

    [SerializeField] private bool drawBase = true;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;


    public void InitCone(DetectorsData data)
    {
        detectorsData = data;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = coneMaterial;
        GenerateCompleteCone();
        UpdateConeColor(PlayerAlarmStatus.NotDetected);
    }
    

 

    public void GenerateCompleteCone()
    {
        // Vertex count: 
        // - 1 for tip
        // - rayCount for wall edges
        // - rayCount for base perimeter
        // - 1 for base center
        int vertexCount = 1 + rayCount * 2 + 1;
        vertices = new Vector3[vertexCount];

        // Triangle count:
        // - rayCount*3 for walls
        // - rayCount*3 for base
        int triangleCount = rayCount * 6;
        triangles = new int[triangleCount];

        // 0 = Cone tip
        vertices[0] = Vector3.zero;

        // Generate wall vertices (1 to rayCount)
        for (int i = 0; i < rayCount; i++)
        {
            float angle = 2 * Mathf.PI * i / rayCount;
            Vector3 localDirection = Quaternion.Euler(
                Mathf.Sin(angle) * detectorsData.DetectionAngle * 0.5f,
                Mathf.Cos(angle) * detectorsData.DetectionAngle * 0.5f,
                0) * Vector3.forward;

            Vector3 worldDirection = transform.TransformDirection(localDirection);

            if (Physics.Raycast(transform.position, worldDirection, out RaycastHit hit, detectorsData.DetectionRange, detectorsData.ObstacleMask))
            {
                vertices[1 + i] = transform.InverseTransformPoint(hit.point);
            }
            else
            {
                vertices[1 + i] = localDirection * detectorsData.DetectionRange;
            }
        }

        // Generate base vertices (rayCount+1 to 2*rayCount)
        if (drawBase)
        {
            for (int i = 0; i < rayCount; i++)
            {
                vertices[1 + rayCount + i] = vertices[1 + i]; // Same as wall vertices
            }

            // Last vertex = base center (index 2*rayCount+1)
            vertices[2 * rayCount + 1] = Vector3.forward * detectorsData.DetectionRange;
        }

        // Generate wall triangles
        for (int i = 0; i < rayCount; i++)
        {
            int current = 1 + i;
            int next = 1 + (i + 1) % rayCount;

            triangles[i * 6] = 0;     // Tip
            triangles[i * 6 + 1] = current;
            triangles[i * 6 + 2] = next;
        }

        // Generate base triangles (if enabled)
        if (drawBase)
        {
            for (int i = 0; i < rayCount; i++)
            {
                int current = 1 + rayCount + i;
                int next = 1 + rayCount + (i + 1) % rayCount;
                int center = 2 * rayCount + 1;

                triangles[rayCount * 3 + i * 3] = current;
                triangles[rayCount * 3 + i * 3 + 1] = next;
                triangles[rayCount * 3 + i * 3 + 2] = center;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }



    public void UpdateConeColor(PlayerAlarmStatus status)
    {
        Color color = status == PlayerAlarmStatus.Detected ? detectorsData.DetectionColor : detectorsData.IdleColor;
        GetComponent<MeshRenderer>().material.color = color;
           
    }

   
}