using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject invHorizPanel;
    public GameObject invItemBtn;
    public GameObject equippedHorizPanel;
    public GameObject equippedItemBtn;

    public Vector3 centerPoint = new Vector3(Screen.width / 2, Screen.height / 2);

    public float walletBalance = 0f;
    public Text walletText;

    public Text emptyInvText;
    private string emptyInvString = "You currently have no items in your inventory.";

    public Text emptyEquippedText;
    private string emptyEquippedString = "You currently have no items equipped.";

    public List<InvItemBtn> items;
    public List<EquippedItemBtn> equipped;

    // Start is called before the first frame update
    void Start()
    {
        UpdateWalletText();
        UpdateEmptyInvText();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateWalletText();
    }

    public void AddInventoryItem(Collectible item)
    {
     // Debug.Log("Inventory.AddInventoryItem()");

        /*
        GameObject itemBtnGO = Instantiate(invItemBtn, invHorizPanel.transform);
        InvItemBtn itemBtnScript = itemBtnGO.GetComponent<InvItemBtn>();
        // Button btnScript = itemBtnGO.GetComponent<Button>(); //can we use this to edit the button's color?
        // Image imageScript = itemBtnGO.GetComponent<Image>();

        items.Add(itemBtnScript);
        */

        /*
         * //Moved to collectible
        if (items.Count >= GameManager.Instance.INV_CAPACITY)
        {
            ShowInvErrorMessage();
            return;
        }
        */

        InvItemBtn itemBtnScript = CreateInvBtn();

        itemBtnScript.collectibleType = item.collectibleType;
     // Debug.Log("item.collectibleType: " + item.collectibleType);
     // Debug.Log("itemBtnScript.collectibleType: " + itemBtnScript.collectibleType);
        itemBtnScript.value = item.value;

        itemBtnScript.typeTxt.text = itemBtnScript.collectibleType.ToString();
        itemBtnScript.valueTxt.text = itemBtnScript.value.ToString();

        itemBtnScript.uses = item.uses;

        UpdateEmptyInvText();
  
    }

    public InvItemBtn CreateInvBtn()
    {
        GameObject itemBtnGO = Instantiate(invItemBtn, invHorizPanel.transform);
        InvItemBtn itemBtnScript = itemBtnGO.GetComponent<InvItemBtn>();
        items.Add(itemBtnScript);
        return itemBtnScript;
    }

    public EquippedItemBtn CreateEquippedBtn()
    {
        GameObject equippedBtnGO = Instantiate(equippedItemBtn, equippedHorizPanel.transform);
        EquippedItemBtn equippedBtnScript = equippedBtnGO.GetComponent<EquippedItemBtn>();
        equipped.Add(equippedBtnScript);
        return equippedBtnScript;
    }

    public void ShowEquipErrorMessage()
    {
        string errorMessage = "You already have the maximum number of items equipped. Unequip an item to equip more items.";
        GameManager.Instance.CreateModal(errorMessage);
    }

    public void ShowInvErrorMessage()
    {
        string errorMessage = "You already have the maximum number of items in your inventory. Equip or drop some items to add more items.";
        GameManager.Instance.CreateModal(errorMessage);
    }

    public void UpdateWalletText()
    {
        walletText.text = $"Wallet Balance: ${walletBalance}";
    }

    public void AddBalance(float amount)
    {
        walletBalance += amount;
        UpdateWalletText();
    }

    public void RemBalance(float amount)
    {
        walletBalance -= amount;
        UpdateWalletText();
    }

    public void UpdateEmptyInvText()
    {
     // Debug.Log("Inventory.UpdateEmptyInvText()");
     // Debug.Log("Inventory.UpdateEmptyInvText() - items: " + items.ToString());
        if (items.Count == 0)
        {
         // Debug.Log("items.Count == 0");
            emptyInvText.text = emptyInvString;
        }
        else
        {
         // Debug.Log("items.Count NOT = 0");
            emptyInvText.text = "";
        }
    }

    public void UpdateEmptyEquippedText()
    {
     // Debug.Log("Inventory.UpdateEmptyEquippedText()");
     // Debug.Log("Inventory.UpdateEmptyEquippedText() - equipped: " + equipped.ToString());
        if (equipped.Count == 0)
        {
         // Debug.Log("equipped.Count == 0");
            emptyEquippedText.text = emptyEquippedString;
        }
        else
        {
         // Debug.Log("equipped.Count NOT = 0");
            emptyEquippedText.text = "";
        }
    }
}
