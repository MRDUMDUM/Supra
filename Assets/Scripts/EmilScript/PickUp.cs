using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    GameObject lifter;
    Vector3 liftOffset;
    float offset = 4f;

    BoxCollider collider;
    bool inRange = false;
    bool carrying;

    // Use this for initialization
    void Start()
    {
        collider = GetComponent<BoxCollider>();
        lifter = GameObject.FindGameObjectWithTag("Player");
        GetComponent<Rigidbody>().useGravity = true;
           
    }
    // Update is called once per frame
    void Update()
    {
        liftOffset = new Vector3(lifter.transform.position.x, lifter.transform.position.y, lifter.transform.position.z + offset);

        if (inRange)
        {
            if (carrying == false)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    pickup();
                    carrying = true;
                }
            }
            else if (carrying == true)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    drop();
                    carrying = false;
                }
            }
        }
    }

    void pickup()
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        transform.position = liftOffset; // lifter.transform.position;
        transform.rotation = lifter.transform.rotation;
        transform.parent = lifter.transform;
    }

    void drop()
    {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        transform.parent = null;
        transform.position = liftOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
        inRange = true;
        Debug.Log("In range!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            Debug.Log("Out of range!");
        }
    }
}
