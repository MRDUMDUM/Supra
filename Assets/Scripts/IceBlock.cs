using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour
{
    public ParticleSystem damp;

    float meltSpeed = 0.5f;
    float meltAmount = 5f;

    bool melting = false;

    Vector3 downPos;

    private void Start()
    {

        downPos = this.transform.position - new Vector3(0, meltAmount, 0);
    }

    private void Update()
    {

        if (melting)
        {
            float distance = Vector3.Distance(transform.position, downPos);

            if (distance > 0.01f)
            {
               transform.position = Vector3.Lerp(transform.position, downPos, Time.deltaTime * meltSpeed);
            }
            else
            {
                downPos = this.transform.position - new Vector3(0, meltAmount, 0);
                melting = false;
            }
            
        }
    }


    void Melt()
    {
        
        //this.transform.Translate(Vector3.down*meltSpeed);
    }

    private void OnCollisionEnter(Collision col)
    {
        switch (col.transform.tag)
        {
            case "Fire":
                Destroy(col.gameObject);
                var newDamp = Instantiate(damp, col.transform.position, Quaternion.identity);
                newDamp.transform.parent = gameObject.transform;
                melting = true;
                
                break;
        }
    }
    
}
