using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabFromHook : MonoBehaviour
{
    [Header("Fish Prefabs")]
    public GameObject fishPrefabA;
    public GameObject fishPrefabB;
    public GameObject wormPrefab;

    [Header("References")]
    public Hook fishingHook;

    private XRGrabInteractable grab;
    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
    }


    private void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrabbed);
    }

    private void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (fishingHook.state == HookState.Empty)
            return;
        if (fishingHook.state == HookState.Bait)
        {
            SpawnWormInHand(args.interactorObject);
            fishingHook.ResetHook();
        }
        else if (fishingHook.state == HookState.Fish)
        {
            SpawnFishInHand(args.interactorObject);
            fishingHook.ResetHook();
        }

    }

    private void SpawnWormInHand(IXRSelectInteractor interactor)
    {

        GameObject worm = Instantiate(wormPrefab, interactor.transform.position,
            interactor.transform.rotation);

        XRGrabInteractable wormGrab = worm.GetComponent<XRGrabInteractable>();
        grab.interactionManager.SelectExit(interactor, grab);
        wormGrab.interactionManager.SelectEnter(interactor, wormGrab);
    }
    private void SpawnFishInHand(IXRSelectInteractor interactor)
    {
        GameObject prefab = fishingHook.GetFish().name.Contains("1")
            ? fishPrefabA
            : fishPrefabB;

        GameObject fish = Instantiate(prefab,interactor.transform.position,
            interactor.transform.rotation);

        XRGrabInteractable fishGrab = fish.GetComponent<XRGrabInteractable>();
        grab.interactionManager.SelectExit(interactor, grab);
        fishGrab.interactionManager.SelectEnter(interactor, fishGrab);
    }
}
