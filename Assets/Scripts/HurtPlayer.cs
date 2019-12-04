using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{

    public int damageToGive = 1;

    public bool lava = false;
    public float lavaForce;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            Vector3 hitDirection = col.transform.position - transform.position;
            
            if (lava)
            {
                hitDirection += new Vector3(0, lavaForce, 0);
                Debug.Log("yes");
            }
            hitDirection = hitDirection.normalized;
            FindObjectOfType<HealthManager>().HurtPlayer(damageToGive,hitDirection);
        }

    }
    
}
