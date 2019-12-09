using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCheck : MonoBehaviour
{
    private int counter = 0;
    public GameObject point1, point2;
    public bool Powered;
    public GameObject powerSource;
    private bool checker;
    public float speedMod = 5.0f;

    public bool moveOnce = false;
    private bool endReached = false;

    // Start is called before the first frame update
    void Start()
    {
        Powered = false;
        checker = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (powerSource.GetComponent<Conductive>().isPowered == true)
        {
            if (!moveOnce)
            {
                if (checker == true)
                {
                    gameObject.transform.position = Vector3.MoveTowards(transform.position, point2.transform.position, speedMod*Time.deltaTime);
                    if (gameObject.transform.position == point2.transform.position)
                    {
                        checker = false;
                    }
                }
                else {
                    gameObject.transform.position = Vector3.MoveTowards(transform.position, point1.transform.position, speedMod * Time.deltaTime);
                    if(gameObject.transform.position == point1.transform.position)
                    {
                        checker = true;
                    }
                }
            }
            else
            {

                if (!endReached)
                {
                    gameObject.transform.position = Vector3.MoveTowards(transform.position, point2.transform.position, speedMod * Time.deltaTime);
                    if (gameObject.transform.position == point2.transform.position)
                    {
                        endReached = true;
                    }
                }
                else
                {
                    gameObject.transform.position = Vector3.MoveTowards(transform.position, point1.transform.position, speedMod * Time.deltaTime);
                    if (gameObject.transform.position == point1.transform.position)
                    {
                        endReached = false;
                        powerSource.GetComponent<Conductive>().isPowered = false;
                    }
                }


            }

        }
    }
}
