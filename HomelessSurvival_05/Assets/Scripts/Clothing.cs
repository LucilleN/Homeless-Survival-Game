using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clothing : Collectible
{
    private Dictionary<string, float> playerInventory;

    // Start is called before the first frame update
    protected override void Start()
    {
        Debug.Log("Clothing.Start");
        base.Start();

        playerInventory = this.player.inventory;
        Debug.Log("playerInventory: " + playerInventory);

        collectibleType = CollectibleType.Clothing;
        uses = 20;
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
