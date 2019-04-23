using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected Transform transf;
    protected PlayerController player;

    /** Start function called before the first frame update, used for initialization. */
    protected virtual void Start()
    {
        Debug.Log("Interactable.Start");
        player = FindObjectOfType<PlayerController>();
        Debug.Log("Interactable.Start - player: " + player);
    }

    // MOVING ALL COLLISION DETECTION TO PLAYER CONTROLLER WITH RAYCAST


    /*
    


    protected void OnTriggerEnter(Collider other)
    {
        Debug.Log("Interactable.OnTriggerEnter");

        //PrintInfo();

        //Debug.Log("other: " + other.name);
        //Debug.Log("other.gameObject.tag: " + other.gameObject.tag);


        if (!this.enabled || other.gameObject.tag != "InteractableCollider") return;

        Debug.Log("Interactable.OnTriggerEnter - other: " + other.name);

        Highlight();

    }

    protected void OnTriggerStay(Collider other)
    {
        Debug.Log("Interactable.OnTriggerStay");

        //PrintInfo();

        //Debug.Log("other: " + other.name);
        //Debug.Log("other.gameObject.tag: " + other.gameObject.tag);

        if (!this.enabled || other.gameObject.tag != "InteractableCollider") return;

        Debug.Log("Interactable.OnTriggerStay(): other = " + other.ToString());
        if (player.InteractPressed)
        {
            Interact();
        }

    }

    protected void OnTriggerExit(Collider other)
    {
        Debug.Log("Interactable.OnTriggerExit");

        //PrintInfo();

        //Debug.Log("other: " + other.name);
        //Debug.Log("other.gameObject.tag: " + other.gameObject.tag);

        if (!this.enabled || other.gameObject.tag != "InteractableCollider") return;

        Debug.Log("other: " + other.name);

        Unhighlight();

    }

    */

    public virtual void Interact()
    {
        Debug.Log("Interactable.Interact");

    }

    public virtual void Highlight()
    {
        Debug.Log("Interactable.Highlight");
    }

    public virtual void Unhighlight()
    {
        Debug.Log("Interactable.Unhighlight");
    }

    protected void PrintInfo()
    {
        Debug.Log("Interactable.PrintInfo");

        Debug.Log("this: " + this);
        Debug.Log("this.name: " + this.name);
        Debug.Log("this.enabled: " + this.enabled);
        Debug.Log("this.isActiveAndEnabled: " + this.isActiveAndEnabled);
        Debug.Log("this.gameObject.transform: " + this.gameObject.transform);
        Debug.Log("player: " + player);
        Debug.Log("transf: " + transf);
    }

}
