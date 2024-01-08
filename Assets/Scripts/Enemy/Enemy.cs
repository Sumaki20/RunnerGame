using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour
{
    bool alive = true;

    private Animator _animator;
    //public Health healthBar;
    private Rigidbody rb;
    PlayerMovement playerMovement;


    [SerializeField] private ParticleSystem vfxHit;
    
    private Transform target;

    [Header("Unity Setup Fields")]
    public int level = 1;
    //public int damage;
    public int expGain = 5;
    public int maxHealth = 5;
    public int currentHealth;
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
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        _animator = transform.GetChild(0).GetComponent<Animator>();
        currentHealth = maxHealth;
        //healthBar.SetMaxHealth(maxHealth);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void Update()
    {
        if (!alive) return;
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
        if (!alive) return;
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
            HitAnything();
        }
    }
    /*public void TakeDamage(int damage)
    {
        if (!alive) return;
        //_animator.SetTrigger("IsHit");
        currentHealth -= damage;
        //healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            //Invoke("DestroyObject", 1f);
        }
    }
    public void Die()
    {
        alive = false;
        GameManager.inst.EnemyScore();
        rb.isKinematic = true; // หรือ false ตามที่คุณต้องการ
        rb.useGravity = false;
        
        // หรือปิดใช้งาน Collider ได้ด้วย
        Collider collider = GetComponent<Collider>();
        collider.enabled = false; // หรือ true ตามที่คุณต้องการ
        DestroyObject();
    }*/
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    /*private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "FireBall")
        {
            Debug.Log("Hit");
            HitAnything();
        }
    }*/
    public void RangeCalDMG()
    {
        if (!alive) return;
        //currentHealth -= damage;
        //_animator.SetTrigger("IsHit");
        if (currentHealth <= 0)
        {
            rb.isKinematic = true; // หรือ false ตามที่คุณต้องการ
            rb.useGravity = false; // หรือ true ตามที่คุณต้องการ

            // หรือปิดใช้งาน Collider ได้ด้วย
            Collider collider = GetComponent<Collider>();
            collider.enabled = false; // หรือ true ตามที่คุณต้องการ

            GameManager.inst.ComboSystem();
            // Add to the player's score
            GameManager.inst.EnemyScore();
            _animator.SetTrigger("Lose");
            GameManager.inst.GainExperienceFlatRate(expGain);
            Invoke("DestroyObject", 1f);
        }
    }
    public void HitAnything()
    {
        _animator.SetTrigger("Lose");
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

        GameManager.inst.ComboSystem();
        GameManager.inst.GainExperienceFlatRate(expGain);
        // Add to the player's score
        GameManager.inst.EnemyScore();
        /*if (currentHealth <= 0)
        {
            vfxHit.Play();
            //_animator.SetTrigger("Lose");
            GameManager.inst.GainExperienceFlatRate(expGain);
        }*/
        // Destroy this coin object
        Invoke("DestroyObject", 1f);
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