using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public static bool GameActive = true;
    public static bool GamePaused = false;

    public GameObject HUD;
    public Slider HealthSlider;
    public Slider HungerSlider;
    public Slider MoraleSlider;
    public Slider WarmthSlider;
    public PlayerController player;
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController firstPersonController;

    public Canvas messageCanvas;
    public GameObject messageModal;

    public Inventory inventory;
    [NonSerialized] public int INV_CAPACITY = 5;
    [NonSerialized] public int EQUIPPED_CAPACITY = 5;

    public static Color foodColor = new Color(0.7264f, 0.5782f, 0.3392f);
    public static Color shetlerColor = new Color(0.6981f, 0.4577f, 0.5086f);
    public static Color moneyColor = new Color(0.3077f, 0.3867f, 0.2353f);

    // Start is called before the first frame update
    void Start()
    {
        //FindRefs();
        HideInvMenu();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBarColors();
    }

    /** This function runs whenever you click reset in the Inspector. */
    private void Reset()
    {
        Debug.Log("GameManager.Reset()");
        FindRefs();
    }

    /** Currently not being used anywhere except for reset. Broken Slider references */
    private void FindRefs()
    {
        HUD = GameObject.FindGameObjectWithTag("HUD");
        HealthSlider = HUD.GetComponentInChildren<Slider>();
        // How do I find each of the different sliders? 
        // In Unity, they each have a different tag - Health, Hunger, Morale
        HungerSlider = HUD.GetComponentInChildren<Slider>();
        MoraleSlider = HUD.GetComponentInChildren<Slider>();

        player = FindObjectOfType<PlayerController>();
        //Three different ways to find objects in the game space:
        //One by finding with tag
        //One through component children
        //One through object type
       
    }

    public static void UpdateHealthHud()
    {
        Instance.HealthSlider.value = Instance.player.health / Instance.player.healthMax;
    }

    public static void UpdateHungerHud()
    {
        Instance.HungerSlider.value = Instance.player.hunger / Instance.player.hungerMax;
    }

    public static void UpdateMoraleHud()
    {
        Debug.Log("GameManager.UpdateMoraleHud()");
        Instance.MoraleSlider.value = Instance.player.morale / Instance.player.moraleMax;
    }

    public static void UpdateWarmthHud()
    {
        Instance.WarmthSlider.value = Instance.player.warmth / Instance.player.warmthMax;
    }

    public static void UpdateAllHud()
    {
        UpdateHealthHud();
        UpdateHungerHud();
        UpdateMoraleHud();
        UpdateWarmthHud();
    }

    private static void UpdateBarColors()
    {
        Color green = new Color(0.188f, 0.745f, 0.243f);
        Color red = Color.red;
        Color yellow = Color.yellow;

        Debug.Log("GameManager.UpdateBarColors()");

        GameObject[] BarFills = GameObject.FindGameObjectsWithTag("BarFill");
        foreach(GameObject barFill in BarFills)
        {
            float value = barFill.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Slider>().value;
            if (value >= 0.5)
            {
                barFill.GetComponent<Image>().color = Color.Lerp(yellow, green, (value - 0.5f) * 2);
            }
            else
            {
                barFill.GetComponent<Image>().color = Color.Lerp(red, yellow, value * 2);
            }
        }
    }

    public static void ToggleInventoryMenu()
    {
        if (Instance.inventory.gameObject.activeSelf)
        {
            HideInvMenu();
        }
        else
        {
            if (!GameManager.GamePaused)
            {
                ShowInvMenu();
            }
           
        }
    }

    private static void ShowInvMenu()
    {
        Instance.inventory.gameObject.SetActive(true);
        DeactivateGame();
    }

    private static void HideInvMenu()
    {
        Instance.inventory.gameObject.SetActive(false);
        ReactivateGame();
    }

    public void CreateModal(String text)
    {
        Debug.Log("GameManager.CreateModal()");
        DeactivateGame();

        GameObject modalGO = Instantiate(messageModal, messageCanvas.transform);
        Debug.Log("GameManager.CreateModal() modalGO = " + modalGO);


        Modal modalScript = modalGO.GetComponent<Modal>();
        modalScript.message = text;
        modalScript.UpdateText();
    }

    public static void DeactivateGame()
    {
        Debug.Log("GameManager.DeactivateGame()");

        Pause();
        // set lockCursor to false
        Instance.firstPersonController.m_MouseLook.SetCursorLock(false);
        // stop the mouse look rotation and movement
        Instance.firstPersonController.enabled = false;
    }

    public static void ReactivateGame()
    {
        Debug.Log("GameManager.ReactivateGame()");

        Unpause();
        // set lockCursor to true
        Instance.firstPersonController.m_MouseLook.SetCursorLock(true);
        // restart the mouse look rotation and movement
        Instance.firstPersonController.enabled = true;
    }

    private static void Pause()
    {
        Debug.Log("GameManager.Pause()");
        GamePaused = true;
        Time.timeScale = 0f;
    }

    private static void Unpause()
    {
        Debug.Log("GameManager.Unpause()");
        GamePaused = false;
        Time.timeScale = 1f;
    }

    public static void EndGame()
    {
        GameActive = false;
        Time.timeScale = 0f;
    }
}
