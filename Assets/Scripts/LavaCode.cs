using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaCode : MonoBehaviour
{

   // Vector3 position;

    public GameObject lavaPlatform;
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
        if(col.gameObject.CompareTag("Water"))
        {
            Instantiate(lavaPlatform, col.transform.position, Quaternion.identity);
            ThrowME.ball = null;
            Destroy(col);

        }
        
    }
}
