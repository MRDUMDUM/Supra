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
    public Component ElementPickup;

    //placemarker
	public GameObject placement;
    public GameObject placementDinaler;
    public GameObject PlayerCharecter;
    public GameObject PlayerHand;


    private GameObject Platform;
    private GameObject Element;

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
    bool aiming = false;


    // Use this for initialization
    void Start () {
        
        //NPC = GameObject.FindGameObjectWithTag("NPC");
        collider = GetComponent<SphereCollider>();
        //NPC.GetComponent<Rigidbody>().useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {

        
        if (Platform != null)
        {
            placementTracker();
            if (Platform.CompareTag("Fire"))
            {
                if (Platform.GetComponent<FireNPC>().isSleeping == true)
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
                        Platform.transform.parent = null;
                    }
                }
            }
            if (Platform.CompareTag("NPC"))
            {
                if (Platform.GetComponent<AIPlatform>().isSleeping == true || Platform.GetComponent<FireNPC>().isSleeping == true)
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
                        Platform.transform.parent = null;
                    }
                }
            }
        }

        if(Element != null)
        {
            if (inRange)
            {
                if (carrying == false)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        anim.SetTrigger("PickUp");
                        PickUpElement();
                        carrying = true;
                        //placement.SetActive(true);
                    }
                }
                else if (carrying == true)
                {
                    Debug.Log("Carring true");
                    //Toggles between aiming and throwing
                    if (!aiming)
                    {
                        if(Input.GetMouseButtonDown(0))
                        {
                            AimElement();
                        }
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            ThrowElement();
                        }
                    }
                    
                    
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        anim.SetTrigger("SetDown");
                        DropElement();
                        aiming = false;
                        carrying = false;
                        //placement.SetActive(false);
                        //collider.enabled = true;
                    }
                }
            }
        }
    }
    //picup tranform object onto parent 
    void Pickup()
    {
        Platform.GetComponent<Rigidbody>().useGravity = false;
        Platform.GetComponent<Rigidbody>().isKinematic = true;
        //NPC.GetComponent<Collider>().enabled = false;
        Platform.transform.position = liftOffset; // lifter.transform.position;
        Platform.transform.rotation = PlayerCharecter.transform.rotation;
        Platform.transform.parent = PlayerCharecter.transform;
        
    }

    void Drop()
    {
        Platform.GetComponent<Rigidbody>().useGravity = true;
        Platform.GetComponent<Rigidbody>().isKinematic = false;
       // NPC.GetComponent<Collider>().enabled = true;
        Platform.transform.parent = null;
        Platform.transform.position = placeAt;
        Platform.transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
    }

    void PickUpElement()
    {
        Element.GetComponent<Rigidbody>().useGravity = false;
        Element.GetComponent<Rigidbody>().isKinematic = true;
        
        Element.transform.parent = PlayerHand.transform;
        
        Element.transform.position = ElementPickup.transform.position;
        
    }

    void DropElement()
    {
        Element.transform.parent = null;
        Element.GetComponent<Rigidbody>().useGravity = true;
        Element.GetComponent<Rigidbody>().isKinematic = false;
    }

    void AimElement()
    {
        aiming = true;
        
        Debug.Log("TAke AIM!!");
    }

    void ThrowElement()
    {
        aiming = false;
        Debug.Log("YEEEET");
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
        if (Platform == null && Element == null && carrying == false )
        {
            if (COLin.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                Platform = COLin.transform.root.gameObject;
               // NPC = COLin.gameObject;
                inRange = true;
                Debug.Log("noget sketet");
            }
            if (COLin.CompareTag("Fire"))
            {
                Platform = COLin.transform.root.gameObject;
                inRange = true;
                
            }
            if (COLin.gameObject.layer == LayerMask.NameToLayer("Element"))
            {
                Element = COLin.transform.root.gameObject;
                inRange = true;
                Debug.Log("tag mig!");
            }
        }
        
    }

    void OnTriggerExit(Collider COLout)
    {
        if (carrying == false)
        {
            if (COLout.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                Platform = null;
                inRange = false;
                Debug.Log("out!");
            }
            if (COLout.CompareTag("Fire"))
            {
                Platform = null;
                inRange = false;
                Debug.Log("out!");
            }
            if (COLout.gameObject.layer == LayerMask.NameToLayer("Element"))
            {
                Element = null;
                inRange = false;
            }
        }
    }

}
