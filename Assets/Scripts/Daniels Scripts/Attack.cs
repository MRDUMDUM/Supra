using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public string NameOfAnimation;
    Animator anim;
    public GameObject Weapon;

    // Use this for initialization
    void Start()
    {

        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (Input.GetKeyDown("x"))
        {
            PerformAttack();
        }
       

    }
    public void PerformAttack()
    {
        Debug.Log("Attack!!");
        anim.SetTrigger("Base_Attack");
    }

}
