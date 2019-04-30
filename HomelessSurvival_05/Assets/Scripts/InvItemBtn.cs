using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvItemBtn : MonoBehaviour
{
    public float value = 0;
    public float uses = 0;
    public Collectible.CollectibleType collectibleType;

    public Text typeTxt;
    public Text valueTxt;
    public Text usesTxt;

    public GameManager gameManager;
    public Inventory inventory;
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        // GameManager.Instance.inventory.items.Add(this);
        gameManager = GameManager.Instance;
        inventory = gameManager.inventory;
        player = gameManager.player;
        UpdateUsesText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseItem()
    {
     // Debug.Log("InvItemBtn.UseItem()");

        uses--;

        // For clothing items only: clicking the button decrements uses and destroys the button but creates button in the equipped section 
        // Unequipping (clicking the equipped button) will create a clothing button in inventory only if number of uses left > 0
        if (collectibleType == Collectible.CollectibleType.Clothing)
        {
         // Debug.Log("InvItemBtn.UseItem(), Collectible type is clothing");
            if (inventory.equipped.Count < gameManager.EQUIPPED_CAPACITY)
            {
                EquipItem();
                return;
            }
            
        }

        switch (collectibleType)
        {
            case Collectible.CollectibleType.Food:
                player.IncreaseHunger(value * 10);
                break;
            case Collectible.CollectibleType.Material:
                //use it to stay warm
                break;

        }

        // For everything that is not a clothing item, clicking the button instantaneously uses it and decrements uses
        // Destroy the button only when no more uses left
        if (uses <= 0)
        {
            inventory.items.Remove(this);
            Destroy(this.gameObject);
        }
        UpdateUsesText();
        inventory.UpdateEmptyInvText();
    }

    public void EquipItem()
    {
     // Debug.Log("InvItemBtn.EquipItem()");

        if (inventory.equipped.Count >= gameManager.EQUIPPED_CAPACITY)
        {
            inventory.ShowEquipErrorMessage();
            return;
        }

        if (collectibleType == Collectible.CollectibleType.Clothing)
        {

         // Debug.Log("InvItemBtn.EquipItem(), Collectible type is clothing");

            player.IncreaseMorale(value * 10);
            player.IncreaseHealth(value * 5);

            EquippedItemBtn equippedBtnScript = inventory.CreateEquippedBtn();

            equippedBtnScript.collectibleType = collectibleType;
            equippedBtnScript.uses = uses;
            equippedBtnScript.value = value;
            equippedBtnScript.typeTxt.text = equippedBtnScript.collectibleType.ToString();
            equippedBtnScript.valueTxt.text = equippedBtnScript.value.ToString();

            equippedBtnScript.UpdateUsesText();

            inventory.UpdateEmptyInvText();
            inventory.UpdateEmptyEquippedText();

            inventory.items.Remove(this);
            Destroy(this.gameObject);

            return;
        }
    }


    public void UpdateUsesText()
    {
        usesTxt.text = "Uses remaining: " + uses;
    }

}
