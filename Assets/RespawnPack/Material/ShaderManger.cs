using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderManger : MonoBehaviour
{
    public GameObject player;
    public Material respawn_Mat;
    public ParticleSystem respawn_par;

    [Header("Particle Values")]
    public Vector3 starPosParticle;
    public Vector3 endPosParticle;
    public float stopParticleOffset;


    [Header("Material Dissolve Values")]
    public float startDissolve = 1;
    public float endDissolve = 0;
    public float dissolveValue;
    public float respawnSpeed;
    [Space]
    public bool dead = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {

            //time += Time.deltaTime / respawnSpeed;
            //dissolveValue = Mathf.Lerp(startDissolve, endDissolve, time);
            //respawn_Mat.SetFloat("Vector1_A7F0C211", dissolveValue);
            StartCoroutine(respawn(respawnSpeed));
            respawn_par.Play();
            dead = false;
        }

        // stop respawn particle
        if(respawn_par.transform.localPosition.y >= endPosParticle.y - stopParticleOffset)
        {
            respawn_par.Stop();
        }
        
    }


    public IEnumerator respawn(float respawnSpeed)
    {
        float t = 0f;
        while (t < 1)
        {
            //timer
            t += Time.deltaTime / respawnSpeed;
            //dissolve Shader Part
            dissolveValue = Mathf.Lerp(startDissolve, endDissolve, t);
            respawn_Mat.SetFloat("Vector1_A7F0C211", dissolveValue);

            //moving particle part
            respawn_par.transform.position = Vector3.Lerp((player.transform.position + starPosParticle), (player.transform.position + endPosParticle), t);

            yield return null;
        }
    }
}
