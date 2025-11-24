using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeWithLineRenderer : MonoBehaviour
{
    [Header("Connect Objects")]
    public Rigidbody startObject;
    public Rigidbody endObject;

    [Tooltip("Local attachment point on the start object")]
    public Vector3 startOffset = Vector3.zero;

    [Tooltip("Local attachment point on the end object")]
    public Vector3 endOffset = Vector3.zero;

    [Header("Rope Settings")]
    public int segmentCount = 20;
    public float segmentLength = 0.5f;
    public int constraintIterations = 50;
    public float pullStrength = 10f; // how strongly the rope pulls the second object

    [Header("Line Renderer Settings")]
    public float lineWidth = 0.1f;
    public Material lineMaterial;

    private List<Vector3> segments;
    private List<Vector3> prevPositions;
    private LineRenderer lineRenderer;

    void Start()
    {
        if (!startObject || !endObject)
        {
            Debug.LogError("Assign startObject and endObject!");
            return;
        }

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = lineWidth;
        if (lineMaterial != null) lineRenderer.material = lineMaterial;

        segments = new List<Vector3>();
        prevPositions = new List<Vector3>();

        Vector3 startWorld = startObject.transform.TransformPoint(startOffset);
        Vector3 endWorld = endObject.transform.TransformPoint(endOffset);
        Vector3 delta = (endWorld - startWorld) / segmentCount;

        for (int i = 0; i <= segmentCount; i++)
        {
            Vector3 pos = startWorld + delta * i;
            segments.Add(pos);
            prevPositions.Add(pos);
        }
    }

    void FixedUpdate()
    {
        Simulate(Time.fixedDeltaTime);
        ApplyConstraints();
        UpdateLineRenderer();
    }

    void Simulate(float dt)
    {
        for (int i = 1; i < segments.Count - 1; i++)
        {
            Vector3 temp = segments[i];
            segments[i] += (segments[i] - prevPositions[i]) + Physics.gravity * dt * dt;
            prevPositions[i] = temp;
        }
    }

    void ApplyConstraints()
    {
        for (int k = 0; k < constraintIterations; k++)
        {
            Vector3 startWorld = startObject.transform.TransformPoint(startOffset);
            Vector3 endWorld = endObject.transform.TransformPoint(endOffset);

            // Fix the start point
            segments[0] = startWorld;

            // Pull the end object with physics if rope is stretched
            Vector3 ropeDir = segments[segments.Count - 1] - segments[segments.Count - 2];
            float dist = ropeDir.magnitude;

            if (dist > segmentLength)
            {
                Vector3 pull = ropeDir.normalized * (dist - segmentLength) * pullStrength;
                endObject.velocity -= pull * Time.fixedDeltaTime; // apply pulling
            }

            // Fix the last segment to the end object
            segments[segments.Count - 1] = endObject.transform.TransformPoint(endOffset);

            // Maintain segment lengths
            for (int i = 0; i < segments.Count - 1; i++)
            {
                Vector3 delta = segments[i + 1] - segments[i];
                float diff = delta.magnitude - segmentLength;
                Vector3 offset = delta.normalized * diff * 0.5f;

                if (i != 0)
                    segments[i] += offset;
                if (i + 1 != segments.Count - 1)
                    segments[i + 1] -= offset;
            }
        }
    }

    void UpdateLineRenderer()
    {
        lineRenderer.positionCount = segments.Count;
        for (int i = 0; i < segments.Count; i++)
        {
            lineRenderer.SetPosition(i, segments[i]);
        }
    }
}
