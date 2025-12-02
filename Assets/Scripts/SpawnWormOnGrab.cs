using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpawnWormOnGrab : MonoBehaviour
{
    public GameObject wormPrefab;
    public Material wormMaterial;

    private XRBaseInteractable bucketInteractable;

    private void Awake()
    {
        bucketInteractable = GetComponent<XRBaseInteractable>();
    }

    private void OnEnable()
    {
        bucketInteractable.selectEntered.AddListener(OnBucketGrabbed);
    }

    private void OnDisable()
    {
        bucketInteractable.selectEntered.RemoveListener(OnBucketGrabbed);
    }

    private void OnBucketGrabbed(SelectEnterEventArgs args)
    {

        IXRSelectInteractor interactor = args.interactorObject;
        XRInteractionManager manager = bucketInteractable.interactionManager;

        GameObject worm = Instantiate(
            wormPrefab,
            interactor.transform.position,
            interactor.transform.rotation);

        worm.transform.localScale = new Vector3(8f, 8f, 8f);

        if (wormMaterial != null)
        {
            foreach (var r in worm.GetComponentsInChildren<MeshRenderer>())
                r.material = wormMaterial;
        }

        var rb = worm.GetComponent<Rigidbody>();
        if (rb == null) rb = worm.AddComponent<Rigidbody>();
        rb.isKinematic = false;

        var col = worm.GetComponent<Collider>();
        if (col == null) worm.AddComponent<BoxCollider>();

        XRGrabInteractable grab = worm.GetComponent<XRGrabInteractable>();
        if (grab == null) grab = worm.AddComponent<XRGrabInteractable>();
        grab.interactionLayers = InteractionLayerMask.GetMask("Interactable");

        manager.SelectEnter(interactor, grab);
    }
}
