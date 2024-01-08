using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Counter : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerMovement playerMovement;
    [SerializeField] private ParticleSystem vfxCounter;
    [SerializeField] private AudioSource counterSound;
    public int manaCost = 20;
    public Animator _animator;
    bool counterAttack = true;
    bool check = false;
    bool isActive = false;
    public KeyCode counterButton = KeyCode.Q;
    [Header("Cooldown")]
    public float counterCD = 1f;
    private float counterCdTimer;

    [Header("activeTime")]
    public float activeTimeSet = 1f;
    private float activeTime;


    void Update()
    {
        CoolDownSkill();
        ActiveSkill(isActive);
        playerMovement.CheckStamina(manaCost);
        if (counterAttack && Input.GetKeyDown(counterButton))
        {
            if (playerMovement.activeSkill == true)
            {
                playerMovement.UseStamina(manaCost);
                activeTime = activeTimeSet;
                counterCdTimer = counterCD;
                isActive = true;
            }
            else
            {
                playerMovement.UseStamina(20);
            }
            
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Rigidbody rb = collision.GetComponent<Rigidbody>();
        if (playerMovement.activeSkill == true && check && collision.CompareTag("Bullet"))
        {
            vfxCounter.Play();
            counterSound.Play();
            _animator.SetTrigger("Countering");
            //Debug.Log("Counter Attack!");
            rb.velocity = -rb.velocity * (playerMovement.speed/5);
            activeTime = 0;
            check = false;
            gameManager.DoSlowmotion();
        }
    }

    void CoolDownSkill()
    {
        // CoolDown
        if (counterCdTimer > 0)
        {
            //Debug.Log("CD: " + counterCdTimer);
            counterCdTimer -= Time.deltaTime;
            counterAttack = false;
        }

        if (counterCdTimer <= 0)
        {
            counterAttack = true; // เปิดให้เริ่มต้นเช็ค Collider ได้
        }
        
    }
    void ActiveSkill(bool isActive)
    {
        if (isActive == true)
        {
            // Active
            if (activeTime > 0f)
            {
                _animator.SetBool("Counter", true);
                check = true;
                activeTime -= Time.deltaTime;
                Debug.Log("Checked: " + activeTime);
            }
            if (activeTime <= 0f)
            {
                _animator.SetBool("Counter", false);
                check = false;
                isActive = false;
            }
        }
    }
}