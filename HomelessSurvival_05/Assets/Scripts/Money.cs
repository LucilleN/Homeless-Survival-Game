using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : Collectible
{
    // Start is called before the first frame update
    protected override void Start()
    {
        // Debug.Log("Food.Start");
        base.Start();

        collectibleType = CollectibleType.Money;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        base.Interact();
    }
}
