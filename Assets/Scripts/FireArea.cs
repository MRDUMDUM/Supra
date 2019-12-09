using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArea : MonoBehaviour
{
    public ParticleSystem damp;

    private void OnTriggerEnter(Collider col)
    {
        switch (col.transform.tag)
        {
            case "Water":
                Destroy(col.gameObject);
                Instantiate(damp, this.transform.position,Quaternion.identity);

                Destroy(this.gameObject);
                break;
        }
    }
}
