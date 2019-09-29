using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPS_Boxes : MonoBehaviour {

    NavMeshAgent pathfinder;

    // Use this for initialization
    void Start () {
        pathfinder = GetComponent<NavMeshAgent>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
