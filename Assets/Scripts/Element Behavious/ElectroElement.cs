using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroElement : MonoBehaviour
{

    public ParticleSystem electroPar;
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
        switch (collision.transform.tag)
        {
            case "WaterPuddle":
                ThrowME.ball = null;
                Destroy(this.gameObject);
                break;
            case "Machine":
                ThrowME.ball = null;
                Destroy(this.gameObject);
                break;
            case "Ground":

                ThrowME.ball = null;
                Destroy(this.gameObject);
                break;

            default:

                ThrowME.ball = null;
                Instantiate(electroPar, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
                break;
        }
    }
}
