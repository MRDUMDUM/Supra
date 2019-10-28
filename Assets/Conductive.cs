using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductive : MonoBehaviour
{

    public bool isPowered;
    public bool conductive;
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
        if (collision.gameObject.CompareTag("Electric")&&conductive==true)
        {
            isPowered = true;
        }
    }
}
