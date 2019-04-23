using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modal : MonoBehaviour
{
    public GameObject modalPanel;
    public Button closeBtn;
    public Text messageText;

    public string message;

    public void Close()
    {
        GameManager.ReactivateGame();

        Destroy(modalPanel);
        Destroy(closeBtn);
        Destroy(messageText);
        Destroy(this.gameObject);
    }

    public void UpdateText()
    {
        messageText.text = message;
    }

}
