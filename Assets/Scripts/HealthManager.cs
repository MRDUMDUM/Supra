using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public GameObject respawnPoint;
    public int maxHealth;
    public int currentHealth;

    public PlayerController playerControl;

    public float invincibilityLength;
    private float invincibilityCounter;

    public Renderer playerRenderer;
    private float flashCounter;
    public float flashLength = 0.1f;

    private bool isRespawning;
    private Vector3 respawnPoint;
    public float respawnlegth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        playerControl = FindObjectOfType<PlayerController>();

        respawnPoint = playerControl.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;

            if (flashCounter <=0)
            {
                playerRenderer.enabled = !playerRenderer.enabled;
                flashCounter = flashLength;
            }

            if(invincibilityCounter <= 0)
            {
                respawn();
                playerRenderer.enabled = true;
            }
        }
    }

    public void HurtPlayer(int damage, Vector3 direction)
    {
        if(invincibilityCounter <= 0)
        {

            currentHealth -= damage;

            if(currentHealth <= 0)
            {
                Respawn();
            }
            else
            {
                playerControl.KnockBack(direction);

                invincibilityCounter = invincibilityLength;

                playerRenderer.enabled = false;

                flashCounter = flashLength;
            }

            
        }

    }



    //when the player hits lava
    public void HurtPlayerLava()
    {

    }


    public void Respawn()
    {
        //playerControl.transform.position = respawnPoint;
        //currentHealth = maxHealth;
        if (!isRespawning)
        {
            StartCoroutine(RespawnCorutine());
        }
        
    }

    public void HealPlayer(int heal)
    {
        currentHealth += heal;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }


    public IEnumerator RespawnCorutine()
    {
        isRespawning = true;
        yield return new WaitForSeconds(respawnlegth);
        isRespawning = false;
    }
}
