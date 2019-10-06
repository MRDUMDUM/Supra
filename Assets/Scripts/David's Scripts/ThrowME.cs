using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowME : MonoBehaviour
{
    public Rigidbody body;
    public Transform target;
    public float velocity;
    public float h;
    public float gravity; // force of gravity on the y direction

    public bool debugPath;
    
    //public float angle;
   // public int resolution;
    
    //float radianAngle;
    LineRenderer throwLine;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        throwLine = this.GetComponent<LineRenderer>();
        //gravity = Mathf.Abs(Physics.gravity.y)*(-1);
        
        body.useGravity = false;
        Debug.Log("Gravity" + gravity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }

        if (debugPath)
        {
            DrawPath();
        }
    }

    void Launch()
    {
        body.isKinematic = false;
        body.useGravity = true;
        body.velocity = CalculateLaunchData().initialVelocity;
        
    }
    
    LaunchData CalculateLaunchData()
    {
        float displacementY = target.position.y - body.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - body.position.x, 0, target.position.z - body.position.z);
        float time = Mathf.Sqrt(-2 * gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ * velocity / time;

        return new LaunchData (velocityXZ + velocityY,time);
    }

    void DrawPath()
    {
        LaunchData launchData = CalculateLaunchData();
        Vector3 previousDrawPoint = body.position;

        int resolution = 30;

        for(int i = 1; i<=resolution;i++)
        {
            float simulationTime = i / (float)resolution * launchData.timeToTarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = body.position + displacement;

            //throwLine.SetPosition(i, )
            Debug.DrawLine(previousDrawPoint, drawPoint, Color.cyan);
            previousDrawPoint = drawPoint;
        }
    }

    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }

    // skal ogsp slettets!
    //Vector3 CalculatePoint(float t, float maxDistance)
    //{
    //    float x = t * maxDistance;
    //    float y = x * Mathf.Tan(radianAngle) * ((gravity * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
    //    return new Vector3(x, y);
    //}
}
