using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class RainAutoCycle : MonoBehaviour
{
    [Header("Dry Period (Wait Time)")]
    public float minDryTime = 10f;
    public float maxDryTime = 30f;

    [Header("Rain Period (Duration)")]
    public float minRainTime = 5f;
    public float maxRainTime = 15f;

    [Header("Audio & Transitions")]
    public AudioClip rainClip;
    public float fadeSpeed = 0.5f;

    private AudioSource audioSource;
    private ParticleSystem ps;
    private bool isRaining = false;
    private float timer;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();

        // Auto-setup AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = rainClip;
        audioSource.loop = true;
        audioSource.volume = 0;
        audioSource.playOnAwake = false;

        // Start as DRY
        isRaining = false;
        ps.Stop();

        // Set the first random wait time
        SetRandomTimer();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            // Switch states
            isRaining = !isRaining;

            if (isRaining)
            {
                ps.Play();
                if (!audioSource.isPlaying) audioSource.Play();
            }
            else
            {
                ps.Stop();
            }

            // Set the next random duration
            SetRandomTimer();
        }

        // Handle Audio Fading
        float targetVolume = isRaining ? 1f : 0f;
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, fadeSpeed * Time.deltaTime);

        // Optimization: Stop audio when it's totally silent
        if (!isRaining && audioSource.volume <= 0 && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void SetRandomTimer()
    {
        if (isRaining)
        {
            // How long the rain lasts
            timer = Random.Range(minRainTime, maxRainTime);
            Debug.Log("Rain started! Will last for: " + timer + " seconds.");
        }
        else
        {
            // How long to wait until it rains again
            timer = Random.Range(minDryTime, maxDryTime);
            Debug.Log("Rain stopped. Waiting " + timer + " seconds for next shower.");
        }
    }
}