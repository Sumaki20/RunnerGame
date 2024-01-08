using UnityEngine;
using UnityEngine.VFX;

public class EnemyMelee : MonoBehaviour
{
    bool alive = true;
    private Animator _animator;
    PlayerMovement playerMovement;
    //public Health healthBar;


    [SerializeField] public Rigidbody rb;
    [SerializeField] private ParticleSystem vfxHit;
    
    private Transform target;

    [Header("Unity Setup Fields")]
    public int damage = 10;
    public string enemyTag = "Player";
    public int level = 1;
    public int maxHealth = 10;
    public int expGain = 10;
    public int currentHealth;
    public Transform partToRotate;
    public float turnSpeed = 10f;
    //public float speed = 4f;

    /*public GameObject bulletPrefap;
    public Transform firePoint;
    public float bulletSpeed = 10;*/

    [Header("Attributes")]
    public float range = 1f;

    /*public float fireRate = 1f;
    [SerializeField] private float fireCountdown = 1f;*/

    private void Start()
    {
        //levelSystem = GameObject.FindObjectOfType<LevelSystem>();
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        _animator = transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        //healthBar.SetMaxHealth(maxHealth);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void Update()
    {
        if (!alive) return;

        /*Vector3 forwardMove = -transform.forward * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove);*/

        if (target == null)
            return;
        //Target lock
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!alive) return;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (collision.gameObject.GetComponent<Obstacle>() != null)
        {
            Destroy(gameObject);
            return;
        }

        // Check that the object we collided with is the player
        if (collision.gameObject.name == "Player")
        {
            //CalDamage(playerMovement.damage);
            playerMovement.TakeDamage(damage);
            GameManager.inst.ComboSystem();
            CalDamage(playerMovement.damage);
        }
        if(collision.gameObject.CompareTag("Bullet"))
        {
            RangeCalDMG();
            //Debug.Log("iioi");
            //HitAnything();
        }
    }
    public void RangeCalDMG()
    {
        if (!alive) return;
        currentHealth -= damage;
        _animator.SetTrigger("IsHit");
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
    public void CalDamage(int damage)
    {
        if (!alive) return;
        currentHealth -= damage;
        
        //healthBar.SetHealth(currentHealth);
        HitAnything();

    }

    public void HitAnything()
    {
        _animator.SetTrigger("Attack");

        
        rb.isKinematic = true; // หรือ false ตามที่คุณต้องการ
        rb.useGravity = false; // หรือ true ตามที่คุณต้องการ

        // หรือปิดใช้งาน Collider ได้ด้วย
        Collider collider = GetComponent<Collider>();
        collider.enabled = false; // หรือ true ตามที่คุณต้องการ

        GameManager.inst.ComboSystem();
        // Add to the player's score
        GameManager.inst.EnemyScore();

        if (currentHealth <= 0)
        {
            vfxHit.Play();
            _animator.SetTrigger("Lose");
            GameManager.inst.GainExperienceFlatRate(expGain);
        }

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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}