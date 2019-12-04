using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class PlatformAttach : MonoBehaviour
{
    //public GameObject Player;
    public GameObject Camera;
    private void Start()
    {
        //Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.transform.parent = transform;
            Camera.GetComponent<AbstractTargetFollower>().m_UpdateType = AbstractTargetFollower.UpdateType.FixedUpdate;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
            Camera.GetComponent<AbstractTargetFollower>().m_UpdateType = AbstractTargetFollower.UpdateType.LateUpdate;
        }
    }
}
