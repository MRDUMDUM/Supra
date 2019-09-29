using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//make it run before any behavior
[DefaultExecutionOrder(-1)]
[RequireComponent(typeof(NavMeshAgent))]
public class NpcController : MonoBehaviour {

    public bool interpolateTurning = false;
    public bool applyAnimationRotation = false;

    protected NavMeshAgent m_NavMeshAgent;
    protected Rigidbody m_Rigidbody;
    protected bool m_FollowNavmeshAgent;
    protected bool m_Grounded;
    protected bool m_ExternalForceAddGravity = true;
    protected bool m_UnderExternalForce;
    protected Vector3 m_ExternalForce;

    const float k_GroundedRayDistance = .8f;

    private void OnEnable()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();

        m_NavMeshAgent.updatePosition = false;

        m_Rigidbody = GetComponentInChildren<Rigidbody>();
        if (m_Rigidbody == null)
            m_Rigidbody = gameObject.AddComponent<Rigidbody>();

        m_Rigidbody.isKinematic = true;
        m_Rigidbody.useGravity = false;
        m_Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        m_Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        m_FollowNavmeshAgent = true;

    }

    private void FixedUpdate()
    {
        CheckGrounded();

        if (m_UnderExternalForce)
            ForceMovement();
    }


    void CheckGrounded()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * k_GroundedRayDistance * 0.5f, -Vector3.up);
        m_Grounded = Physics.Raycast(ray, out hit, k_GroundedRayDistance, Physics.AllLayers,
            QueryTriggerInteraction.Ignore);
    }

    void ForceMovement()
    {
         if (m_ExternalForceAddGravity)
            m_ExternalForce += Physics.gravity * Time.deltaTime;

        RaycastHit hit;
        Vector3 movement = m_ExternalForce * Time.deltaTime;
         if (!m_Rigidbody.SweepTest(movement.normalized, out hit, movement.sqrMagnitude))
         {
           m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
         }

     m_NavMeshAgent.Warp(m_Rigidbody.position);
    }

    public void SetFollowNavmeshAgent(bool follow)
    {
        if (!follow && m_NavMeshAgent.enabled)
        {
            m_NavMeshAgent.ResetPath();
        }
        else if (follow && !m_NavMeshAgent.enabled)
        {
            m_NavMeshAgent.Warp(transform.position);
        }

        m_FollowNavmeshAgent = follow;
        m_NavMeshAgent.enabled = follow;
    }

    public void AddForce(Vector3 force, bool useGravity = true)
    {
        if (m_NavMeshAgent.enabled)
            m_NavMeshAgent.ResetPath();

        m_ExternalForce = force;
        m_NavMeshAgent.enabled = false;
        m_UnderExternalForce = true;
        m_ExternalForceAddGravity = useGravity;
    }

    public void ClearForce()
    {
        m_UnderExternalForce = false;
        m_NavMeshAgent.enabled = true;
    }

    public void SetForward(Vector3 forward)
    {
        Quaternion targetRotation = Quaternion.LookRotation(forward);

        if (interpolateTurning)
        {
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                m_NavMeshAgent.angularSpeed * Time.deltaTime);
        }

        transform.rotation = targetRotation;
    }

    public void SetTarget(Vector3 position)
    {
        m_NavMeshAgent.destination = position;
    }
}
