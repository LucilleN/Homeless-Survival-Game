using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : Interactable
{
    public enum CollectibleType {
        Clothing,
        Food,
        Money,
        Material,
    };
    [NonSerialized] public CollectibleType collectibleType;

    public float value = 1f;
    public float uses = 0;

    public float highlightSpeed = 1.0f;

    private Color defaultColor;
    [NonSerialized] public Color highlightColor = new Color(0.18f, 0.83f, 0f);

    private Vector3 defaultScale;
    public float hScaleFactor = 1.1f;
    private Vector3 highlightScale;

    private Material material;
    private Rigidbody rb;

    /** Start function called before the first frame update, used for initialization. */
    protected override void Start()
    {
        // Debug.Log("Collectible.Start");
        // Debug.Log("CollectibleType: " + collectibleType);
        // Debug.Log("Collectible position: " + transform.position);

        base.Start();

        material = this.gameObject.GetComponent<MeshRenderer>().material;
        transf = this.gameObject.transform;
        rb = this.gameObject.GetComponent<Rigidbody>();
        // Debug.Log("transf: " + transf);

        defaultColor = material.color;
        defaultScale = transf.localScale;

        //float newScale = defaultScale.magnitude * hScaleFactor;
        highlightScale = new Vector3(defaultScale.x * hScaleFactor, defaultScale.y * hScaleFactor, defaultScale.z * hScaleFactor);

    }

    /** Start function called before the first frame update, used for initialization. */
    public void Update()
    {
        disableRb();
    }

    /** Function called by PlayerController when user presses key to interact. */
    public override void Interact()
    {
        // Debug.Log("Collectible.Interact");
        // Debug.Log(collectibleType);

        if (collectibleType == CollectibleType.Money)
        {
            GameManager.Instance.inventory.AddBalance(value);
        }

        else
        {
            if (GameManager.Instance.inventory.items.Count >= GameManager.Instance.INV_CAPACITY)
            {
                GameManager.Instance.inventory.ShowInvErrorMessage();
                return;
            }

            GameManager.Instance.inventory.AddInventoryItem(this);
        }


        Destroy(this.gameObject);
    }

    /** Function called when player's look area collides with collectible trigger.
     *  Increases size and changes color to highlight color.
     */
    public override void Highlight()
    {
        // Debug.Log("Collectible.Highlight");

        PrintInfo();

        // Debug.Log("transf.localScale: " + transf.localScale);
        // Debug.Log("transf.localScale.x: " + transf.localScale.x);
        // Debug.Log("defaultScale: " + defaultScale);
        // Debug.Log("highlightScale: " + highlightScale);
        // Debug.Log("highlightSpeed: " + highlightSpeed);


        //transf.localScale = Vector3.Lerp(defaultScale, highlightScale, highlightSpeed);
        //material.color = Color.Lerp(defaultColor, highlightColor, highlightSpeed);
        transf.localScale = highlightScale;
        material.color = highlightColor;
        // Debug.Log("material.color: " + material.color.ToString());
        // Debug.Log("highlightColor: " + highlightColor.ToString());
    }

    /** Function called when player's look area exits collectible trigger. 
     *  Resets material and scale to default.
     */
    public override void Unhighlight()
    {
        // Debug.Log("Collectible.Unhighlight");

        transf.localScale = defaultScale;
        material.color = defaultColor;
    }

    private void disableRb()
    {
        if (rb && rb.velocity.magnitude == 0)
        {
            rb.isKinematic = true;
        }
    }

}