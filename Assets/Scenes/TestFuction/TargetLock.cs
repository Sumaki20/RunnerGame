using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetLock : MonoBehaviour
{
    public Transform[] target;
    public Transform partToRotate;
    public Image imageTargetLock;
    //public Image imageToMove;
    private Quaternion initialRotation;
    public float turnSpeed = 10f;
    public float range = 10f;
    public float detectionAngle = 180f;
    public bool aimMode = false;

    private int currentTargetIndex = 0;  // Added variable to track the current target index

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        // Store the initial rotation of partToRotate
        initialRotation = partToRotate.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SelectNextTarget();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SelectPreviousTarget();
        }
        
        if (target == null || target.Length == 0)
        {
            // When no targets are available, rotate partToRotate back to the initial position
            partToRotate.rotation = Quaternion.Lerp(partToRotate.rotation, initialRotation, Time.deltaTime * turnSpeed);
        }
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            if (aimMode == true)
            {
                aimMode = false;
            }
            else
            {
                aimMode = true;
            }
        }
        TargetLockOn();
    }
    void TargetLockOn()
    {
        
        // Target lock
        Transform currentTarget = target[currentTargetIndex];

        
        if (currentTarget != null)
        {
            Vector3 dir = currentTarget.position - transform.position;
            float distanceToTarget = dir.magnitude;
            dir.Normalize();
            float angleY = Vector3.Angle(transform.forward, dir);
            float angleX = Vector3.Angle(partToRotate.forward, dir);
            if (aimMode == false)
            {
                imageTargetLock.enabled = false;
                return;
            }
            else
            {
                imageTargetLock.enabled = true;
            }
            // Check distance and angles
            if (distanceToTarget <= range && Mathf.Abs(angleY) <= detectionAngle / 2 && Mathf.Abs(angleX) <= detectionAngle / 2)
            {
                if (imageTargetLock != null && target != null)
                {
                    // กำหนดตำแหน่งใหม่ของ UI Image ไปที่ตำแหน่งของ Target
                    imageTargetLock.rectTransform.position = Camera.main.WorldToScreenPoint(currentTarget.position);
                }
                else
                {
                    Debug.LogWarning("Image or target is not assigned.");
                }
                // Rotate only when within range and desired angles
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
                partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
            }
            else
            {
                // If not within range or desired angles, reset rotation to initial position
                partToRotate.rotation = Quaternion.Lerp(partToRotate.rotation, initialRotation, Time.deltaTime * turnSpeed);
            }
        }
    }
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<Transform> validTargets = new List<Transform>();

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= range)
            {
                validTargets.Add(enemy.transform);
            }
        }

        // Set the target array to all valid targets found
        target = validTargets.ToArray();

        // If no new targets and array is not empty, reset the current target index to the first target
        if (target.Length > 0 && currentTargetIndex >= target.Length)
        {
            currentTargetIndex = 0;
        }
    }
    // Add this method to be called when the button is pressed to select the next target
    public void SelectNextTarget()
    {
        if (target != null && target.Length > 0)
        {
            currentTargetIndex = (currentTargetIndex + 1) % target.Length;
        }
    }
    public void SelectPreviousTarget()
    {
        if (target != null && target.Length > 0)
        {
            currentTargetIndex = (currentTargetIndex - 1) % target.Length;
        }
    }
}
