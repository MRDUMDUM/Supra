using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

namespace RootMotion.FinalIK
{
    public class BlobAttack : MonoBehaviour
    {
        
        public GameObject goal;//player
        public GameObject arm_armature;
        public GameObject bender;
        public GameObject attackPoint;
        public FABRIK arm;
        public FABRIKRoot arm_root;
        public LimbIK bend;
        public float leanBackSpeed = 0.9f;
        public float leanBackLerp = 0f;
        public float attackSpeed = 2f;
        public float atkLerp = 0f;
        

        public float guardDistance ;
        public float attackDistance;

        public Vector3 maxScale;
        public Vector3 minScale;
        public float scaleSpeed;

        //Vector3 gaurdBendPoint;

        //fabrik_root Weights on blobArmmid
        float root_Wight_GuardMin = 0.035f;
        float root_Wight_GuardMax = 0.103f;
        float guardT = 0.0f;


        float Limb_IK_Pos_WeightMin = 0.09f;
        float Limb_IK_Pos_WeightMax = 0.150f;
        //float limbPos = 0.0f;

        float Limb_IK_Rot_WeightMin = 0.0f;
        float Limb_IK_Rot_WeightMax = 0.166f;
        //float limbrot = 0.0f;

        // Vector3 pointA = new Vector3 (1, 0, 0);

        public bool lean = true;
        public bool attack = false;
        bool guardMode = false;

        //for detection of last posiiton in to new smoothstep
        int leanLastPosValue = 0;
        int restoreGuardLastPosValue = 0;
        float lastRoot = 0;
        float lastLimbPos = 0;
        float lastLimbRot = 0;
        float leanT = 0.0f;
        float leanBendT = 0.0f;
        float attackT = 0.0f;
        float attackBendT = 0.0f;
        float restoreT = 0.0f;

        [Task]
        public bool placedOnce = false;

        // Use this for initialization
        void Start()
        {
          //  gaurdBendPoint = bender.transform.localPosition + pointA;
        }

        // Update is called once per frame
        void Update()
        {

            Debug.Log("root: " + lastRoot);
            Debug.Log("POs: " + lastLimbPos);
            Debug.Log("rot: " + lastLimbRot);

            Vector3 attackDistance = goal.transform.position - this.transform.position;
            if(attackDistance.magnitude < 40f)
            {
                attack = true;
            }
            //  gaurdBendPoint = bender.transform.localPosition;
            // Debug.Log("vector: " + gaurdBendPoint);
            //Vector3 pointA = new Vector3(bender.transform.position.x + 1f, bender.transform.position.y, bender.transform.position.z);
            //Vector3 pointB = new Vector3(bender.transform.localPosition.x - 1f, bender.transform.localPosition.y, bender.transform.localPosition.z);
            // Attack();
            //Vector3 move = new Vector3(bender.transform.localPosition.y*Mathf.Sin(2 * Time.deltaTime) * 1f, bender.transform.localPosition.y, bender.transform.localPosition.z);
            // bender.transform.localPosition = move;
            //bender.transform.localPosition += Vector3(bender.transform.position.x * Mathf.Sin(Time.deltaTime * 5f) * 0.5f, bender.transform.position.y, bender.transform.position.z);
        }

        [Task]
        public bool PlayerClose(float minDest)
        {
            Vector3 distance = goal.transform.position - this.transform.position;
            return (distance.magnitude < minDest);
        }

        //[Task]
        //public bool AttackPlayer(float minDest)
        //{
        //    Vector3 distance = goal.transform.position - this.transform.position;
        //    return (distance.magnitude < minDest);
        //}

        [Task]
        public void Grow()
        {

            if (arm_armature.transform.localScale.x < 1f)
            {
                arm_armature.transform.localScale = Vector3.Lerp(arm_armature.transform.localScale, maxScale, scaleSpeed * Time.deltaTime);
            }
        }

        [Task]
        public void Withdraw()
        {
            guardMode = false;
            if (arm_armature.transform.localScale.x > 0.001f)
            {
                arm_armature.transform.localScale = Vector3.Lerp(arm_armature.transform.localScale, minScale, scaleSpeed * Time.deltaTime);
            }
           
        }

        [Task]
        public void Guard()
        {
            guardMode = true;
            arm_root.solver.IKPositionWeight = Mathf.SmoothStep(root_Wight_GuardMin, root_Wight_GuardMax, guardT);
            bend.solver.IKPositionWeight = Mathf.SmoothStep(Limb_IK_Pos_WeightMax, Limb_IK_Pos_WeightMin, guardT);
            bend.solver.IKRotationWeight = Mathf.SmoothStep(Limb_IK_Rot_WeightMin, Limb_IK_Rot_WeightMax, guardT);

            guardT += 0.5f * Time.deltaTime;

            if (guardT > 1.0f)
            {
                float temp1 = root_Wight_GuardMax;
                root_Wight_GuardMax = root_Wight_GuardMin;
                root_Wight_GuardMin = temp1;

                float temp2 = Limb_IK_Pos_WeightMax;
                Limb_IK_Pos_WeightMax = Limb_IK_Pos_WeightMin;
                Limb_IK_Pos_WeightMin = temp2;

                float temp3 = Limb_IK_Rot_WeightMax;
                Limb_IK_Rot_WeightMax = Limb_IK_Rot_WeightMin;
                Limb_IK_Rot_WeightMin = temp3;

               

                guardT = 0.0f;
            }

            leanLastPosValue = 0;
            //bender.transform.localPosition = Vector3.Lerp(bender.transform.localPosition, gaurdBendPoint, Mathf.PingPong(Time.time,2));
            // bender.transform.localPosition = new Vector3(bender.transform.localPosition.x*Mathf.Sin(Time.deltaTime*1f)*5f, bender.transform.localPosition.y, bender.transform.localPosition.z);
        }

        [Task]
        public bool RestoreGuardMode()
        {
            if (guardMode == true)
            {
                return (true);
            }
            else
            {
                return (false);
            }

        }

        [Task]
        public bool AttakingMode()
        {
            if (attack == true)
            {
                return (true);
            }
            else
            {
                return (false);
            }

        }

        [Task]
        public void RestoreToGuard (){

            guardT = 0.0f;

                 if (restoreGuardLastPosValue < 1)
                {
                    lastRoot = arm_root.solver.GetIKPositionWeight();
                    lastLimbPos = bend.solver.GetIKPositionWeight();
                    lastLimbRot = bend.solver.GetIKRotationWeight();
                    restoreGuardLastPosValue = 1;
                }

                arm_root.solver.IKPositionWeight = Mathf.SmoothStep(lastRoot, root_Wight_GuardMin, restoreT);
                bend.solver.IKPositionWeight = Mathf.SmoothStep(lastLimbPos, Limb_IK_Pos_WeightMax, restoreT);
                bend.solver.IKRotationWeight = Mathf.SmoothStep(lastLimbRot, Limb_IK_Rot_WeightMin, restoreT);

                restoreT += 0.5f * Time.deltaTime;

                if (restoreT > 1.0f)
                {
                    restoreT = 0.0f;
                    restoreGuardLastPosValue = 0;
                    Task.current.Succeed();
                }
          
        }

        [Task]
        public void LeanBack()
        {
            guardMode = true;

            if (leanLastPosValue < 1)
            {
                lastRoot = arm_root.solver.GetIKPositionWeight();
                lastLimbPos = bend.solver.GetIKPositionWeight();
                lastLimbRot = bend.solver.GetIKRotationWeight();

                leanLastPosValue = 1;
            }

            

            arm_root.solver.IKPositionWeight = Mathf.SmoothStep(lastRoot, 0.027f, leanT);
            bend.solver.IKPositionWeight = Mathf.SmoothStep(lastLimbPos, 0.700f, leanBendT);
            bend.solver.IKRotationWeight = Mathf.SmoothStep(lastLimbRot, 0, leanT);

            leanBendT += 0.5f * Time.deltaTime;
            leanT += 0.5f * Time.deltaTime;



            if (leanT > 0.9f)
            {
                placedOnce = true;
                //leanT = 0.0f;
                //leanBendT = 0.0f;
                Task.current.Succeed();
            }
        }

        [Task]
        public void Leap()
        {
            
            
            bend.solver.IKPositionWeight = Mathf.SmoothStep(0.700f, 0, attackBendT);
            arm_root.solver.IKPositionWeight = Mathf.SmoothStep(0.027f, 1, attackT);
            bend.solver.IKRotationWeight = 0;

            attackBendT += 1.5f * attackSpeed * Time.deltaTime;
            attackT += attackSpeed * Time.deltaTime;

            if(attackT > 1.0f)
            {
                
                attackBendT = 0.0f;
                attackT = 0.0f;
                Task.current.Succeed();
            }

        }

        [Task]
        public void PlaceAttackPoint()
        {
           
            
            
                attackPoint.transform.position = goal.transform.position;
                arm.solver.target = attackPoint.transform;
                //placedOnce = true;
                Task.current.Succeed();
            
           
        }


        [Task]
        public void ResetAttack()
        {
            attackPoint.transform.position = this.transform.position;
            arm.solver.target = goal.transform;
            placedOnce = false;
            leanT = 0.0f;
            leanBendT = 0.0f;
            attackBendT = 0.0f;
            attackT = 0.0f;
            leanLastPosValue = 0;
            attack = false;
            
            Task.current.Succeed();
        }


        [Task]
        public void RotateTowardsPlayer()
        {
            Vector3 targetPosition = new Vector3(goal.transform.position.x, transform.position.y, goal.transform.position.z);

            transform.LookAt(targetPosition);
        }

        //public void Attack()
        //{
        //    float dist = Vector3.Distance(goal.transform.position, transform.position);
        //    if (dist < guardDistance)
        //    {
        //        arm.fixTransforms = true;
        //        arm.solver.IKPositionWeight = 0.090f;
        //        if (dist < attackDistance)
        //        {

        //            arm.fixTransforms = false;
        //            if (lean == true)
        //            {
        //                if (leanBackLerp < 1)
        //                {
        //                    leanBackLerp += Time.deltaTime * leanBackSpeed;
        //                    arm.solver.IKPositionWeight = Mathf.Lerp(0.09f, 0, leanBackLerp);
        //                    if (leanBackLerp <= 0)
        //                    {
        //                        attack = true;
        //                        lean = false;
        //                    }
        //                }
        //            }

        //            if (attack == true)
        //            {
        //                if (atkLerp < 1)
        //                {
        //                    atkLerp += Time.deltaTime * attackSpeed;
        //                    arm.solver.FixTransforms();
        //                    arm.solver.IKPositionWeight = Mathf.Lerp(0.09f, 1, atkLerp);
        //                    bend.solver.IKPositionWeight = 0.0f;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            // leanBackLerp = 0;
        //            atkLerp = 0;
        //        }

        //    }
        //    Debug.Log("Distance: " + dist);
        //}


    }
}
