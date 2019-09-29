using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Movement: MonoBehaviour {

    public List<GameObject> RunAwayPoints;

    public float timeToMove, wakeTimer;
    private float time, startX, startY = 2, startZ, curX, curZ, sleepTimer;
    private int selectedRunPoint;
    public float awarenessRadius = 10f;
    private Vector3 Target;

    public bool sleeping;

    Transform player;
    NavMeshAgent Agent;
    Renderer rend;
    public Material[] materials;

    // Use this for initialization
    void Start () {

        player = PlayerManager.instance.player.transform;

        rend = GetComponent<Renderer>();
        materials = rend.materials; // SKAL være her, må IKKE være i Feelings
                                    // OK... :'(

        Agent = gameObject.GetComponent<NavMeshAgent>();
        startX = gameObject.transform.position.x; startY = gameObject.transform.position.y; startZ = gameObject.transform.position.z;
        sleeping = false;
	}

    // Update is called once per frame
    void Update()
    {
        //distance to player 
        float distance = Vector3.Distance(player.position, transform.position);

        //selected runPoint
       

        
        if (!sleeping)
        {
            if (distance >= awarenessRadius)
            {
                time += Time.deltaTime;

                if (time >= timeToMove)
                {
                    GetRandomPosition();
                    time = 0;
                }

                Agent.SetDestination(Target); // Increase speed i Nav Mesh Agent Component
            }
            else {

                // løb langt væk fra player!!!!
                selectedRunPoint = Random.Range(0, RunAwayPoints.Count);
                Agent.SetDestination(RunAwayPoints [selectedRunPoint].transform.position);
                Debug.Log("runnig to point: " + selectedRunPoint);
            }
           
        }
        else
        {
           
            Agent.ResetPath();
            
            sleepTimer += Time.deltaTime;

            if (sleepTimer >= wakeTimer)
            {
                sleeping = false;
                sleepTimer = 0;

                //change materials Aka texture
                rend.material = materials[0];
            }
        }
    }

    Vector3 GetRandomPosition()
    {
        float x, z;

        x = Random.Range(-20, 20);
        z = Random.Range(-20, 20);

        Target = new Vector3(x, startY, z);
        return Target;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, awarenessRadius);
    }
}
