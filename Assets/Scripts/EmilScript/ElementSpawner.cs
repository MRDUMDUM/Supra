using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSpawner : MonoBehaviour {


    public GameObject fire, water, lightning, platform;
    string fireTag, waterTag, lightningTag;
    GameObject[] fireElements, waterElements, lightningElements;
    public int currentFire, currentWater, currentLightning, currentPlatform;
    public int maxFire, maxWater, maxLightning, maxPlatform;

    public BoxCollider spawnArea; // The area of which the elements should be in
    public float respawnCheckTimer; // How often to check for amount of elements alive

    public GameObject elementToSpawn; // Type of element to spawn
    public ParticleSystem spawnParticle; // What effect to produce when spawning
    public int maxElements; // Max amount of alive elements

    GameObject[] elements; // Used when searching for specific types of elements (holds the amount of elements found and used to count current elements alive);
    string elementType; // Type of element; will be set by getting the Tag of the elementToSpawn
    int curElements; // Current amount of alive elements

    public Transform fireSpawnArea;
    public Transform waterSpawnArea;
    public Transform electroSpawnArea;
    public Transform platformSpawnArea;

    GameObject player;

    public List<GameObject> numberOfWater = new List<GameObject>();
    public List<GameObject> numberOfFire = new List<GameObject>();
    public List<GameObject> numberOfElectro = new List<GameObject>();
    public List<GameObject> numberOfPlatform = new List<GameObject>();

    private void Awake()
    {
        elementType = elementToSpawn.tag; // Set the type of element equal to the Tag (e.g. "Frost")
        fireTag = fire.tag;
        waterTag = water.tag;
        lightningTag = lightning.tag;

        

    }

    void Start() {
        //StartCoroutine("checkElements"); // Start a coroutine (thread)

        player = GameObject.FindWithTag("Player");
        //fireSpawnArea = 
    }

    private void Update()
    {
        //check's if items still are on the list
        CurrentNumberCheck();
        Check();

        if (currentWater < maxWater)
        {
            
            Instantiate(water, waterSpawnArea.position, Quaternion.identity);
        }

        if(currentFire < maxFire)
        {
           Instantiate(fire, fireSpawnArea.position, Quaternion.identity);
        }

        if(currentLightning < maxLightning)
        {
             Instantiate(lightning, electroSpawnArea.position, Quaternion.identity);
        }

        if(currentPlatform < maxPlatform)
        {
            Instantiate(platform, platformSpawnArea.position, Quaternion.identity);
        }
        
        
    }

    void Check()
    {
        //numbers on list's updates current numbers
        currentWater = numberOfWater.Count;
        currentFire = numberOfFire.Count;
        currentLightning = numberOfElectro.Count;
        currentPlatform = numberOfPlatform.Count;
    }

    // Checks for whether the current amount of elements match the max amount
    IEnumerator checkElements()
    {

        while (true) // Infinite loop
        {
            if (spawnArea.GetComponent<ColliderSpawnArea>().playerColliding) // Only check if the player is colliding with the SpawnArea Collider box
            {
                if (elementType == null)
                {
                    Debug.Log("(ElementSpawner) elementType returns null.");
                }
                else
                {
                    elements = GameObject.FindGameObjectsWithTag(elementType); // Find all objects with corresponding Tag


                    //Daniel code starts here. try to do the same as above but with more. 
                    fireElements = GameObject.FindGameObjectsWithTag(fireTag);
                    waterElements = GameObject.FindGameObjectsWithTag(waterTag);
                    lightningElements = GameObject.FindGameObjectsWithTag(lightningTag);


                    foreach (GameObject element in elements) // Go through list of the found elements with corresponding Tag
                    {
                        curElements++; // and increment curElements for each one
                    }
                    foreach (GameObject element in fireElements) // Go through list of the found elements with corresponding Tag
                    {
                        currentFire++; // and increment curElements for each one
                    }
                    foreach (GameObject element in waterElements) // Go through list of the found elements with corresponding Tag
                    {
                        currentWater++; // and increment curElements for each one
                    }
                    foreach (GameObject element in lightningElements) // Go through list of the found elements with corresponding Tag
                    {
                        currentLightning++; // and increment curElements for each one
                    }

                    if (currentFire< maxElements) // if less alive than max alive
                    {
                        respawnElements(fire,fireSpawnArea); // Spawn one!
                    }
                    if (currentWater < maxElements) // if less alive than max alive
                    {
                        //respawnElements(water,waterSpawnArea); // Spawn one!
                    }
                    if (currentLightning < maxElements) // if less alive than max alive
                    {
                        respawnElements(lightning,electroSpawnArea); // Spawn one!
                    }
                    if (curElements < maxElements) // if less alive than max alive
                    {
                        respawnElements(elementToSpawn,this.transform); // Spawn one!
                    }
                    else
                    {
                        Debug.Log("(ElementSpawner) Max amount of " + elementType + " elements present (" + maxElements + ")!");
                    }

                    // Reset the list of elements found
                    elements = null;
                    curElements = 0;

                    fireElements = null;
                    waterElements = null;
                    lightningElements = null;

                    currentFire = 0;
                    currentWater = 0;
                    currentLightning = 0;
                }
            }
            else // Player not inside SpawnArea 
            {
                Debug.Log("Player not in Spawn Area.");
            }

                yield return new WaitForSeconds(respawnCheckTimer); // Wait for respawnCheckTimer
        }
    }

    void CurrentNumberCheck()
    {
        if (numberOfWater != null)
        {
            foreach (GameObject e in numberOfWater)
            {
                if (e == null)
                {
                    numberOfWater.Remove(e);
                }
            }
        }

        if (numberOfFire != null)
        {
            foreach (GameObject e in numberOfFire)
            {
                if (e == null)
                {
                    numberOfFire.Remove(e);
                }
            }
        }

        if (numberOfElectro != null)
        {
            foreach (GameObject e in numberOfElectro)
            {
                if (e == null)
                {
                    numberOfElectro.Remove(e);
                }
            }
        }

        if (numberOfPlatform != null)
        {
            foreach (GameObject e in numberOfPlatform)
            {
                if (e == null)
                {
                    numberOfPlatform.Remove(e);
                }
            }
        }
    }


    private void OnTriggerStay(Collider col)
    {
        switch (col.transform.tag)
        {
            case "Water":
                // col.GetComponent<TPS_Puddle>().connected.Add(this.gameObject);
                //connected.Add(col.gameObject);
                if (!numberOfWater.Contains(col.gameObject))
                {

                    numberOfWater.Add(col.gameObject);

                }
                break;
            case "Fire":
                if (!numberOfFire.Contains(col.gameObject))
                {

                    numberOfFire.Add(col.gameObject);

                }
                break;
            case "Electric":
                if (!numberOfElectro.Contains(col.gameObject))
                {

                    numberOfElectro.Add(col.gameObject);

                }
                break;
            case "NPC":
                if (!numberOfPlatform.Contains(col.gameObject))
                {

                    numberOfPlatform.Add(col.gameObject);

                }
                break;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        switch (col.transform.tag)
        {
            case "Water":
                // col.GetComponent<TPS_Puddle>().connected.Add(this.gameObject);
                //connected.Add(col.gameObject);
               

                    numberOfWater.Remove(col.gameObject);

               
                break;
            case "Fire":
                
               

                    numberOfFire.Remove(col.gameObject);

             
                break;
            case "Electric":
                
               

                    numberOfElectro.Remove(col.gameObject);

               
                break;
            case "NPC":
              
                    numberOfPlatform.Remove(col.gameObject);

           
                break;
        }
    }

    IEnumerator SpawnFire()
    {
        yield return new WaitForSeconds(respawnCheckTimer);
        Instantiate(fire, fireSpawnArea.position, Quaternion.identity);
    }

    IEnumerator SpawnWater()
    {
        yield return new WaitForSeconds(respawnCheckTimer);
        Instantiate(water,waterSpawnArea.position, Quaternion.identity);
    }

    IEnumerator SpawnLightning()
    {
        yield return new WaitForSeconds(respawnCheckTimer);
        Instantiate(lightning, electroSpawnArea.position, Quaternion.identity);
    }




    void respawnElements(GameObject element,Transform spawnPosition)
    {   // Basically creates a clone of the "elementToSpawn" GameObject; inherits scripts and components!
        Instantiate(element, spawnPosition.position, this.transform.rotation);


    }
}
