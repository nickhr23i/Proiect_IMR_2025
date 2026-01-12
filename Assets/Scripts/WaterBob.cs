using UnityEngine;

public class WaterBob : MonoBehaviour
{
    [Header("Bobbing Settings")]
    public float amplitude = 0.1f;   
    public float frequency = 1f;     

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
