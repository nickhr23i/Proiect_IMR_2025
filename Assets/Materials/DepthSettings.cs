using UnityEngine;

[ExecuteInEditMode]
public class DepthSettings : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }
}