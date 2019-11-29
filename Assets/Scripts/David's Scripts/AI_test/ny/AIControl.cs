using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour {

   // GameObject[] goalLocations;
    GameObject player;
    NavMeshAgent agent;
    //Animator anim;
    float speedMul;
    float detectionRadius = 20;
    public float fleeRadius = 4.0f;

    public float wanderRadius;
    public float wanderTimer;
    public float speed;
    private float timer;


    void ResetAgent()
    {
        speedMul = Random.Range(3f, 4f);
        agent.speed = 2 * speedMul;
        agent.angularSpeed = 120;
        agent.acceleration = 10;
        //anim.SetFloat("speedMult", speedMult);
       // anim.SetTrigger("isWalking");
        agent.ResetPath();
    }

    // Use this for initialization
    void Start () {

        timer = wanderTimer;

        player = GameObject.FindGameObjectWithTag("Player");

        //goalLocations = GameObject.FindGameObjectsWithTag("Goal");
        agent = this.GetComponent<NavMeshAgent>();
        
        //agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        
        //anim = this.GetComponent<Animator>();
        //anim.SetFloat("wOffset", Random.Range(0, 1));
        //ResetAgent();
    }
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < detectionRadius)
        {
            Vector3 fleeDirection = (transform.position - player.transform.position).normalized;
            Vector3 newgoal = transform.position + fleeDirection * fleeRadius;

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(newgoal, path);

            if (path.status != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(path.corners[path.corners.Length - 1]);
                //anim.SetTrigger("isRunning");
                agent.speed = speed;
                agent.angularSpeed = 50;
                agent.acceleration = 60;
            }

        }
        else
        {
            agent.speed = 5f;
            agent.angularSpeed = 120;
            agent.acceleration = 10;

            if (timer >= Random.Range(3f, 6f))
            {
                Vector3 newPos = RandomNavSphere(transform.position, Random.Range(15f, 30f), -1);
                agent.SetDestination(newPos);
                timer = 0;
            }
            //if (agent.remainingDistance < 1)
            //{
            //    ResetAgent();
            //    //agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
            //}
        }

    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
