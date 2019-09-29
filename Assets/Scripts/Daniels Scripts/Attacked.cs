using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacked : MonoBehaviour {
    public GameObject blood;
    
    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
       
	}
    private void OnCollisionEnter(Collision coll)
    {
        
        if (coll.gameObject.tag == "Weapon")
        {
            Instantiate(blood, transform.position, transform.rotation);
            Debug.Log("HIT");
        }





    }
}
