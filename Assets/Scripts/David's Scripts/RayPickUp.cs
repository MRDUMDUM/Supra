using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayPickUp : MonoBehaviour {

    public Animator anim;

	public float Range = 10;
    public float offset = 4;

	public Component startPos;
	public Component endPos;
    public Component pickupPos;

    //placemarker
	public GameObject placement;
    public GameObject placementDinaler;
    public GameObject PlayerCharecter;

    private GameObject NPC; 

    Vector3 placeAt;
	Vector3 PlaceStart;
	Vector3 placeDirection;
    Vector3 liftOffset;

    Collider collider;
    
    RaycastHit hit;

    bool inRange = false;
    bool carrying;
    bool cantPlace;
    bool wall = false;


    // Use this for initialization
    void Start () {

        //NPC = GameObject.FindGameObjectWithTag("NPC");
        collider = GetComponent<SphereCollider>();
        //NPC.GetComponent<Rigidbody>().useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {

        placementTracker();
        if (NPC != null)
        {
            if (NPC.CompareTag("Fire"))
            {
                if (NPC.GetComponent<FireNPC>().isSleeping == true)
                {
                    if (inRange)
                    {
                        if (carrying == false)
                        {
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                anim.SetTrigger("PickUp");
                                Pickup();
                                carrying = true;
                                placement.SetActive(true);
                            }
                        }
                        else if (carrying == true && cantPlace == false)
                        {
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                anim.SetTrigger("SetDown");
                                Drop();
                                carrying = false;
                                placement.SetActive(false);
                                //collider.enabled = true;
                            }
                        }
                    }
                }
                else
                {
                    if (carrying == true)
                    {
                        anim.SetTrigger("SetDown");//needs a new animation here
                                                   // NPC.GetComponent<Collider>().enabled = true;
                        placement.SetActive(false);
                        carrying = false;
                        NPC.transform.parent = null;
                    }
                }
            }
            if (NPC.CompareTag("NPC"))
            {
                if (NPC.GetComponent<AIPlatform>().isSleeping == true || NPC.GetComponent<FireNPC>().isSleeping == true)
                {
                    if (inRange)
                    {
                        if (carrying == false)
                        {
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                anim.SetTrigger("PickUp");
                                Pickup();
                                carrying = true;
                                placement.SetActive(true);
                                //collider.enabled = false;
                            }
                        }
                        else if (carrying == true && cantPlace == false)
                        {
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                anim.SetTrigger("SetDown");
                                Drop();
                                carrying = false;
                                placement.SetActive(false);
                                //collider.enabled = true;
                            }
                        }
                    }
                }
                else
                {
                    if (carrying == true)
                    {
                        anim.SetTrigger("SetDown");//needs a new animation here
                                                   // NPC.GetComponent<Collider>().enabled = true;
                        placement.SetActive(false);
                        carrying = false;
                        NPC.transform.parent = null;
                    }
                }
            }
        }
    }
    //picup tranform object onto parent 
    void Pickup()
    {
        NPC.GetComponent<Rigidbody>().useGravity = false;
        NPC.GetComponent<Rigidbody>().isKinematic = true;
        //NPC.GetComponent<Collider>().enabled = false;
        NPC.transform.position = liftOffset; // lifter.transform.position;
        NPC.transform.rotation = PlayerCharecter.transform.rotation;
        NPC.transform.parent = PlayerCharecter.transform;
        
    }

    void Drop()
    {
        NPC.GetComponent<Rigidbody>().useGravity = true;
        NPC.GetComponent<Rigidbody>().isKinematic = false;
       // NPC.GetComponent<Collider>().enabled = true;
        NPC.transform.parent = null;
        NPC.transform.position = placeAt;
        NPC.transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
    }

    void placementTracker()
    {
        liftOffset = pickupPos.transform.position;
        //start position for raycast
        PlaceStart = startPos.transform.position;
        //pointing direction for raycast
        placeDirection = (endPos.transform.position - startPos.transform.position).normalized;
        //shows raycast in scene on playMode
        Debug.DrawRay(PlaceStart, placeDirection * 20, Color.red);
        //stores vector3 point for placement 
        RaycastHit hit;

        if (Physics.Raycast(PlaceStart, placeDirection, out hit, Range))
        {

            //Places placmarker at given hit.point and offset with 2 in the Y direction
            //And rotate it to match the the surface of the normal the raycating is hitting
            placeAt = hit.point + new Vector3(0,2,0);
            placement.transform.position = placeAt;
            placement.transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            placementDinaler.transform.position = placeAt;
            placementDinaler.transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal);

            //placement.transform.localRotation = hit.normal;

            //if z rotaion change value the placer is hitting a wall or 90 degree surface
            if ((placement.transform.eulerAngles.z <= 80 && placement.transform.eulerAngles.z >= -80) || (placement.transform.eulerAngles.z >= 100 && placement.transform.eulerAngles.z <= 260) || (placement.transform.eulerAngles.z >= 280 && placement.transform.eulerAngles.z <= 360))
            {
                wall = false;
            }
            else
            {
                wall = true;
            }
        
            cantPlace = false;

           
            //shows placemarker
            if(carrying == true && wall == false)
            {
                //shows placemarker when carring NPC
                placement.SetActive(true);
                placementDinaler.SetActive(false);
                cantPlace = false;
            }
            else if(carrying == true && wall == true)
            {
                placement.SetActive(false);
                placementDinaler.SetActive(true);
                cantPlace = true;
            }
           
        }
        else
        {
            //if nothing is in range hide placemarker
            placement.SetActive(false);
            cantPlace = true;
        }
    }

    //stores object colliding with player pickup colider
    void OnTriggerStay(Collider COLin)
    {
        if (NPC == null && carrying == false)
        {
            if (COLin.CompareTag("NPC"))
            {
                NPC = COLin.transform.root.gameObject;
               // NPC = COLin.gameObject;
                inRange = true;
                Debug.Log("noget sketet");
            }
            if (COLin.CompareTag("Fire"))
            {
                NPC = COLin.transform.root.gameObject;
                inRange = true;
                
            }
        }
        
    }

    void OnTriggerExit(Collider COLout)
    {
        if (carrying == false)
        {
            if (COLout.CompareTag("NPC"))
            {
                NPC = null;
                inRange = false;
                Debug.Log("out!");
            }
            if (COLout.CompareTag("Fire"))
            {
                NPC = null;
                inRange = false;
                Debug.Log("out!");
            }
        }
    }

}
