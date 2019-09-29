using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachine : MonoBehaviour
{

    public List<GameObject> RunAwayPoints;

    public float timeToMove, wakeTimer, sprintTimer, runAwayTime,normalSpeed,runSpeed;
    //public int minTime, maxTime;
    private float time, runTime, startX, startY = 2, startZ, curX, curZ, sleepTimer;
    private int selectedRunPoint;
    //public float awarenessRadius = 10f;
    

    //TEST AREA

    private Vector3 Target;
    public TargetScanner playerScanner;
    public float fleeingDistance = 3.0f;

    [Header("Audio")]
    public RandomAudioPlayer spottedAudio;



    public NpcController controller { get { return m_Controller; } }
    public PlayerController target { get { return m_Target; } }

    protected PlayerController m_Target = null;
    protected NpcController m_Controller;
    protected bool Fleeing = false;

    //TEST AREA

    public bool sleeping;
    //private bool run;


    //Transform player;
    //NavMeshAgent Agent;
    Renderer rend;
    public Material[] materials;

    protected void OnEnable()
    {
        m_Controller = GetComponent<NpcController>();
        rend = GetComponent<Renderer>();
        //materials = rend.materials; // SKAL være her, må IKKE være i Feelings
                                    // OK... :'(
        startX = gameObject.transform.position.x; startY = gameObject.transform.position.y; startZ = gameObject.transform.position.z;
        sleeping = false;
    }

    public void Spotted()
    {
        if (spottedAudio != null)
          spottedAudio.PlayRandomClip();
    }

    // Use this for initialization
    void Start()
    {
        //finds player position
        //player = PlayerManager.instance.player.transform;

        //sets texture after hit and time
       // rend = GetComponent<Renderer>();
        //materials = rend.materials; // SKAL være her, må IKKE være i Feelings
                                    // OK... :'(
        //sætter det første runPoint
	    //selectedRunPoint = Random.Range(0,RunAwayPoints.Count);


        //Agent = gameObject.GetComponent<NavMeshAgent>();
        //startX = gameObject.transform.position.x; startY = gameObject.transform.position.y; startZ = gameObject.transform.position.z;
        //sleeping = false;
        //run = false;
    }

    public void CheckNeedFleeing()
    {
        if(m_Target == null)
        {
            Fleeing = false;
            return;
            
        }

        Vector3 FromTarget = transform.position - m_Target.transform.position;

        if(Fleeing || FromTarget.sqrMagnitude <= fleeingDistance * fleeingDistance)
        {
            //picks new direction if player is to close (like a conrer)
            Vector3 fleePoint = transform.position + FromTarget * 2 * fleeingDistance;

            Debug.DrawLine(fleePoint, fleePoint + Vector3.up * 10.0f);

            if (!Fleeing)
            {
                //if we're not already fleeing, we may be in the cooldown, so the navmesh agent is disabled, enable it
                controller.SetFollowNavmeshAgent(true);
            }

            controller.SetTarget(fleePoint);

            Fleeing = true;
        }

        if (Fleeing && FromTarget.sqrMagnitude > fleeingDistance * fleeingDistance * 4)
        {
            //we're twice the fleeing distance from the player and fleeing, we can stop now
            Fleeing = false;
            //controller.animator.SetBool(hashFleeing, m_Fleeing);
        }

        if (!Fleeing)
        {
            time += Time.deltaTime;

            if (time >= timeToMove)
            {
                controller.SetTarget(Target);
            }
        }
    }

    private void Update()
    {
        print("target" + m_Target);
        print("vector" + Target);
    }

    // Update is called once per frame
    //   void Update ()
    //{




    //       if (!sleeping)
    //       {

    //           //Agent.enabled = true;
    //           //if (!run) //distance >= awarenessRadius &&
    //           //{
    //               time += Time.deltaTime;

    //               if (time >= timeToMove)
    //               {
    //                   //GetRandomPosition();
    //                   time = 0;
    //               }

    //              // Agent.SetDestination(Target);
    //               Debug.Log("walking");
    //          // }
    //            else if (distance <= awarenessRadius)
    //            {
    //                run = true;
    //              //  runTime += Time.deltaTime;
    ////                if (runTime >= runAwayTime)
    ////                {
    //                   // selectedRunPoint = Random.Range(0, RunAwayPoints.Count);
    ////                    run = false;
    ////                    runTime = 0;
    ////                }
    //		        StartCoroutine(runaway());
    //               // Agent.SetDestination(RunAwayPoints[selectedRunPoint].transform.position);
    //                Debug.Log("runnig to point: " + selectedRunPoint);
    //                Debug.Log("RUN!!");
    //            }
    //}
    //else {

    //    //Agent.enabled = false;
    //    sleepTimer += Time.deltaTime;
    //    Debug.Log("AV...");
    //    if (sleepTimer >= wakeTimer)
    //    {
    //        sleeping = false;
    //        sleepTimer = 0;

    //        //change materials Aka texture
    //        rend.material = materials[0];
    //    }

    //}

    /*
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
        else
        {

            // løb langt væk fra player!!!!
            selectedRunPoint = Random.Range(0, RunAwayPoints.Count);
            Agent.SetDestination(RunAwayPoints[selectedRunPoint].transform.position);
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
    }*/
    //}

    //   public IEnumerator runaway()
    //   {
    //  // int index = Random.Range(minTime, maxTime);
    //Agent.SetDestination(RunAwayPoints[selectedRunPoint].transform.position);
    //Agent.speed = runSpeed;
    //   yield return new WaitForSeconds(sprintTimer);
    //   Agent.speed = normalSpeed;
    //   yield return new WaitForSeconds(runAwayTime);
    //   run = false;
    //   }

    Vector3 GetRandomPosition()
    {

        
            float x, z;

        x = Random.Range(-20, 20);
        z = Random.Range(-20, 20);

        Target = new Vector3(x, startY, z);
        return Target;
       
    }

    public void FindTarget()
    {
        //we ignore height difference if the target was already seen
        m_Target = playerScanner.Detect(transform, m_Target == null);
        //m_Controller.animator.SetBool(hashHaveEnemy, m_Target != null);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        playerScanner.EditorGizmo(transform);
    }
    #endif
}