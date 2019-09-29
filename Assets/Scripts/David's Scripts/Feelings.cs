using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Feelings : MonoBehaviour {

    StateMachine Hit;
    NPC_Movement movement;
    public Material[] materials;
    private int currentMaterial;
    public float hitForce;
    private Vector3 Backwards;
    Renderer rend;
    Feelings state;

	// Use this for initialization
	void Start () {
        movement = GetComponent<NPC_Movement>();
        rend = GetComponent<Renderer>();
        Hit = GetComponent<StateMachine>();
        // materials = rend.materials; <--- Hvis man tilføjer det her sletter den det ene Element når man kører det. 
        // ^ Til gengæld SKAL det være der før det virker i NPC_Movement. Det var fedt at finde ud af ^

        // rend.sharedMaterial = material[0];
        // currentMaterial = 0;

        rend.material = materials[0];
        
    }

    private void OnCollisionEnter(Collision Feel)
    {
        if (Feel.gameObject.tag == "Weapon")
        {
            try
            {
                //skift mellem movement og Hit
                if (Hit.sleeping == false)
                {

                    //Find impact vector point and adds force to the opposite vector point
                    ContactPoint impact = Feel.contacts[0];
                    Debug.Log(Feel.contacts);
                    Backwards = transform.InverseTransformDirection(impact.point);
                    GetComponent<Rigidbody>().AddForce(Backwards * hitForce, ForceMode.Impulse);

                    //change materials Aka texture
                    //currentMaterial++;
                    //currentMaterial %= material.Length;
                    //rend.sharedMaterial = material[currentMaterial];

                    rend.material = materials[1];
                    // currentMaterial = 1;

                    //movement.sleeping = true;
                    Hit.sleeping = true;

                    transform.position = transform.position;
                }
                else
                {
                    // Ikke gør noget hvis man tæver den mens den sover
                }
            }
            catch
            {
                Debug.Log("Not an NPC or something went wrong!");
            }
        }
     }

    // Update is called once per frame
    void Update () {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            currentMaterial++;
            currentMaterial %= material.Length;
            rend.sharedMaterial = material[currentMaterial];
        }*/
		
	}
}
