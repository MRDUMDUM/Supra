using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalloutZone : MonoBehaviour {

    public Animator anim;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            anim.SetTrigger("Falling");
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
