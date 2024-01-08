using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootMode : MonoBehaviour
{
    public Transform target;
    [Header("Settting")]
    public Image imageToMove;
    public float turnSpeed = 10f;
    public float range = 10f;
    public float detectionAngle = 180f;
    private Quaternion initialRotation;
    public bool aimMode = false;
    [Header("Shoot")]
    public Transform partToRotate;
    public GameObject bulletPrefap;
    public Transform firePoint;
    public float bulletSpeed = 10;
    [Header("Cursor")]
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Texture2D aimCursorTexture;
    private Vector2 hotSpot = Vector2.zero;

    private void Start()
    {
        initialRotation = partToRotate.rotation;
    }
    void Update()
    {
        Shoot();
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (aimMode == true)
            {
                Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
                aimMode = false;
            }
            else
            {
                Cursor.SetCursor(aimCursorTexture, new Vector2(aimCursorTexture.width/2, aimCursorTexture.height/2), CursorMode.Auto);
                aimMode = true;
            }
        }
        // สร้าง Ray จากตำแหน่งเมาส์
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // ทดสอบว่า Ray ชน Object ไหน
        if (Physics.Raycast(ray, out hit))
        {
            // เช็คว่า Object ที่ชนต้องมี Component ในการหมุน (เช่น Transform) และมี tag เป็น "Enemy"
            Transform objectToRotate = hit.transform;

            if (objectToRotate != null && objectToRotate.CompareTag("Enemy"))
            {
                // กำหนด Object ที่ถูกคลิกเป็นเป้าหมายในการหมุนต่อเนื่อง
                target = objectToRotate;
            }
        }
        // ตรวจสอบการคลิกเมาส์
        /*if (Input.GetMouseButton(0))
        {
            // สร้าง Ray จากตำแหน่งเมาส์
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ทดสอบว่า Ray ชน Object ไหน
            if (Physics.Raycast(ray, out hit))
            {
                // เช็คว่า Object ที่ชนต้องมี Component ในการหมุน (เช่น Transform) และมี tag เป็น "Enemy"
                Transform objectToRotate = hit.transform;

                if (objectToRotate != null && objectToRotate.CompareTag("Enemy"))
                {
                    // กำหนด Object ที่ถูกคลิกเป็นเป้าหมายในการหมุนต่อเนื่อง
                    target = objectToRotate;
                }
            }
        }*/

        // หมุน Object ตาม Object ที่ถูกคลิก
        RotateTowardsTarget();
    }

    void RotateTowardsTarget()
    {
        if (aimMode == false)
        {
            partToRotate.rotation = Quaternion.Lerp(partToRotate.rotation, initialRotation, Time.deltaTime * turnSpeed);
            imageToMove.enabled = false;
            target = null;
            return;
        }
        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            float distanceToTarget = dir.magnitude;
            dir.Normalize();
            float angleY = Vector3.Angle(transform.forward, dir);
            float angleX = Vector3.Angle(partToRotate.forward, dir);
            
            // Check distance and angles
            if (distanceToTarget <= range && Mathf.Abs(angleY) <= detectionAngle / 2 && Mathf.Abs(angleX) <= detectionAngle / 2)
            {
                if (imageToMove != null && target != null)
                {
                    // กำหนดตำแหน่งใหม่ของ UI Image ไปที่ตำแหน่งของ Target
                    imageToMove.rectTransform.position = Camera.main.WorldToScreenPoint(target.position);
                    imageToMove.enabled = true;
                }
                else
                {
                    Debug.LogWarning("Image or target is not assigned.");
                }
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
                partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
            }
            else
            {
                partToRotate.rotation = Quaternion.Lerp(partToRotate.rotation, initialRotation, Time.deltaTime * turnSpeed);
                imageToMove.enabled = false;
                target = null;
            }
        }
        
    }
    void Shoot()
    {
        if (aimMode != true)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            
            var bulletGO = Instantiate(bulletPrefap, firePoint.position, firePoint.rotation);
            bulletGO.GetComponent<Rigidbody>().velocity = firePoint.forward * bulletSpeed;
        }

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
