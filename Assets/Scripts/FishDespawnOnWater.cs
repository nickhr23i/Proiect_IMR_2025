using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class FishDespawnOnWater : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Optional: if your water is a trigger collider
        if (!grabInteractable.isSelected && other.gameObject.name=="WaterPlaceholder")
        {
            Despawn();
        }
    }

    private void Despawn()
    {

        Destroy(gameObject);
    }
}
