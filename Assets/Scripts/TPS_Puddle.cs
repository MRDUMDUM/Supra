using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Puddle : MonoBehaviour
{
    //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
    //Stand in code to be replaced in the future!!!
    //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&

    public GameObject player;
    public float PlayerFadeDis;
    public int damageToGive = 1;
    public bool electricOn = false;
    public bool AlreadyElectro = false;
    //public bool machineConnected = false;
    float duration = 5f;
    Renderer rend;
    public ParticleSystem electroPar;
    public ParticleSystem DAmp;

    public List<GameObject> connected = new List<GameObject>();
    public GameObject machine;

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
            
            if(machine != null)
            {
                machine.GetComponent<Conductive>().isPowered = true;
            }
            
            ElectrifiedLink();
            StartCoroutine(TurnOff());
            electricOn = false;

        }

        RemoveObject();
        ConnectedCheck();
    }

    void ElectrifiedLink()
    {
        
        foreach(GameObject e in connected)
        {
            if(e.GetComponent<TPS_Puddle>().AlreadyElectro != true)
            {
                e.GetComponent<TPS_Puddle>().electricOn = true;
                e.GetComponent<TPS_Puddle>().AlreadyElectro = true; 
            }
            
        }
       
        
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
            foreach (GameObject e in connected)
            {
                if (e == null)
                {
                    connected.Remove(e);
                }
            }
        }

    }

    private void OnTriggerEnter(Collider col)
    {
        switch (col.transform.tag)
        {
            case "WaterPuddle":
                // col.GetComponent<TPS_Puddle>().connected.Add(this.gameObject);
                //connected.Add(col.gameObject);
                if (!connected.Contains(col.gameObject))
                {

                    connected.Add(col.gameObject);

                }
                break;

            case "Fire":

                Destroy(this.gameObject);
                break;
            case "Electric":

                electricOn = true;
                AlreadyElectro = true;
                break;
            case "player":
                if (electricOn)
                {


                    Vector3 hitDirection = col.transform.position - transform.position;
                    hitDirection += new Vector3(0, 1, 0);
                    hitDirection = hitDirection.normalized;

                    FindObjectOfType<HealthManager>().HurtPlayer(damageToGive,hitDirection);
                }

                break;
            
            default:

               
                break;

        }

        if(col.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {

            col.gameObject.GetComponentInParent<AIPlatform>().WakeUp();
        }
    }
    
    

    private void OnTriggerStay(Collider col)
    {
        
        if(col.transform.tag == "WaterPuddle")
        {
            if (!connected.Contains(col.gameObject))
            {
                
                connected.Add(col.gameObject);

            }

        }

        if (col.transform.tag == "Player")
        {
            if (electricOn)
            {
                Vector3 hitDirection = col.transform.position - transform.position;
                hitDirection += new Vector3(0, 1, 0);
                hitDirection = hitDirection.normalized;
                FindObjectOfType<HealthManager>().HurtPlayer(damageToGive, hitDirection);
            }
        }

        if (col.transform.tag == "Machine")
        {
            machine = col.gameObject;
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            col.gameObject.GetComponent<AIPlatform>().WakeUp();
        }

    }

    private void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "WaterPuddle")
        {
            connected.Remove(col.gameObject);
        }
    }

    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(1.5f);
        AlreadyElectro = false;
    }
}
