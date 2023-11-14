using UnityEngine.AI;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
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
    private float fireCountdown = 0f;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
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

    private void Update()
    {
        if (target == null)
            return;
        //Target lock
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler (0f, rotation.y, 0f);

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;

    }
    void Shoot()
    {
        var bulletGO = Instantiate (bulletPrefap, firePoint.position, firePoint.rotation);
        bulletGO.GetComponent<Rigidbody>().velocity = firePoint.forward * bulletSpeed;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
