using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    ParticleSystem thisSystem;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        thisSystem = GetComponent<ParticleSystem>();
        timer = thisSystem.main.duration;
    }

    // Update is called once per frame
    void Update()
    {

        Destroy(this.gameObject, timer);
    }
}
