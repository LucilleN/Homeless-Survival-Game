using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Collectible
{
    private Dictionary<string, float> playerInventory;

    protected override void Start()
    {
     // Debug.Log("Food.Start");
        base.Start();

        playerInventory = this.player.inventory;
     // Debug.Log("playerInventory: " + playerInventory);

        collectibleType = CollectibleType.Food;
        uses = 1;
    }

    public override void Interact()
    {
        /*
        if (playerInventory == null) playerInventory = this.player.inventory;

        float prevQuantity = playerInventory.ContainsKey(this.name) ? playerInventory[this.name] : 0;
        player.inventory.Add(this.name, prevQuantity + 1);
        */

        //Later, this should only happen when the player uses the food from the inventory.
        //player.IncreaseHunger(quality);

        base.Interact();
    }

}
