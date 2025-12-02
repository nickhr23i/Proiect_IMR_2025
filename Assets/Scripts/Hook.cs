using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HookState
{
    Empty,
    Bait,
    Fish
}

public class Hook : MonoBehaviour
{
    public HookState state= HookState.Empty;
    public GameObject hookModel;
    public GameObject baitModel;
    public GameObject fishModel;
    // Start is called before the first frame update
    void Start()
    {
        hookModel.SetActive(true);
        baitModel.SetActive(false);
        fishModel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(state==HookState.Empty)
        {
            if(other.gameObject.name=="Bait"||other.gameObject.name=="worm(Clone)")
            {
                hookModel.SetActive(false);
                baitModel.SetActive(true);
                Destroy(other.gameObject);
                state= HookState.Bait;
            }
        }
        else if(state==HookState.Bait)
        {
            if(other.gameObject.name=="WaterPlaceholder")
            {
                baitModel.SetActive(false);
                fishModel.SetActive(true);
                state = HookState.Fish;
            }
        }
    }




}
