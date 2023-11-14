using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private VisualEffect vfxHit;
    
    private Transform target;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Player";
    public Transform partToRotate;
    public float turnSpeed = 10f;

    public GameObject bulletPrefap;
    public Transform firePoint;
    public float bulletSpeed = 10;

    [Header("Attributes")]
    public float range = 1f;
    public float fireRate = 1f;
    [SerializeField] private float fireCountdown = 1f;

    private void Start()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
        
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void Update()
    {
        if (target == null)
            return;
        //Target lock
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;

    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (collision.gameObject.GetComponent<Obstacle>() != null)
        {
            Destroy(gameObject);
            return;
        }

        // Check that the object we collided with is the player
        if (collision.gameObject.name == "Player")
        {
            HitAnything();
        }
        if(collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("iioi");
            HitAnything();
        }
    }
    /*private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "FireBall")
        {
            Debug.Log("Hit");
            HitAnything();
        }
    }*/
    public void HitAnything()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        _animator.SetTrigger("IsHit");
        vfxHit.Play();

        /*Vector3 pushDirection = transform.position - collision.transform.position;
        pushDirection.Normalize();


        float pushForce = 10f;

        rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);*/
        rb.isKinematic = true; // หรือ false ตามที่คุณต้องการ
        rb.useGravity = false; // หรือ true ตามที่คุณต้องการ

        // หรือปิดใช้งาน Collider ได้ด้วย
        Collider collider = GetComponent<Collider>();
        collider.enabled = false; // หรือ true ตามที่คุณต้องการ

        // Add to the player's score
        GameManager.inst.EnemyScore();
        // Destroy this coin object
        Invoke("DestroyObject", 1f);
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Shoot()
    {
        
        _animator.SetTrigger("Cast");
        var bulletGO = Instantiate(bulletPrefap, firePoint.position, firePoint.rotation);
        bulletGO.GetComponent<Rigidbody>().velocity = firePoint.forward * bulletSpeed; 
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}