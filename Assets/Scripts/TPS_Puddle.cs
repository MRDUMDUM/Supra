using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Puddle : MonoBehaviour
{

    public GameObject player;
    public float PlayerFadeDis;

    //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
    //Stand in code to be replaced in the future!!!
    //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
    
    public bool electricOn = false;
    public bool StartElectro = false;

    float duration = 5f;
    Renderer rend;
    public ParticleSystem electroPar;

    public List<GameObject> connected = new List<GameObject>();

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        //colorStart = GetComponent<Material>().color;
        rend = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (electricOn)
        {
            
            Instantiate(electroPar, transform.position, Quaternion.identity);
            
            if (StartElectro)
            {
                ElectrifiedLink();
            }
            electricOn = false;
        }

        RemoveObject();
    }

    void ElectrifiedLink()
    {
        
        foreach(GameObject e in connected)
        {
            e.GetComponent<TPS_Puddle>().electricOn = true;
        }
        StartElectro = false;
    }

    void RemoveObject()
    {

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        
        if(distanceToPlayer > PlayerFadeDis)
        {
            Destroy(this.gameObject);
        }
    }

    void ConnectedCheck()
    {
        if(connected != null)
        {

        }
    }

    private void OnTriggerEnter(Collider col)
    {
        switch (col.transform.tag)
        {
            case "WaterPuddle":
                col.GetComponent<TPS_Puddle>().connected.Add(this.gameObject);
                connected.Add(col.gameObject);

                break;

            case "Fire":

                Destroy(this.gameObject);
                break;
            case "Electric":

                electricOn = true;
                StartElectro = true;
                break;


            default:

               
                break;
        }
    }

    
    
}
