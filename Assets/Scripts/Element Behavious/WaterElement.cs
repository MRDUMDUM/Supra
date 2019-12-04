using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterElement : MonoBehaviour
{
  
    public GameObject WaterPudle;
    public GameObject lavaPlatform;


    public float pudleScale = 1.0f;

    public LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
  //      // Get how many splats are in the splat atlas
		//splatsX = SplatManagerSystem.instance.splatsX;
		//splatsY = SplatManagerSystem.instance.splatsY;
    }

    private void OnCollisionEnter(Collision collision)
    {


       
        switch (collision.transform.tag)
        {
            case "Fire":
                break;

            case "Lava":
                Instantiate(lavaPlatform, this.transform.position, Quaternion.identity);
                ThrowME.ball = null;
                Destroy(this.gameObject);
                break;

            default:
                Instantiate(WaterPudle, this.transform.position, Quaternion.identity);
                ThrowME.ball = null;
                //WaterPudleRay();
                Destroy(this.gameObject);
                break;
        }
    }


}
