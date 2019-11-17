using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

    public float rotationSpeed = 5;
    public bool RotateX = false;
    public bool RotateY = false;
    public bool RotateZ = false;

    [Header("Rotate object on local axis")]
    public bool localRotateX = false;
    public bool localRotateY = false;
    public bool localRotateZ = false;

    [Header("Rotate object in an arch")]
    public bool arcRotate = false;
    public bool arcX = false;
    public bool arcY = false;
    public bool arcZ = false;
    public float arcdegress = 90;

    [Header("Rotation Direction")]
    [Range(-1,1)]
    public int Direction = 1;

    private float StartPositionX;
    private float StartPositionY;
    private float StartPositionZ;

    private float arcEndPositionX;
    private float arcEndPositionY;
    private float arcEndPositionZ;

    private float arcStartPositionX;
    private float arcStartPositionY;
    private float arcStartPositionZ;

    public GameObject RotatingObject;

    // Use this for initialization
    void Start () {
        StartPositionX = transform.eulerAngles.x;
        StartPositionY = transform.eulerAngles.y;
        StartPositionZ = transform.eulerAngles.z;

        arcStartPositionX = transform.eulerAngles.z + arcdegress;
        arcStartPositionY = transform.eulerAngles.z + arcdegress;
        arcStartPositionZ = transform.eulerAngles.z + arcdegress;

        arcEndPositionX = transform.eulerAngles.x - arcdegress;
        arcEndPositionY = transform.eulerAngles.y - arcdegress;
        arcEndPositionZ = transform.eulerAngles.z - arcdegress;

    }
	
	// Update is called once per frame
	void Update () {

        if (RotateX == true)
        {
            transform.RotateAround(this.transform.position, Vector3.right, (rotationSpeed * Time.deltaTime)*Direction);
        }

        if (RotateY == true)
        {
            transform.RotateAround(this.transform.position, Vector3.up, (rotationSpeed * Time.deltaTime)*Direction);
        }

        if (RotateZ == true)
        {
            transform.RotateAround(this.transform.position, Vector3.forward, (rotationSpeed * Time.deltaTime)*Direction);
        }

        if(arcRotate == true)
        {
            RotatingArch();
        }

        LocalRotation();

        //if (RotateX == true)
        //{
        //    transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        //}

        //if (RotateY == true)
        //{
        //    transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        //}

        //if (RotateZ == true)
        //{
        //    transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        //}
    }

    public void RotatingArch()
    {
       

        if(arcX == true)
        {
            this.transform.rotation = Quaternion.Euler(StartPositionX + 90 * Mathf.Sin(Time.time * rotationSpeed)*Direction, 0f, 0f);

            if (this.transform.rotation.x == arcStartPositionX)
            {
                RotatingObject.GetComponent<Rotation>().rotationSpeed = -200;

            }else if(this.transform.rotation.x == arcEndPositionX)
            {
                RotatingObject.GetComponent<Rotation>().rotationSpeed = 200;
            }
        }
        if (arcY == true)
        {
            this.transform.rotation = Quaternion.Euler(0f, (StartPositionY + 90 * Mathf.Sin(Time.deltaTime * rotationSpeed))*Direction, 0f);

        }
        if (arcZ == true)
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, (StartPositionZ + 90 * Mathf.Sin(Time.deltaTime * rotationSpeed))*Direction);

        }

    }

    void LocalRotation()
    {
        if (localRotateX == true)
        {
            transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime, Space.Self);
        }

        if (localRotateY == true)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
        }

        if (localRotateZ == true)
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime,Space.Self);
        }
    }
}
