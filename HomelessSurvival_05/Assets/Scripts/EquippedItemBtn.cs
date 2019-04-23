using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItemBtn : MonoBehaviour
{
    public float value = 0;
    public float uses = 0;
    public Collectible.CollectibleType collectibleType;

    public Text typeTxt;
    public Text valueTxt;
    public Text usesTxt;

    public int wearTime = 10000;

    public Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameManager.Instance.inventory;
        inventory.equipped.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        wearTime--;
        if (wearTime <= 0)
        {
            Unequip();
        }
        */
    }

    public void Unequip()
    {
        Debug.Log("ON CLICK - EquippedItemBtn.Unequip()");
        InvItemBtn itemBtnScript = GameManager.Instance.inventory.CreateInvBtn();

        Debug.Log("Created new InvBtn, itemBtnScript: " + itemBtnScript);

        itemBtnScript.collectibleType = collectibleType;
        itemBtnScript.uses = uses;
        itemBtnScript.value = value;
        itemBtnScript.typeTxt.text = itemBtnScript.collectibleType.ToString();
        itemBtnScript.valueTxt.text = itemBtnScript.value.ToString();

        GameManager.Instance.inventory.equipped.Remove(this);
        GameManager.Instance.inventory.UpdateEmptyEquippedText();
        GameManager.Instance.inventory.UpdateEmptyInvText();


        Destroy(this.gameObject);

    }

    public void UpdateUsesText()
    {
        usesTxt.text = "Uses remaining: " + uses;
    }
}
