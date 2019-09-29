using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform pivot;
    public Transform target;
    public Vector3 offset;
    public bool useOffsetValues;
    public bool invertY;
    public float rotateSpeed;
    public float maxViewAngle;
    public float minViewAngle;

    // Use this for initialization
    void Start () {
        if (!useOffsetValues)
        {
            offset = target.position - transform.position;
        }

        pivot.transform.position = target.transform.position;
        // pivot.transform.parent = target.transform;
        pivot.transform.parent = null;

        //removes mouse Cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate () {

        pivot.transform.position = target.transform.position;
        
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        pivot.Rotate(0f, horizontal, 0f);

        //get the y position of the mouse & rotate the pivot
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;

        if (invertY)
        {
            pivot.Rotate(vertical, 0f, 0f);
        }
        else
        {
            pivot.Rotate(-vertical, 0f, 0f);
        }
        //limit up/down camera position
        if (pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180f)
        {
            pivot.rotation = Quaternion.Euler(maxViewAngle, 0, 0);
        }
        if (pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < 360f + minViewAngle)
        {
            pivot.rotation = Quaternion.Euler(360f + minViewAngle, 0, 0);
        }

        //moves the target in the x position of the mouse
        float desiredYAngle = pivot.eulerAngles.y;
        float desiredXAngle = pivot.eulerAngles.x;

        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0f);
        transform.position = target.position - (rotation * offset);

        if(transform.position.y < target.position.y)
        {
            transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z);
        }

        transform.LookAt(target); 
	}
}
