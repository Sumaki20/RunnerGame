using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Counter : MonoBehaviour
{
    public GameManager gameManager;

    [SerializeField] private VisualEffect vfxCounter;
    [SerializeField] private AudioSource counterSound;
    public Animator _animator;
    bool counterAttack = true;
    bool check = true;

    [Header("Cooldown")]
    public float counterCD = 1f;
    private float counterCdTimer;

    [Header("activeTime")]
    public float activeTimeSet = 1f;
    private float activeTime;

    void Update()
    {
        CoolDownSkill();
        ActiveSkill();
    }

    private void OnTriggerEnter(Collider collision)
    {
        Rigidbody rb = collision.GetComponent<Rigidbody>();
        if (check && collision.CompareTag("Bullet"))
        {
            vfxCounter.Play();
            counterSound.Play();
            _animator.SetTrigger("Countering");
            Debug.Log("Counter Attack!");
            rb.velocity = -rb.velocity * 1.5f;
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
            Debug.Log("CD: " + counterCdTimer);
            counterCdTimer -= Time.deltaTime;
            counterAttack = false;
        }

        if (counterCdTimer <= 0)
        {
            counterAttack = true; // เปิดให้เริ่มต้นเช็ค Collider ได้
        }
        
    }
    void ActiveSkill()
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
        }
        if (counterAttack && Input.GetKey(KeyCode.J))
        {
            activeTime = activeTimeSet;
            counterCdTimer = counterCD;
        }
    }
}