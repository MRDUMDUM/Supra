using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollow : MonoBehaviour {

    public UnityStandardAssets.Utility.WaypointCircuit circuit;
    int currentWP = 0;

    public float speed = 3.0f;
    public float accuracy = 1.0f;
    public float rotSpeed = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

        if (circuit.Waypoints.Length == 0)
            return;

        Vector3 lookAtGoal = new Vector3(this.transform.position.x, circuit.Waypoints[currentWP].position.y, circuit.Waypoints[currentWP].position.z);

        Vector3 direction = lookAtGoal - this.transform.position;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);

        if(direction.magnitude< accuracy)
        {
            currentWP++;
            if(currentWP >= circuit.Waypoints.Length)
            {
                currentWP = 0;
            }
        }

        this.transform.Translate(0, 0, speed * Time.deltaTime);

    }
}
