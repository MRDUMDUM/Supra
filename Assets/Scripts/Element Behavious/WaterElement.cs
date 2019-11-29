using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterElement : MonoBehaviour
{
  
    public GameObject WaterPudle;


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


        Debug.Log("sker der noget?");
        switch (collision.transform.tag)
        {
            case "Fire":
                break;

            case "Ground":
                Instantiate(WaterPudle, this.transform.position, Quaternion.identity);
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

    //void WaterPudleRay()
    //{
    //    Ray ray = new Ray(transform.position, Vector3.down);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit, 100, layerMask))
    //    {

    //        Vector3 leftVec = Vector3.Cross(hit.normal, Vector3.up);
    //        float randScale = Random.Range(0.5f, 1.5f);

    //        GameObject newSplatObject = new GameObject();
    //        newSplatObject.transform.position = hit.point;
    //        if (leftVec.magnitude > 0.001f)
    //        {
    //            newSplatObject.transform.rotation = Quaternion.LookRotation(leftVec, hit.normal);
    //        }
    //        newSplatObject.transform.RotateAround(hit.point, hit.normal, Random.Range(-180, 180));
    //        newSplatObject.transform.localScale = new Vector3(randScale, randScale * 0.5f, randScale) * pudleScale;

    //        Splat newSplat;
    //        newSplat.splatMatrix = newSplatObject.transform.worldToLocalMatrix;
    //        newSplat.channelMask = channelMask;

    //        float splatscaleX = 1.0f / splatsX;
    //        float splatscaleY = 1.0f / splatsY;
    //        float splatsBiasX = Mathf.Floor(Random.Range(0, splatsX * 0.99f)) / splatsX;
    //        float splatsBiasY = Mathf.Floor(Random.Range(0, splatsY * 0.99f)) / splatsY;

    //        newSplat.scaleBias = new Vector4(splatscaleX, splatscaleY, splatsBiasX, splatsBiasY);

    //        SplatManagerSystem.instance.AddSplat(newSplat);
    //        Debug.Log("YAA");
    //        GameObject.Destroy(newSplatObject);

    //    }
    //    else
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}

}
