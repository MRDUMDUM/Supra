using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowME : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform target;
    public GameObject targetObject;
    public float fireAngle = 45f;
    public float gravity = 9.8f;

    public static Transform ball = null;
    private Transform StartPosition;

    public Transform point1, point2,resetPoint1, point3,resetPoint2;
    private int numPoints = 50;
    private Vector3[] positions = new Vector3[51];

    private Vector3 placeTargetPoint;

    public LayerMask layerMask;

    public static bool canThrow = false;
    public static bool inRange = false;

    public static int elementIndicator;

    [Header("All Element Prefabs")]
    public GameObject fire;
    public GameObject water;
    public GameObject ice;
    public GameObject electro;

    private void Awake()
    {
        // StartPosition = transform;
        lineRenderer.positionCount = numPoints;
    }

    private void Update()
    {
        if (canThrow)
        {

            DrawCurve();
            RaycastCurver();
            if (inRange)
            {
                
                this.gameObject.GetComponent<LineRenderer>().enabled = true;
                targetObject.SetActive(true);
            }
            else
            {
                this.gameObject.GetComponent<LineRenderer>().enabled = false;
                targetObject.SetActive(false);
            }
            
        }
        else
        {
            this.gameObject.GetComponent<LineRenderer>().enabled = false;
            targetObject.SetActive(false);
        }
        
    }

    void RaycastCurver()
    {
        RaycastHit hit1;
        RaycastHit hit2;

        if (Physics.Raycast(point1.position, point2.position-point1.position, out hit1, 40, layerMask))
        {
            placeTargetPoint = hit1.point;
            target.position = placeTargetPoint;
            point2.position = placeTargetPoint;
            point3.position = placeTargetPoint;
            target.rotation = Quaternion.FromToRotation(transform.up, hit1.normal) * transform.rotation;
            inRange = true;
        }
        else
        {
            point2.position = point1.position + (resetPoint1.position - point1.position);
            point3.position = point2.position + (resetPoint2.position - point2.position);

            if (Physics.Raycast(point2.position, point3.position-point2.position, out hit2, 150, layerMask))
            {
                placeTargetPoint = hit2.point;
                target.position = placeTargetPoint;
                point3.position = placeTargetPoint;
                target.rotation = Quaternion.FromToRotation(transform.up, hit2.normal) * transform.rotation;
                inRange = true;
            }
            else
            {
                inRange = false;
            }
            
        }


        Debug.DrawRay(point1.position, (point2.position-point1.position), Color.red);
        Debug.DrawRay(point2.position, (point3.position-point2.position), Color.red);
    }


    private void DrawCurve()
    {
        for(int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculatCurv(t, point1.position, point2.position, point3.position);
        }
        lineRenderer.SetPositions(positions);
    }

    private Vector3 CalculatCurv(float t, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        // (1-t)2 p1+2(1-t)tp2 + t2p3
        //   u          u
        // uu * p0 + 2 * u * t * p1 + tt * p2 
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p1;
        p += 2 * u * t * p2;
        p += tt * p3;
        return p;
        
    }

    public void ElementToThrow()
    {
        switch (elementIndicator)
        {
            case 0:
                GameObject fireElement = Instantiate(fire, point1.position, Quaternion.identity) as GameObject;
                ball = fireElement.transform;
                break;
            case 1:
                GameObject waterElement = Instantiate(water, point1.position, Quaternion.identity) as GameObject;
                ball = waterElement.transform;
                break;
            case 2:
                GameObject iceElement = Instantiate(ice, point1.position, Quaternion.identity) as GameObject;
                ball = iceElement.transform;
                break;
            case 3:
                GameObject electroElement = Instantiate(electro, point1.position, Quaternion.identity) as GameObject;
                ball = electroElement.transform;
                break;

        }
    }

    
    public IEnumerator SimulateBallCurv()
    {
        ElementToThrow();
        ball.position = point1.position;
        
        float targetDistance = Vector3.Distance(ball.position, target.position - new Vector3(0,3,0));

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float ballVelocity = targetDistance / (Mathf.Sin(2 * fireAngle * Mathf.Deg2Rad) / gravity);

        //X and y Component of the velocity
        float Vx = Mathf.Sqrt(ballVelocity) * Mathf.Cos(fireAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(ballVelocity) * Mathf.Sin(fireAngle * Mathf.Deg2Rad);

        //flight time
        float flightDuration = targetDistance / Vx;

        //Rotate towards the target
        ball.rotation = Quaternion.LookRotation(target.position - ball.position);

        float elapseTime = 0;

        while (elapseTime < flightDuration)
        {
            if(ball == null)
            {
                yield return null;
            }
            else
            {
                ball.Translate(0f, (Vy - (gravity * elapseTime))* Time.deltaTime, Vx * Time.deltaTime);

                elapseTime += Time.deltaTime;
                yield return null;
            }
 
        }
        
    }

}
