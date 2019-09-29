using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSpawnArea : MonoBehaviour {

    public bool playerColliding;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerColliding = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerColliding = false;
        }
    }
}
