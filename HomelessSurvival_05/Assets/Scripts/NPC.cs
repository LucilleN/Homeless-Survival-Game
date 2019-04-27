using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    public GameObject spawnSpot;

    public GameObject[] collectibles = new GameObject[2]; // Only 2 because an NPC can only give money or food

    private static int DEFAULT_NICENESS = 10;
    private static int MAX_NICENESS = 10;
    private static int MIN_NICENESS = 0;
    public int niceness = DEFAULT_NICENESS;

    private static int NICENESS_TIME_DEFAULT = 1000;
    public int nicenessTime = NICENESS_TIME_DEFAULT;

    public string[] dialogueNice = new string[] {
        "\"How are you today?\"",
        "\"Have a lovely day!\"",
        "\"Weather's been up and down, huh?\"",
        "\"Godspeed, friend!\"",
    };

    public string[] dialogueNeutral = new string[] {
        "\"Sorry, I can't talk right now.\"",
        "\"Can it really be that hard to find a job?\"",
        "\"Why do I keep seeing you around?\"",
    };

    public string[] dialogueMean = new string[] {
        "\"Get out of here!\"",
        "\"You are filthy.\"",
        "\"Jesus, you stink! Take a damn shower.\"",
        "\"People like you are the problem with this city.\"",
        "\"Get out of here, hobo!\"",
        "\"Don't talk to me.\"",
        "\"Bug off!\"",
        "\"Why can't you just get a job, you lazy bum?\""
    };

    public string offerItem = "\"Here, you can have this.\"";

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        niceness = RandomNiceness();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateNicenessTime();
    }

    public override void Interact()
    {
        if (IsFacing())
        {
            SpeakRandom();
            DecreaseNiceness();
        }
    }

    private void SpeakRandom()
    {
        int moraleChange = 0;

        if (niceness == 10 && spawnSpot)
        {
            int chance = Random.Range(0, 1);
            if (chance == 1)
            {
                OfferItem();
                moraleChange = 10;
                GameManager.Instance.player.IncreaseMorale(moraleChange);
                GameManager.UpdateAllHud();
                return;
            }
        }

        string[] options;

        if (niceness >= 8)
        {
            options = dialogueNice;
            moraleChange = 5;
        } 
        else if (niceness >= 6)
        {
            options = dialogueNeutral;
            moraleChange = 1;
        }
        else
        {
            options = dialogueMean;
            moraleChange = -5;
        }

        int index = Random.Range(0, options.Length);
        string message = options[index];
        Speak(message);

        if (moraleChange > 0)
        {
            GameManager.Instance.player.IncreaseMorale(moraleChange);
        }
        else if (moraleChange < 0)
        {
            GameManager.Instance.player.DecreaseMorale(-moraleChange);
        }
        GameManager.UpdateAllHud();

    }

    private int RandomNiceness()
    {
        return Random.Range(MIN_NICENESS, MAX_NICENESS);
    }

    private void DecreaseNiceness()
    {
        if (niceness > 0)
        {
            niceness--;
        }
    }

    private void IncreaseNiceness()
    {
        if (niceness < 10)
        {
            niceness++;
        }
    }

    private void OfferItem()
    {
        Speak(offerItem);
        //instantiate an item right in front of them
        int GOindex = Random.Range(0, collectibles.Length-1);
        GameObject newCollectible = Instantiate(collectibles[GOindex], spawnSpot.transform.position, spawnSpot.transform.rotation);
    }

    private void Speak(string message)
    {
        GameManager.Instance.CreateModal(message);
    }

    private void UpdateNicenessTime()
    {
        if (nicenessTime > 0)
        {
            nicenessTime--;
        } 
        else
        {
            IncreaseNiceness();
            nicenessTime = NICENESS_TIME_DEFAULT;
        }
    }

    public bool IsFacing()
    {
        Vector3 playerForward = GameManager.Instance.player.transform.forward;
        Vector3 npcForward = this.gameObject.transform.forward;
        Vector3 sum = playerForward + npcForward;
        return sum.magnitude <= Mathf.Max(playerForward.magnitude, npcForward.magnitude);
    }
}
