using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerMovement : MonoBehaviour
{
    GameData saveData = new GameData();

    bool alive = true;

    public float speed;
    public float speedSum;
    public float normalSpeed = 9.99f;
    public float driveSpeed = 15f;
    private BoxCollider boxCollider;
    private Animator _animator;
    private FollowPlayer followPlayer;
    EnemyMelee enemyMelee;
    public Health healthBar;
    public Stamina stamina;

    [Header("reference")]

    [SerializeField] private ParticleSystem vfxHealing;
    [SerializeField] private AudioSource healSound;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource coinSound;
    [SerializeField] private AudioSource slashSound;
    [SerializeField] private AudioSource looseSound;
    [SerializeField] private ParticleSystem vfxSmoke;
    [SerializeField] private ParticleSystem vfxSlash;
    [SerializeField] private ParticleSystem vfxDrive;
    [SerializeField] public Rigidbody rb;

    [Header("Setting")]
    [SerializeField] float horizontalMultiplier = 2;
    [SerializeField] float jumpForce = 2f;
    [SerializeField] float downForce = 800f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float detectRange = 0.2f;
    //[SerializeField] float value = -2f;
    [Header("Stat")]
    public int maxHealth;
    public int currentHealth;
    public int maxStamina = 100;
    public int currentStamina;
    //public int staminaOverTime = 1;
    public int damage = 5;


    public float normalAcceleration;
    [HideInInspector] public float acceleration;
    public float speedIncreasePerPoint = 0.1f;
    public bool isGrounded;
    public bool isFalling;
    public bool isWall = false;
    float horizontalInput;

    public Vector3 horizontalMove;
    public MovementState state;
    public enum MovementState
    {
        none,
        dashing
    }
    public bool none;
    public bool dashing;

    void Start()
    {
        LoadPlayer();
        vfxSmoke.Play();
        boxCollider = GetComponent<BoxCollider>();
        followPlayer = transform.GetChild(1).GetComponent<FollowPlayer>();
        //enemyMelee = GameObject.FindObjectOfType<EnemyMelee>();
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreLayerCollision(2, 3, false);
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        currentStamina = maxStamina;
        stamina.SetMaxStamina(maxStamina);
        speedSum = normalSpeed;
        //StartCoroutine(StaminaTime()); staminOverTime
        
    }

    private void FixedUpdate()
    {
        if (!alive) return;
        _animator = transform.GetChild(0).GetComponent<Animator>();
        
        speed = speedSum + (speedIncreasePerPoint * GameManager.inst.score);
        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        horizontalMove = transform.right * horizontalInput * speed * Time.fixedDeltaTime * horizontalMultiplier;
        rb.MovePosition(rb.position + forwardMove + horizontalMove);

    }

    private void Update()
    {
        if (!alive) return;
        // Check Ground
        RaycastHit hit;
        Ray rayLeft = new Ray(transform.position, -transform.right);
        Ray rayRight = new Ray(transform.position, transform.right);
        float height = GetComponent<Collider>().bounds.size.y;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, (height / 2f) + 0.1f, groundMask);
        //Physics.Raycast(transform.position, Vector3.down, (height / 2) + 0.1f, groundMask);
        horizontalInput = Input.GetAxis("Horizontal");
        

        // ตรวจสอบว่า ray ชน collider ด้านข้างหรือไม่
        if (Physics.Raycast(rayRight, out hit, detectRange))
        {
            isWall = true;
        }
        else if (Physics.Raycast(rayLeft, out hit, detectRange))
        {
            isWall = true;
        }
        else
        {
            isWall = false;
        }
        StateHandler();
        UsePotion();
        UseManaPotion();
        GroundCheck();
        RunFast();
        //StartCoroutine(StaminaTime());


        if (transform.position.y < -5)
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // Check if colliding with the enemy
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Play VFX
            //enemyMelee.CalDamage(damage);
            Slash();
            /*GameManager.inst.combo++;
            GameManager.inst.ComboSystem();*/
        }
        
    }
    /*private IEnumerator StaminaTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); // รอ 1 วินาที

            // ลด Stamina ตามอัตราที่กำหนด
            currentStamina -= staminaOverTime;
            stamina.SetStamina(currentStamina);
            Debug.Log(currentStamina);
        }
    }*/
    public void HitObstacle()
    {
        StartCoroutine(TimeIframe());
    }
    IEnumerator TimeIframe()
    {
        Physics.IgnoreLayerCollision(2, 3, true);
        _animator.SetTrigger("Iframe");

        yield return new WaitForSeconds(1);

        Physics.IgnoreLayerCollision(2, 3, false);
    }
    public void LoadPlayer()
    {
        saveData = SaveSystem.instance.LoadGame();
        
        maxHealth = saveData.maxHealth;
    }
    public void RunFast()
    {
        if (Input.GetKey(KeyCode.W))
        {
            speedSum = driveSpeed;
            vfxDrive.Play();
        }
        else
        {
            speedSum = normalSpeed;
            vfxDrive.Stop();
        }
        followPlayer.DynamicFOV();
    }
    public void Die()
    {
        alive = false;
        vfxSmoke.Stop();
        GameManager.inst.ScoreBoard();
        GameManager.inst.DashUI();
        GameManager.inst.soundBGM.Stop();
        looseSound.Play();
        _animator.SetTrigger("Death");
    }
    public void ChargeStamina(int Charge)
    {
        //vfxHealing.Play();
        //healSound.Play();
        if (currentStamina < maxStamina)
        {
            currentStamina += Charge;

            stamina.SetStamina(currentStamina);
        }
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
    }
    public bool activeSkill = true;
    public void CheckStamina(int Cost)
    {
        if (currentStamina >= Cost)
        {
            activeSkill = true;
        }
        
    }
    public void UseStamina(int amount)
    {
        if (!alive) return;
        if (currentStamina <= maxStamina)
        {
            Debug.Log(activeSkill);
            if (currentStamina >= amount)
            {
                currentStamina -= amount;
                stamina.SetStamina(currentStamina);
            }
            if (currentStamina < amount)
            {
                activeSkill = false;
            }
        }
    }
    public void Healing(int healing)
    {
        vfxHealing.Play();
        healSound.Play();
        if (currentHealth < maxHealth)
        {
            currentHealth += healing;

            healthBar.SetHealth(currentHealth);
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public void PickCoin()
    {
        GameManager.inst.IncrementScore();
        coinSound.Play();

    }
    public void TakeDamage(int damage)
    {
        if (!alive) return;
        currentHealth -= damage;
        GameManager.inst.combo = 0;
        healthBar.SetHealth(currentHealth);
        _animator.SetTrigger("GotDMG");
        hitSound.Play();
        healthBar.lerpTimer = 0f;
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Jump ()
    {
        // if we are, jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //rb.AddForce(Vector3.up * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            _animator.SetBool("Jump", true);
            //SlideTime = 0;
        }
        
    }
    public void Slash()
    {
        
        vfxSlash.Play();
        slashSound.Play();
        _animator.SetTrigger("Slash");
    }
    void ForceDown()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            rb.AddForce(new Vector3(0, -downForce, 0), ForceMode.Impulse);
        }
    }
    private void StateHandler()
    {
        // Mode - Dashing
        if (dashing)
        {
            state = MovementState.dashing;
            
        }

    }
    void UsePotion ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (GameManager.inst.potion > 0)
            {
                GameManager.inst.potion--;
                Healing(40);
            }
            else
            {
                Debug.Log("Not Enough Potion");
            }
        }
    }
    void UseManaPotion()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (GameManager.inst.manaPotion > 0)
            {
                GameManager.inst.manaPotion--;
                ChargeStamina(30);
            }
            else
            {
                Debug.Log("Not Enough Potion");
            }
        }
    }
    //private int SlideTime = 1;
    void Slide()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //SlideTime = 1;
            StartCoroutine(SlideC());

        }
        
    }
    private IEnumerator SlideC() 
    {
        _animator.SetBool("Slide", true);
        boxCollider.center = new Vector3(0, -0.25f, 0);
        boxCollider.size = new Vector3(1, 0.5f, 1);

        yield return new WaitForSeconds(1);

        _animator.SetBool("Slide", false);
        boxCollider.center = new Vector3(0, 0.08f, 0);
        boxCollider.size = new Vector3(1, 1.16f, 1);

    }
    void GroundCheck()
    {
        if (isGrounded)
        {
            _animator.SetBool("Move", true);
            _animator.SetBool("Falling", false);
            _animator.SetBool("Jump", false);
            vfxSmoke.Play();
            Jump();
            Slide();
        }
        else
        {
            _animator.SetBool("Falling", true);
            _animator.SetBool("Move", false);
            ForceDown();
            vfxSmoke.Stop();
        }
    }
}