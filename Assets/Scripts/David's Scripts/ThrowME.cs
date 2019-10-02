using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowME : MonoBehaviour
{
    public Rigidbody body;
    public Transform target;

    public float h;
    float gravity; // force of gravity on the y direction


    public float velocity;
    public float angle;
    public int resolution;
    
    float radianAngle;
    LineRenderer throwLine;
    // Start is called before the first frame update
    void Start()
    {
        throwLine = this.GetComponent<LineRenderer>();
        gravity = Mathf.Abs(Physics.gravity.y);
        
    }

    // Update is called once per frame
    void Update()
    {
        RenderArc();
    }

    void RenderArc()
    {
        throwLine.SetVertexCount(resolution + 1);
        
        throwLine.SetPositions(CalculateLaunchVelocity());
    }

    Vector3[] CalculateLaunchVelocity()
    {
        float displacementY = target.position.y - body.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - body.position.x, 0, target.position.z - body.position.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);



        //Gamel code skal slettes!
        Vector3[] arcArray = new Vector3[resolution + 1];
        
        return arcArray;
    }


    // skal ogsp slettets!
    Vector3 CalculatePoint(float t, float maxDistance)
    {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) * ((gravity * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return new Vector3(x, y);
    }
}
