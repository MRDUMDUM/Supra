using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUP : MonoBehaviour {

    public Transform player;
    public Transform playerCam;
    //public GameObject playerCam;
    public float throwForce = 50;
   
    bool hasPlayer = false;
    bool beingCarried = false;
    private bool touched = false;

    void Start()
    {
        // audio = GetComponent<AudioSource>();
        //  playerCam = GameObject.FindWithTag("Camera");
        playerCam = GameObject.FindWithTag("MainCamera").transform;
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {

     
        float dist = Vector3.Distance(gameObject.transform.position, player.position);
        if (dist <= 7.0f)
        {
            hasPlayer = true;
        }
        else
        {
            hasPlayer = false;
        }
        if (hasPlayer && Input.GetKeyDown(KeyCode.E))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.parent = playerCam;
            beingCarried = true;
        }
        if (beingCarried)
        {
            if (touched)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
                touched = false;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
              
                GetComponent<Rigidbody>().isKinematic = false;
            
                transform.parent = null;
                beingCarried = false;
                GetComponent<Rigidbody>().AddForce(playerCam.forward * throwForce,ForceMode.Impulse);
             
            }
            else if (Input.GetMouseButtonDown(0))
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
            }
        }
    }
    void OnTriggerEnter()
    {
        if (beingCarried)
        {
            touched = true;
        }
    }
}
