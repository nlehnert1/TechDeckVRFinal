using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SkateboardController : MonoBehaviour
{

    private Transform skateboard;
    //private Vector3 prevLocation;
    //private Vector3 delta;
    //This should allow us to read inputs from the vive thumb touchpad
    private SteamVR_Action_Vector2 controllerInput = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("platformer", "move");
    bool doingTrick;
    bool tooSoon;
    private Queue<string> TrickQueue;
    Hand attachedHand;
    private SteamVR_Input_Sources hand;
    private Interactable interactable;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
        Debug.Log("Touched Start");
        TrickQueue = new Queue<string>();
        skateboard = this.gameObject.transform;
        //prevLocation = skateboard.position;
        doingTrick = false;
    }

    void Update()
    {
        Debug.Log("X: " + controllerInput.axis.x + ", Y: " + controllerInput.axis.y);
        // Checks for a hand if it isn't attached yet
        
        if(!tooSoon && interactable.attachedToHand)
        {
            StartCoroutine(ReadControllerInput());
        }
        //prevLocation = skateboard.position;
        if(!doingTrick && TrickQueue.Count > 0)
        {
            StartCoroutine(ExecuteNextTrick());
        }

    }

    public IEnumerator ReadControllerInput()
    {
        tooSoon = true;
        hand = interactable.attachedToHand.handType;
        Vector2 input = controllerInput[hand].axis;
        //Vector2 input = controllerInput.axis;
        if(input.y > 0.7f)
        {
            TrickQueue.Enqueue("impossible");
        }
        else if(input.y < -0.7f)
        {
            TrickQueue.Enqueue("popshoveit");
        }
        else if(input.x > 0.7f)
        {
            TrickQueue.Enqueue("kickflip");
        }
        else if(input.x < -0.7f)
        {
            TrickQueue.Enqueue("heelflip");
        }
        float time = 0.0f;
        while(time < 0.1f)  //Can only input a trick every 10th of a second
        {
            time += Time.deltaTime;
            yield return null;
        }
        tooSoon = false;
    }

    IEnumerator ExecuteNextTrick()
    {
        string trick = TrickQueue.Dequeue();
        if(trick.Equals("impossible"))
        {
            StartCoroutine(DoImpossible());
        }
        if(trick.Equals("kickflip"))
        {
            StartCoroutine(DoKickflip());
        }
        if (trick.Equals("heelflip"))
        {
            StartCoroutine(DoHeelflip());
        }
        if(trick.Equals("popshoveit"))
        {
            StartCoroutine(DoPopShoveIt());
        }
        yield return null;
    }
    
    IEnumerator DoKickflip()
    {
        //Detach from hand so hand doesn't flip around like crazy
        Hand hand = GetComponent<Interactable>().attachedToHand;
        hand.DetachObject(gameObject);

        doingTrick = true;
        Vector3 origRotation = skateboard.rotation.eulerAngles;
        float time = 0.0f;
        //Have kickflip take like half a second
        while (time <= 0.25f)
        {
            //TODO: This is very framerate dependent and not great, but it works for now.
            skateboard.Rotate(24, 0, 0);
            time += Time.deltaTime;
            yield return null;
        }
        // Reset board back to upright position
        skateboard.rotation = Quaternion.Euler(0, origRotation.y, origRotation.z);

        //quarter second delay
        time = 0f;
        while(time < 0.1f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        doingTrick = false;
    }

    IEnumerator DoHeelflip()
    {
        //Detach from hand so hand doesn't flip around like crazy
        Hand hand = GetComponent<Interactable>().attachedToHand;
        hand.DetachObject(gameObject);

        doingTrick = true;
        float time = 0.0f;
        while(time <= 0.25f)
        {
            skateboard.Rotate(-24, 0, 0);
            time += Time.deltaTime;
            yield return null;
        }

        skateboard.rotation = Quaternion.Euler(0, skateboard.rotation.y, skateboard.rotation.z);

        //quarter second delay
        time = 0f;
        while(time < 0.1f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        doingTrick = false;
    }

    IEnumerator DoImpossible()
    {
        //Detach from hand so hand doesn't flip around like crazy
        Hand hand = GetComponent<Interactable>().attachedToHand;
        hand.DetachObject(gameObject);

        doingTrick = true;
        float time = 0.0f;
        while(time <= 0.25f)
        {
            skateboard.Rotate(0, 0, -24);
            time += Time.deltaTime;
            yield return null;
        }

        //Reset position in case it went too far or not far enough
        skateboard.rotation = Quaternion.Euler(skateboard.rotation.x, skateboard.rotation.y, 0);

        //delay
        time = 0f;
        while(time  <= 0.1f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        doingTrick = false;
    }

    IEnumerator DoPopShoveIt()
    {
        //Detach from hand so hand doesn't flip around like crazy
        Hand hand = GetComponent<Interactable>().attachedToHand;
        hand.DetachObject(gameObject);
        doingTrick = true;
        float time = 0.0f;
        Vector3 origRotation = skateboard.rotation.eulerAngles;
        while (time <= 0.25f)
        {
            skateboard.Rotate(0, 12, 0);
            time += Time.deltaTime;
            yield return null;
        }

        //Set Y value to be 180 degrees from where it started
        skateboard.rotation = Quaternion.Euler(0, origRotation.y + 180, 0);

        //delay
        time = 0f;
        while (time <= 0.1f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        doingTrick = false;
    }
}

