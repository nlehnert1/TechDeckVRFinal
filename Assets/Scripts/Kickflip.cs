using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Kickflip : MonoBehaviour
{

    private Transform skateboard;
    private SteamVR_Action_Vector2 kickflipAction;

    // Start is called before the first frame update
    void Start()
    {
        skateboard = GameObject.FindGameObjectWithTag("skateboard").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
