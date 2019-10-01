using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

public class FireNPC : MonoBehaviour
{
    NavMeshAgent agent;
    Rigidbody rigidbody;
    Animator anim;
    public GameObject player;
    public Collider ragdollCol;
    public Collider hitCol;

    //public Vector3 destination;
    //public Vector3 target;
    public int stamina = 1;
    public bool canSprint;
    public bool hit = false;
    public bool isSleeping = false;
    private float timer = 0.0f;


    // Use this for initialization
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        rigidbody = this.GetComponent<Rigidbody>();
        anim = this.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {


        if (isSleeping == true)
        {

            //hitCol.enabled = false;
            ragdollCol.enabled = true;
        }
        else
        {
            ragdollCol.enabled = false;
            hitCol.enabled = true;
        }

        if (stamina < 1)
        {
            timer += Time.deltaTime;
            if (timer > 15f)
            {
                stamina = +1;
                timer = 0f;
            }
        }
        Debug.Log("stamina: " + stamina);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            hit = true;
            Debug.Log("Hit!");
        }
        if(other.gameObject.tag=="Water"&&isSleeping==true)
        {
            
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Debug.Log("vaporized!");
        }
    }

    [Task]
    public void ResetAgent()
    {
        agent.ResetPath();
        Task.current.Succeed();
    }

    [Task]
    public void PickRandomDestination()
    {
        Vector3 newPos = RandomNavSphere(transform.position, Random.Range(10f, 100f), -1);
        agent.speed = 10;
        agent.acceleration = 8;
        agent.SetDestination(newPos);

        Task.current.Succeed();
    }

    [Task]
    public void MoveToDestination()
    {

        if (Task.isInspected)
            Task.current.debugInfo = string.Format("t={0:0.00}", Time.time);

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    public bool PlayerClose(float minDest)
    {
        Vector3 distance = player.transform.position - this.transform.position;
        return (distance.magnitude < minDest);
    }

    [Task]
    public void RunAway()
    {
        Vector3 runDirection = this.transform.position - player.transform.position;
        Vector3 newDest = this.transform.position + runDirection * 2;

        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(newDest, path);

        if (path.status != NavMeshPathStatus.PathInvalid)
        {

            agent.SetDestination(path.corners[path.corners.Length - 1]);

            agent.speed = 25;
            agent.angularSpeed = 100;
            agent.acceleration = 60;

        }

        agent.speed = 18;
        agent.acceleration = 30;
        agent.SetDestination(newDest);
        Task.current.Succeed();
    }

    [Task]
    public void Sprint()
    {
        Vector3 runDirection = this.transform.position - player.transform.position;
        Vector3 newdest = this.transform.position + runDirection * 3;
        agent.speed = 40;
        agent.acceleration = 60;
        agent.SetDestination(newdest);
        Task.current.Succeed();
    }

    [Task]
    public void UseStamina()
    {
        stamina--;
        Task.current.Succeed();
    }

    [Task]
    public void Sleeping()
    {
        isSleeping = true;
        agent.height = 3.9f;
        agent.baseOffset = 0f;
        agent.enabled = false;
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        anim.SetBool("isSleeping", true);
        Task.current.Succeed();
    }

    [Task]
    public void WakeUp()
    {
        isSleeping = false;
        hit = false;
        agent.enabled = true;
        agent.height = 6.34f;
        agent.baseOffset = 0f;
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        anim.SetBool("isSleeping", false);
        agent.ResetPath();
        Task.current.Succeed();
    }

    [Task]
    public bool Dead()
    {
        Destroy(this.gameObject);
        return true;
    }

    [Task]
    public bool SleepCheck()
    {
        if (hit == true)
        {
            return (true);
        }
        else
        {
            return (false);
        }

    }

    [Task]
    public bool SprintCheck(int stamina)
    {
        return this.stamina >= stamina;
    }


    //Used for Pick Random Destination
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
