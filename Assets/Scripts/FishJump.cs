using UnityEngine;

public class FishJump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpHeight = 2f;
    public float jumpDistance = 3f;
    public float jumpDuration = 1f;
    public float waitBetweenJumps = 2f;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip jumpSound;   // plays when going up
    public AudioClip splashSound; // plays when landing

    private Vector3 startPos;
    private float timer = 0f;
    private bool isJumping = false;
    private float jumpTime = 0f;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Wait between jumps
        if (!isJumping)
        {
            timer += Time.deltaTime;
            if (timer >= waitBetweenJumps)
            {
                timer = 0f;
                isJumping = true;
                jumpTime = 0f;

                // Play jump sound
                if (audioSource && jumpSound)
                    audioSource.PlayOneShot(jumpSound);
            }
            return;
        }

        // Jump progress
        jumpTime += Time.deltaTime;
        float t = jumpTime / jumpDuration;

        if (t >= 1f)
        {
            isJumping = false;
            transform.position = startPos;
            transform.rotation = Quaternion.identity;

            // Play splash sound
            if (audioSource && splashSound)
                audioSource.PlayOneShot(splashSound);

            return;
        }

        // Forward movement
        Vector3 forwardPos = startPos + transform.forward * (t * jumpDistance);

        // Height curve
        float height = Mathf.Sin(t * Mathf.PI) * jumpHeight;

        // Apply position
        transform.position = new Vector3(
            forwardPos.x,
            startPos.y + height,
            forwardPos.z
        );

        // Tilt fish
        float tiltAngle = Mathf.Lerp(45f, -45f, t);
        transform.rotation = Quaternion.Euler(tiltAngle, transform.eulerAngles.y, 0f);
    }
}
