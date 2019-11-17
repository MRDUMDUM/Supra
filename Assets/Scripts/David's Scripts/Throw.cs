using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public Rigidbody Ball;
    public GameObject target;
    
    public float h = 25;
    public float gravity = -18f;

    public LineRenderer lineRenderer;
    public GameObject hittingPoint;
    
    
    public float firingAngle = 45.0f;

    
    

    private void Start()
    {
        Ball = this.GetComponent<Rigidbody>();
    }


    private void Update()
    {
        Ray ray = new Ray(target.transform.position, Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100f))
        {
            hittingPoint.SetActive(true);
            hittingPoint.transform.position = hit.point + Vector3.up * 0.1f;

            Vector3 Vo = CalculateVelocity(hit.point, this.transform.position, 1f);

            //transform.rotation = Quaternion.LookRotation(Vo);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Ball.velocity = Vo;
            }
        }
        else
        {
            hittingPoint.SetActive(false);
        }
    }

    void LaunchElement()
    {

    }

    Vector3 CalculateVelocity(Vector3 target, Vector3 start, float time)
    {
        //Define distance of y and xz
        Vector3 distance = target - start;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        // create a float thats represents pur distance
        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * gravity * time;

        Vector3 result = distanceXZ.normalized;

        result *= Vxz;
        result.y = Vy;

        return result;
    }

}
