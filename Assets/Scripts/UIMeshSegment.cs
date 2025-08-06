using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))] 
public class UIMeshSegment : Graphic
{
    public float startAngle;
    public float endAngle;
    public float innerRadius;
    public float outerRadius;
    public int segments = 32;
    protected override void Awake()
    {
        base.Awake();
        raycastTarget = true; 
    }
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (outerRadius <= innerRadius || segments < 3)
            return;

        float angleStep = (endAngle - startAngle) / segments;
        float currentAngle = startAngle * Mathf.Deg2Rad;

        // Add center vertex (for inner radius)
        UIVertex centerVertex = UIVertex.simpleVert;
        centerVertex.position = Vector3.zero;
        centerVertex.color = color;
        vh.AddVert(centerVertex);

        // Add vertices
        for (int i = 0; i <= segments; i++)
        {
            // Outer vertex
            UIVertex outerVertex = UIVertex.simpleVert;
            outerVertex.position = new Vector3(
                Mathf.Sin(currentAngle) * outerRadius,
                Mathf.Cos(currentAngle) * outerRadius,
                0
            );
            outerVertex.color = color;
            vh.AddVert(outerVertex);

            // Inner vertex
            UIVertex innerVertex = UIVertex.simpleVert;
            innerVertex.position = new Vector3(
                Mathf.Sin(currentAngle) * innerRadius,
                Mathf.Cos(currentAngle) * innerRadius,
                0
            );
            innerVertex.color = color;
            vh.AddVert(innerVertex);

            currentAngle += angleStep * Mathf.Deg2Rad;
        }

        // Add triangles
        for (int i = 0; i < segments; i++)
        {
            int outer1 = 1 + i * 2;
            int inner1 = 2 + i * 2;
            int outer2 = 3 + i * 2;
            int inner2 = 4 + i * 2;

            vh.AddTriangle(0, outer1, inner1);
            vh.AddTriangle(inner1, outer1, outer2);
            vh.AddTriangle(outer2, inner2, inner1);
        }
    }

    public void UpdateSegment(float newStartAngle, float newEndAngle, float newInnerRadius, float newOuterRadius)
    {
        startAngle = newStartAngle;
        endAngle = newEndAngle;
        innerRadius = newInnerRadius;
        outerRadius = newOuterRadius;
        SetVerticesDirty(); // Triggers OnPopulateMesh
    }
}