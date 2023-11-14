using UnityEngine;
using UnityEngine.VFX;

public class PlayerMovement : MonoBehaviour
{

    bool alive = true;
    
    public float speed = 5;

    private Animator _animator;

    public Health healthBar;
    public FadeHP playerUI;

    [Header("reference")]

    [SerializeField] private AudioSource healSound;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource soundBGM;
    [SerializeField] private AudioSource slashSound;
    [SerializeField] private AudioSource looseSound;
    [SerializeField] private VisualEffect vfxSmoke;
    [SerializeField] private ParticleSystem vfxSlash;
    [SerializeField] private ParticleSystem vfxHealing;
    [SerializeField] public Rigidbody rb;

    [Header("Setting")]
    public int maxHealth = 100;
    public int currentHealth;
    [SerializeField] float horizontalMultiplier = 2;
    [SerializeField] float jumpForce = 2f;
    [SerializeField] float downForce = 800f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float detectRange = 0.2f;
    [SerializeField] float value = -2f;

    public float normalAcceleration;
    [HideInInspector] public float acceleration;
    public float speedIncreasePerPoint = 0.1f;
    public bool isGrounded;
    public bool isFalling;
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
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    private void FixedUpdate()
    {
        if (!alive) return;
        _animator = transform.GetChild(0).GetComponent<Animator>();

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
            horizontalInput = Mathf.Clamp(value, -1f, 0);
        }
        if (Physics.Raycast(rayLeft, out hit, detectRange))
        {
            horizontalInput = Mathf.Clamp(value, 0, 1f);
        }
        StateHandler();

        if (alive)
        {
            GroundCheck();
        }

        if(transform.position.y < -5)
        {
            TakeDamage(100);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // Check if colliding with the enemy
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Play VFX
            Slash();
        }
        
    }

    public void Die()
    {
        alive = false;
        vfxSmoke.Stop();
        GameManager.inst.ScoreBoard();
        GameManager.inst.DashUI();
        soundBGM.Stop();
        looseSound.Play();
        _animator.SetTrigger("Death");
    }
    public void Healing(int healing)
    {
        vfxHealing.Play();
        healSound.Play();
        if (currentHealth < 100)
        {
            currentHealth += healing;

            healthBar.SetHealth(currentHealth);
        }
        if (currentHealth > 100)
        {
            currentHealth = maxHealth;
        }
    }
    public void TakeDamage(int damage)
    {
        if (!alive) return;
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
        _animator.SetTrigger("GotDMG");
        playerUI.ShowUIplayer();
        hitSound.Play();
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Jump ()
    {
        // if we are, jump
        if (Input.GetKeyDown(KeyCode.W))
        {
            //rb.AddForce(Vector3.up * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            _animator.SetBool("Jump", true);
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
    void GroundCheck()
    {
        if (isGrounded)
        {
            _animator.SetBool("Move", true);
            _animator.SetBool("Falling", false);
            _animator.SetBool("Jump", false);
            Jump();

        }
        else
        {
            _animator.SetBool("Falling", true);
            _animator.SetBool("Move", false);
            ForceDown();
            vfxSmoke.Play();
        }
    }
}