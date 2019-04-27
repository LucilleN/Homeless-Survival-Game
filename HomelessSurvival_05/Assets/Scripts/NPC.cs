using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    private static int DEFAULT_NICENESS = 10;
    private static int MAX_NICENESS = 10;
    private static int MIN_NICENESS = 0;
    public int niceness = 10;

    public string[] dialogue = new string[] {
        "\"How are you today?\"",
        "\"Have a lovely day!\"",
        "\"Sorry, I can't talk right now.\"",
        "\"Get out of here!\"",
        "\"You are filthy.\"",
        "\"People like you are the problem with this city.\"",
        "\"Get out of here, hobo!\"",
        "\"Don't talk to me.\"",
        "\"Bug off!\"",
        "\"Why can't you just get a job, you lazy bum?\""
    };

    public string offerItem = "\"Here, you can have this.\"";

    /*
    public string[] dialogueKind = {
        "Here, you can have this.",
        "How are you today?",
        "Have a lovely day!",
    };

    public string[] dialogueNeutral = {
        "Sorry, I can't talk right now.",
    };

    public string[] dialogueAngry = {
        "Get out of here!",
        "You are filthy.",
        "People like you are the problem with this city."
    };
    */

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        niceness = DEFAULT_NICENESS;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        SpeakRandom();
    }

    private void SpeakRandom()
    {
        int index = Random.Range(0, this.dialogue.Length);
        string message = dialogue[index];
        GameManager.Instance.CreateModal(message);
    }

}
