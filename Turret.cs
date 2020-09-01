using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Detecting Player")]
    [SerializeField] float rotationSpeed;
    [SerializeField] float detectionRange;
    [SerializeField] float detectionAngle;

    [Header("Restricting Rotation")]
    [SerializeField] bool restrictRotation;
    [SerializeField] float rotationAngle;
    
    [Header("Transforms")]
    [SerializeField] Transform target;
    [SerializeField] Transform turret;
    [SerializeField] Transform body;

    bool otherSide = false;

    bool actionMode = false;
    
    
    void Update()
    {
        if(!actionMode)
        {
            if(restrictRotation)
            {
                float angle = Vector3.SignedAngle(body.forward, turret.forward, Vector3.up);
                
                if(angle < rotationAngle && !otherSide)
                {
                    turret.Rotate(Vector3.up * rotationSpeed * 10 * Time.deltaTime);
                }
                else
                {
                    otherSide = true;
                }

                if(angle > -rotationAngle && otherSide)
                {
                    turret.Rotate(Vector3.down * rotationSpeed * 10 * Time.deltaTime);
                }
                else
                {
                    otherSide = false;
                }
            }
            else
            {
                turret.Rotate(Vector3.up * rotationSpeed * 10 * Time.deltaTime);
            }
            

        }
        

        float detectionAngles = GetTargetAngles(turret, target);

        Debug.Log(detectionAngles);

        bool lookRight = detectionAngles <= detectionAngle && detectionAngles >= 0;
        bool lookLeft = detectionAngles >= -detectionAngle && detectionAngles <= 0;

        bool withinDistance = Vector3.Distance(turret.position, target.position) <= detectionRange;

        if ((lookRight || lookLeft) && withinDistance)
        {
            actionMode = true;
            FaceTowards(target);
        }
        else
        {
            actionMode = false;
        }

            
    }

    void FaceTowards(Transform target)
    {
        Vector3 targetDirection = target.position - turret.position;

        targetDirection.y = 0;

        Vector3 newDirection = Vector3.RotateTowards(turret.forward, targetDirection, rotationSpeed * Time.deltaTime, 0.0f);

        Debug.DrawRay(turret.position, newDirection, Color.red);

        turret.rotation = Quaternion.LookRotation(newDirection);

    }

    private float GetTargetAngles(Transform Self, Transform Target)
    {
        //Declare         
        Vector3 SelfForwardXZ = new Vector3(Self.forward.x, 0f, Self.forward.z);

        Vector3 TargetForwardXZ = new Vector3(Target.position.x - Self.position.x, 0f, Target.position.z - Self.position.z).normalized;

        //Return         
        return Vector3.SignedAngle(SelfForwardXZ, TargetForwardXZ, Self.up);      //x

    }
}
