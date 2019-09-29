using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gettingHit : MonoBehaviour {

    public Material[] material;
    Renderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        rend.sharedMaterial = material[0];
	}

    private void OnCollisionEnter(Collision collision)
    {
        rend.sharedMaterial = material[1];
    }

    // Update is called once per frame
    void Update () {
		
	}
}
