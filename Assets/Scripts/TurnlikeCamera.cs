using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

public class TurnlikeCamera : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

        this.gameObject.transform.rotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
    }
}
