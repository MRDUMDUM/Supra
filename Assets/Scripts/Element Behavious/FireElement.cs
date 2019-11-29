using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {


        Debug.Log("sker der noget?");
        switch (collision.transform.tag)
        {
            case "Water":
                //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                //Stand in code to be replaced in the future!!!
                //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                

                ThrowME.ball = null;
                Destroy(this.gameObject);
                break;

            case "Ground":
                
                ThrowME.ball = null;
                Destroy(this.gameObject);
                break;

            default:
               
                ThrowME.ball = null;
                //WaterPudleRay();
                Destroy(this.gameObject);
                break;
        }
    }
}
