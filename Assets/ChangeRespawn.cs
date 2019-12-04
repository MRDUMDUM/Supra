using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRespawn : MonoBehaviour
{

    public GameObject spawnPoint;
    public GameObject healthManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = this.gameObject.transform.GetChild(0).gameObject;
        healthManager = FindObjectOfType<HealthManager>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            healthManager.GetComponent<HealthManager>().respawnPoint = spawnPoint.transform.position;
            healthManager.GetComponent<HealthManager>().respawnRotation = spawnPoint.transform.rotation;
        }
    }
}
