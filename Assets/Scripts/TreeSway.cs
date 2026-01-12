using UnityEngine;

public class TreeSway : MonoBehaviour
{
    [Header("Sway Settings")]
    public float swayAmount = 4f;      // Maximum rotation in degrees
    public float swaySpeed = 0.8f;       // Speed of the swaying motion
    public float randomOffset = 0f;    // Adds variety if many trees use this script

    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.rotation;

        randomOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        float sway = Mathf.Sin((Time.time + randomOffset) * swaySpeed) * swayAmount;

        Quaternion targetRotation = startRotation * Quaternion.Euler(sway, 0f, sway);

        transform.rotation = targetRotation;
    }
}
