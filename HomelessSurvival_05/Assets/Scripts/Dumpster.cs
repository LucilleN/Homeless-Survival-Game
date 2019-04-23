using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dumpster : Interactable
{
    /*
    public Collectible.CollectibleType[] collectibleTypes = {
        Collectible.CollectibleType.Clothing,
        Collectible.CollectibleType.Food,
        Collectible.CollectibleType.Material,
        Collectible.CollectibleType.Money
    };
    */

    public GameObject[] collectibles = new GameObject[3];
    public GameObject spawnSpot;

    private int SPAWN_TIME_MIN = 50; //5000
    private int SPAWN_TIME_MAX = 150; //15000
    private int spawnTime;

    public List<int> internalItems = new List<int>();

    [NonSerialized] public Color highlightColor = new Color(0.18f, 0.83f, 0f);
    private Color defaultColor;

    private Vector3 defaultScale;
    public float hScaleFactor = 1.1f;
    private Vector3 highlightScale;

    private Material originalMaterial;
    //private Material highlightMaterial = new Material();

    private float forceYmin = 2f;
    private float forceYmax = 5f;
    private float forceXmin = 20f;
    private float forceXmax = 25f;
    private float forceZmin = 15f;
    private float forceZmax = 20f;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ResetSpawnTime();

        originalMaterial = this.gameObject.GetComponent<Renderer>().material;
        defaultColor = originalMaterial.GetColor("_EmissionColor");
        //print(gameObject.name+": "+originalMaterial);
        transf = this.gameObject.transform;

        defaultScale = transf.localScale;
        highlightScale = new Vector3(defaultScale.x * hScaleFactor, defaultScale.y * hScaleFactor, defaultScale.z * hScaleFactor);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime--;
        if (spawnTime <= 0)
        {
            GenerateInternalItem();
            ResetSpawnTime();
        }
    }

    public override void Interact()
    {
        InstantiateItem();
        base.Interact();
    }

    public override void Highlight()
    {
        Debug.Log("Dumpster.Highlight()");
        transf.localScale = highlightScale;
        originalMaterial.SetColor("_EmissionColor", highlightColor * 0.5f);
    }

    public override void Unhighlight()
    {
        Debug.Log("Dumpster.Unhighlight()");

        transf.localScale = defaultScale;
        originalMaterial.SetColor("_EmissionColor", defaultColor);
        //originalMaterial.color = defaultColor;
    }


    private void GenerateInternalItem()
    {
        int index = UnityEngine.Random.Range(0, collectibles.Length); //or collectibles.Length, not sure which
        internalItems.Add(index);
    }


    private void InstantiateItem()
    {
        Debug.Log("Dumpster.InstantiateItem()");
        Debug.Log("internalItems: " + internalItems.ToString() + ", count: " + internalItems.Count);
        if (internalItems.Count <= 0)
        {
            return;
        }
        int GOindex = internalItems[0];
        GameObject newCollectible = Instantiate(collectibles[GOindex], spawnSpot.transform.position, spawnSpot.transform.rotation);
        //newCollectible.transform.position = spawnSpot.transform.position;
        //the rotation of the newCollectible needs to be the opposite of whatever the dumpster's rotation is to get the thing zeroed out
        //newCollectible.transform.rotation = new Vector3(-transform.rotation.x, -transform.rotation.y, -transform.rotation.z);
        //newCollectible.transform.rotation = Transform.InverseTransformDirection(transform.rotation);
        //newCollectible.transform.rotation = new Vector3(90f, 0f, 0f);

        Vector3 upAndForward = spawnSpot.transform.right + spawnSpot.transform.up;
        print("spawnSpot.transform.rotation: " + spawnSpot.transform.rotation);
        print("up: " + spawnSpot.transform.up);
        print("forward: " + spawnSpot.transform.right);
        print("upAndForward: " + upAndForward);

        newCollectible.GetComponent<Rigidbody>().AddForce(upAndForward*50);
        internalItems.RemoveAt(0);
    }

    private Vector3 RandomLaunchForce()
    {
        
        float forceY = UnityEngine.Random.Range(forceYmin, forceYmax);
        float forceX = UnityEngine.Random.Range(forceXmin, forceXmax);
        float forceZ = UnityEngine.Random.Range(forceZmin, forceZmax);

        return new Vector3(forceX, forceY, forceZ);
                
    }

    private void ResetSpawnTime()
    {
        spawnTime = UnityEngine.Random.Range(SPAWN_TIME_MIN, SPAWN_TIME_MAX);
    }

}
