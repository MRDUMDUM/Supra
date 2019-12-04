using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSpawner : MonoBehaviour {


    public GameObject fire, water, lightning;
    string fireTag, waterTag, lightningTag;
    GameObject[] fireElements, waterElements, lightningElements;
    public int currentFire, currentWater, currentLightning;

    public BoxCollider spawnArea; // The area of which the elements should be in
    public float respawnCheckTimer; // How often to check for amount of elements alive

    public GameObject elementToSpawn; // Type of element to spawn
    public ParticleSystem spawnParticle; // What effect to produce when spawning
    public int maxElements; // Max amount of alive elements

    GameObject[] elements; // Used when searching for specific types of elements (holds the amount of elements found and used to count current elements alive);
    string elementType; // Type of element; will be set by getting the Tag of the elementToSpawn
    int curElements; // Current amount of alive elements

    GameObject player;

    private void Awake()
    {
        elementType = elementToSpawn.tag; // Set the type of element equal to the Tag (e.g. "Frost")
        fireTag = fire.tag;
        waterTag = water.tag;
        lightningTag = lightning.tag;
    }

    void Start() {
        StartCoroutine("checkElements"); // Start a coroutine (thread)

        player = GameObject.FindWithTag("Player");
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
                        respawnElements(fire); // Spawn one!
                    }
                    if (currentWater < maxElements) // if less alive than max alive
                    {
                        respawnElements(water); // Spawn one!
                    }
                    if (currentLightning < maxElements) // if less alive than max alive
                    {
                        respawnElements(lightning); // Spawn one!
                    }
                    if (curElements < maxElements) // if less alive than max alive
                    {
                        respawnElements(elementToSpawn); // Spawn one!
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

    void respawnElements(GameObject element)
    {   // Basically creates a clone of the "elementToSpawn" GameObject; inherits scripts and components!
        Instantiate(element, this.transform.position, this.transform.rotation);


    }
}
