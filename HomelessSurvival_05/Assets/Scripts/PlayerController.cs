using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Camera FPScamera;
    public Vector3 centerPoint = new Vector3(Screen.width / 2, Screen.height / 2);
    private float interactDistance = 2f;
    private int layerMask = 9; //Layer for Interactables


    private Vector3 raycastOffset;// = new Vector3(0, 5, 0);
    /*private Vector3 raycastPos;
    private Vector3 raycastDir;
    //These are broken
    */

    private GameObject currentlyInteractingGO;
    private Interactable currentlyInteractable;

    public Text healthText;
    public Text moraleText;
    public Text hungerText;
    public Text energyText;

    private const float HEALTH_DEFAULT = 100;
    private float healthMin = 0;
    public float healthMax = HEALTH_DEFAULT;
    public float health = HEALTH_DEFAULT;

    private const float HUNGER_DEFAULT = 100;
    private float hungerMin = 0;
    public float hungerMax = HUNGER_DEFAULT;
    public float hunger = HUNGER_DEFAULT;

    private const float MORALE_DEFAULT = 75;
    private float moraleMin = 0;
    public float moraleMax = 100;
    public float morale = MORALE_DEFAULT;

    private const float WARMTH_DEFAULT = 75;
    private float warmthMin = 0;
    public float warmthMax = 100;
    public float warmth = MORALE_DEFAULT;

    private float HEALTH_DRAIN_DEFAULT = 0.0001f;
    private float HEALTH_DRAIN_2 = 0.001f;
    private float HEALTH_DRAIN_3 = 0.002f;
    private float HUNGER_DRAIN_DEFAULT = 0.001f;//0.000001f;
    private float MORALE_DRAIN_DEFAULT = 0.002f;
    private float WARMTH_DRAIN_DEFAULT = 0.002f;

    private float healthDrain;
    private float hungerDrain;
    private float moraleDrain;
    private float warmthDrain;

    //Note: cannot use float health = healthMax here because the values are being initialized in each instance

    public KeyCode interactKey = KeyCode.E;
    public KeyCode inventoryKey = KeyCode.I;

    //default for a bool is false, but doesn't hurt to hard code it.
    private bool _interactPressed = false;

    public bool  InteractPressed
    {
        get
        {
            return _interactPressed;
        }
    }

    private bool _inventoryPressed = false;

    public bool InventoryPressed
    {
        get
        {
            return _inventoryPressed;
        }
    }

    private Rigidbody rb;
    public Dictionary<string, float> inventory;



    // ---------------------------------------------------------------
    // BUILT-IN UNITY LIFECYCLE METHODS
    // ---------------------------------------------------------------

    private void Awake()
    {
        Debug.Log("PlayerController.Awake");

        healthDrain = HEALTH_DRAIN_DEFAULT;
        hungerDrain = HUNGER_DRAIN_DEFAULT;
        moraleDrain = MORALE_DRAIN_DEFAULT;
        warmthDrain = WARMTH_DRAIN_DEFAULT;


        rb = GetComponent<Rigidbody>();
        inventory = new Dictionary<string, float>();

        raycastOffset = FPScamera.transform.position - transform.position;
        //print("raycastOffset: " + raycastOffset);

    }

    /** Start is called before the first frame update */
    void Start()
    {
        Debug.Log("PlayerController.Start");
        UpdateHealth();
        UpdateHunger();
        UpdateMorale();
    }

    private void FixedUpdate()
    {

    }

    private void Update()
    {
        if (!GameManager.GameActive) return;

        /*
        raycastPos = new Vector3(
                        FPScamera.transform.position.x,
                        FPScamera.transform.position.y + raycastOffset,
                        FPScamera.transform.position.z
        );
        raycastDir = FPScamera.transform.forward;
        //for some reason this stuff is broken but idk
        */

        /** GetKeyDown is only true for a single frame when the key is fist pressed down */
        _interactPressed = Input.GetKeyDown(interactKey);
        _inventoryPressed = Input.GetKeyDown(inventoryKey); 

        if(_inventoryPressed)
        {
            GameManager.ToggleInventoryMenu();
        }

        DrainAll();
        /*
        Debug.Log("health: " + health);
        Debug.Log("hunger: " + hunger);
        Debug.Log("morale: " + morale);
        */

        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        RaycastHit hit;
        // Ray cameraRay = FPScamera.ScreenPointToRay(centerPoint);

        //NOTE: Raycast offset is not working
        if (Physics.Raycast(transform.position, FPScamera.transform.forward, out hit, interactDistance))
        {

            Debug.Log("COLLISION WITH INTERACTABLE DETECTED: " + hit.collider.gameObject.name);

            Vector3 raycastOrigin = transform.position + raycastOffset;
            print("raycastOrigin: "+ raycastOrigin);

            //Debug.DrawRay(raycastOrigin, FPScamera.transform.forward, Color.red, 1f);

            if (currentlyInteractingGO != hit.collider.gameObject)
            {

                if (currentlyInteractingGO != null)
                {
                    currentlyInteractable.Unhighlight();
                }

                currentlyInteractingGO = hit.collider.gameObject;

                Interactable interactScript = currentlyInteractingGO.GetComponent<Interactable>();

                print("interactScript: " + interactScript);

                if (interactScript)
                {
                    currentlyInteractable = interactScript;
                    currentlyInteractable.Highlight();
                }
                else
                {
                    currentlyInteractingGO = null;
                }

            }

            if (_interactPressed && !GameManager.GamePaused)
            {
                currentlyInteractable.Interact();
            }
        }

        else
        {
            if (currentlyInteractingGO != null)
            {
                currentlyInteractable.Unhighlight();
                currentlyInteractingGO = null;
            }
        }

    }

    // ---------------------------------------------------------------
    // UPDATE METHODS THAT UPDATE GAMEMANAGER HUDS AND HANDLE EFFECTS OF LOW HEALTH/HUNGER/MORALE
    // ---------------------------------------------------------------

    private void UpdateHealth()
    {
        health = Mathf.Clamp(health, healthMin, healthMax);
        GameManager.UpdateHealthHud();
        //Note: Mathf.Clamp does not change the existing value, but rather returns a clamped value
        if (health <= healthMin)
        {
            Debug.Log("game over!");
            GameManager.EndGame();
        }
    }

    private void UpdateHunger()
    {
        hunger = Mathf.Clamp(hunger, hungerMin, hungerMax);
        GameManager.UpdateHungerHud();

        if (hunger <= hungerMin)
        {
            EscalateHealthDrain();
            EscalateMoraleDrain();
        }
        else
        {
            healthDrain = HEALTH_DRAIN_DEFAULT;
            moraleDrain = MORALE_DRAIN_DEFAULT;
        }

    }

    private void UpdateMorale()
    {
        Debug.Log("PlayerController.UpdateMorale() - this function calls GameManager.UpdateMoraleHud()");

        morale = Mathf.Clamp(morale, moraleMin, moraleMax);
        GameManager.UpdateMoraleHud();

        if (morale <= moraleMin)
        {
            EscalateHealthDrain();
        }
        else
        {
            healthDrain = HEALTH_DRAIN_DEFAULT;
        }
    }

    private void UpdateWarmth()
    {
        warmth = Mathf.Clamp(warmth, warmthMin, warmthMax);
        GameManager.UpdateWarmthHud();

        if (warmth <= warmthMin)
        {
            EscalateHealthDrain();
        }
        else
        {
            healthDrain = HEALTH_DRAIN_DEFAULT;
        }
    }

    private void EscalateHealthDrain()
    {
        healthDrain = (healthDrain == HEALTH_DRAIN_DEFAULT) ? HEALTH_DRAIN_2 : HEALTH_DRAIN_3;
    }

    private void EscalateMoraleDrain()
    {
        moraleDrain = MORALE_DRAIN_DEFAULT * 10;
    }

    // ---------------------------------------------------------------
    // DRAINS CALLED EVERY FRAME ON UPDATE
    // ---------------------------------------------------------------

    private void DrainAll()
    {
        DrainHealth();
        DrainHunger();
        DrainMorale();
    }

    private void DrainHealth()
    {
        DecreaseHealth(healthDrain);
    }

    private void DrainHunger()
    {
        DecreaseHunger(hungerDrain);
    }

    private void DrainMorale()
    {
        DecreaseMorale(moraleDrain);
    }

    private void DrainWarmth()
    {
        DecreaseWarmth(warmthDrain);
    }


    // ---------------------------------------------------------------
    // METHODS FOR DECREASING HEALTH, HUNGER, MORALE
    // ---------------------------------------------------------------

    public void DecreaseHealth(float amount)
    {
        health = (health - amount < 0) ? 0 : health - amount;
        UpdateHealth();
    }

    public void DecreaseHunger(float amount)
    {
        hunger = (hunger - amount < 0) ? 0 : hunger - amount;
        UpdateHunger();
    }

    public void DecreaseMorale(float amount)
    {
        Debug.Log("PlayerController.DecreaseMoral() by " + amount);
        morale = (morale - amount < 0) ? 0 : morale - amount;
        UpdateMorale();
    }

    public void DecreaseWarmth(float amount)
    {
        warmth = (warmth - amount < 0) ? 0 : warmth - amount;
        UpdateWarmth();
    }

    /*
    private void DecreaseProperty(out float originalValue, float amount)
    {
        original = originalValue;
        originalValue = original - amount;
    }
    */

    // ---------------------------------------------------------------
    // METHODS FOR INCREASING HEALTH, HUNGER, MORALE
    // ---------------------------------------------------------------

    public void IncreaseHealth(float value)
    {
        health = (health + value > healthMax) ? healthMax : health + value;
        UpdateHealth();
    }

    public void IncreaseHunger(float value)
    {
        hunger = (hunger + value > hungerMax) ? hungerMax : hunger + value;
        UpdateHunger();
    }

    public void IncreaseMorale(float value)
    {
        Debug.Log("PlayerController.IncreaseMoral() by " + value);

        morale = (morale + value > moraleMax) ? moraleMax : morale + value;
        UpdateMorale();
    }


    // ---------------------------------------------------------------
    // MISC GRAVEYARD
    // ---------------------------------------------------------------



    /** Method called after Awake and before Start. Called whenever Reset is clicked in Editor. */
    /*
     // Messing around with Reset method

    private void Reset()
    {
        Debug.Log("PlayerController.Reset()");

        Text[] texts = FindObjectsOfType<Text>();

        foreach (Text text in texts)
        {
            Debug.Log("text: "+text.name);
            switch (text.name)
            {
                case "Health Text":
                    healthText = text;
                    break;
            }
        }
    }

    */

    /* // moved to Interactable.cs

    private void onTriggerStay(Collider other)
    {
        Debug.Log("PlayerController.OnTriggerStay");
        Debug.Log("other: " + other.name);

        if (other.gameObject == this.gameObject)
        {
            return;
        }

        Interactable interactable = other.gameObject.GetComponent<Interactable>();


        if (interactPressed && interactable)
        {

            interactable.Interact();
        }

    }
    
     */


}
