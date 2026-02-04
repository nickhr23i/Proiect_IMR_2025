using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum HookState
{
    Empty,
    Bait,
    Fish
}

public class Hook : MonoBehaviour
{
    public HookState state = HookState.Empty;
    [Header("Fishing Settings")]
    public float minBiteTime = 3f;
    public float maxBiteTime = 10f;
    public float reactionTime = 3f;

    [Header("Models")]
    public GameObject hookModel;
    public GameObject baitModel;
    public GameObject fishModel;
    public GameObject fishModel2;
    private GameObject chosenFish;

    [Header("References")]
    public XRGrabInteractable rodGrabInteractable;
    // Start is called before the first frame update


    private bool inWater = false;
    private bool fishBiting = false;
    private bool fishCaught = false;
    private AudioScript audioScript = null;
    void Start()
    {
        hookModel.SetActive(true);
        baitModel.SetActive(false);
        fishModel.SetActive(false);
        audioScript = GetComponent<AudioScript>();
    }

    private void OnEnable()
    {
        rodGrabInteractable.activated.AddListener(OnTriggerPressed);
    }

    private void OnDisable()
    {
        rodGrabInteractable.activated.RemoveListener(OnTriggerPressed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (state == HookState.Empty)
        {
            if (other.gameObject.name == "Bait" || other.gameObject.name == "worm(Clone)" ||
                other.gameObject.name == "Worm(Clone)")
            {
                hookModel.SetActive(false);
                baitModel.SetActive(true);
                Destroy(other.gameObject);
                state = HookState.Bait;
            }
        }
        else if (state == HookState.Bait)
        {
            if (other.gameObject.name == "WaterPlaceholder" && !inWater)
            {
                inWater = true;
                StartCoroutine(FishBiteRoutine());
            }
        }
    }
    public void ResetHook()
    {
        if (chosenFish != null)
            chosenFish.SetActive(false);
        if (state == HookState.Bait)
            baitModel.SetActive(false);
        chosenFish = null;
        hookModel.SetActive(true);
        fishBiting = false;
        fishCaught = false;
        inWater = false;
        state = HookState.Empty;
    }
    private IEnumerator FishBiteRoutine()
    {
        float waitTime = UnityEngine.Random.Range(minBiteTime, maxBiteTime);
        yield return new WaitForSeconds(waitTime);

        fishBiting = true;
        audioScript.PlayClip(audioScript.clipBiting);
        Debug.Log("Fish is biting!");
        SendHaptics();

        StartCoroutine(FishEscapeRoutine());
    }

    private IEnumerator FishEscapeRoutine()
    {
        yield return new WaitForSeconds(reactionTime);

        if (fishBiting && !fishCaught)
        {
            FishEscaped();
        }
    }

    private void FishEscaped()
    {
        fishBiting = false;
        audioScript.PlayClip(audioScript.clipEscaped);
        Debug.Log("Fish escaped!");
        SendHaptics(0.2f, 0.1f);

        // Try again
        StartCoroutine(FishBiteRoutine());
    }

    private void OnTriggerPressed(ActivateEventArgs args)
    {
        if (!fishBiting || fishCaught)
            return;

        CatchFish();
    }

    private void CatchFish()
    {
        fishCaught = true;
        fishBiting = false;
        state = HookState.Fish;
        baitModel.SetActive(false);
        if (UnityEngine.Random.value > 0.5f)
            chosenFish = fishModel;
        else
            chosenFish = fishModel2;
        chosenFish.SetActive(true);
        audioScript.PlayClip(audioScript.clipCaught);
        Debug.Log("Fish caught!");
    }


    private void SendHaptics(float intensity = 0.7f, float duration = 0.2f)
    {
        if (rodGrabInteractable.interactorsSelecting.Count > 0)
        {
            XRBaseControllerInteractor controller = rodGrabInteractable.interactorsSelecting[0] as XRBaseControllerInteractor;

            if (controller != null)
            {
                controller.SendHapticImpulse(intensity, duration);
            }
        }
    }

    public GameObject GetFish()
    {
        return chosenFish;
    }

}
