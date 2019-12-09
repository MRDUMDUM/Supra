using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinHere : MonoBehaviour
{
    public ParticleSystem fireWork;
    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("Win",true);
            fireWork.Play();
        }
    }
}
