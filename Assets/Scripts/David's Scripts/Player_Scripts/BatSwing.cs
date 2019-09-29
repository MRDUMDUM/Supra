using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSwing : MonoBehaviour {

    public float force;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(Vector3.up);
        if (Input.GetButton("Fire1"))
        {

        }
	}
    private void OnCollisionEnter(Collision col)
    {
            
            if (col.gameObject.tag == "NPC")
            {
                Debug.Log("Collision!");
                //Vector3 hitForce = new Vector3()
                col.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up*force,ForceMode.Impulse);
               
            }
        
    }
}
