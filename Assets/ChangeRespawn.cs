using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRespawn : MonoBehaviour
{

    public GameObject spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = this.gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("HealthManager").GetComponent<HealthManager>().respawnPoint = spawnPoint.transform.position;
            GameObject.FindGameObjectWithTag("HealthManager").GetComponent<HealthManager>().respawnRotation = spawnPoint.transform.rotation;
        }
    }
}
