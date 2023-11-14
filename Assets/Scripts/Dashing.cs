using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dashing : MonoBehaviour
{
    private Animator _animator;
    
    [SerializeField] private Slider cdDash;
    [SerializeField] private ParticleSystem vfxDashingRight;
    [SerializeField] private ParticleSystem vfxDashingLeft;
    [SerializeField] private AudioSource dashingSound;

    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    //private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Dashing")]
    public float dashSpeed; // ความเร็วของการ Dash
    public float dashDistance; // ระยะทางที่ต้องการให้ Dash

    [Header("Cooldown")]
    public float dashCD;
    private float dashCdTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.Space;

    private void Start()
    {
        //rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        SetMaxCDDash(dashCD);
    }

    private void Update()
    {
        
        Dash();
        
        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
        
    }
    private void Dash()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        _animator = transform.GetChild(0).GetComponent<Animator>();
        SetCDDash(dashCdTimer);

        
        if (dashCdTimer > 0) return;
        if (horizontalInput != 0 && Input.GetKey(dashKey))
        {

            if (horizontalInput > 0)
            {
                _animator.SetTrigger("DashRight");
                vfxDashingLeft.Play();
                dashingSound.Play();
            }
            if (horizontalInput < 0)
            {
                
                _animator.SetTrigger("DashLeft");
                vfxDashingRight.Play();
                dashingSound.Play();
            }
            dashCdTimer = dashCD;

            pm.dashing = true;

            // คำนวณเวลาที่จะใช้ในการ Dash
            float dashTime = dashDistance / dashSpeed;

            // คำนวณตำแหน่งที่ต้องการไป
            Vector3 targetPosition = transform.position + orientation.right * horizontalInput * dashDistance;

            // เริ่ม Coroutine สำหรับการเคลื่อนที่
            StartCoroutine(DashCoroutine(targetPosition, dashTime));
            
        }
        else
        {
            vfxDashingRight.Stop();
            vfxDashingLeft.Stop();
        }
    }

    private IEnumerator DashCoroutine(Vector3 targetPosition, float dashTime)
    {
        float elapsedTime = 0f;

        // ทำ Loop จนกว่าจะครบเวลาที่กำหนด
        while (elapsedTime < dashTime)
        {
            // คำนวณตำแหน่งใหม่ที่ต้องการไป
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, elapsedTime / dashTime);

            // ย้าย Object ไปที่ตำแหน่งใหม่
            transform.position = newPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // เมื่อเสร็จสิ้น Dash, ปรับตำแหน่งให้เป็นที่ตั้งตามทิศทางหลังจาก Dash
        transform.position = targetPosition;

        // รอจนกว่าจะเสร็จสิ้นการ Dash ในเวลาที่กำหนด
        //yield return new WaitForSeconds(0.25f);
        vfxDashingRight.Stop();
        vfxDashingLeft.Stop();
        // หลังจากเสร็จสิ้นการรอ, ปิดสถานะการ Dash
        pm.dashing = false;
    }

    public void SetMaxCDDash(float coolDown)
    {
        cdDash.maxValue = coolDown;
        cdDash.value = coolDown;

    }

    public void SetCDDash(float coolDown)
    {
        cdDash.value = coolDown;
    }
    /*private void Dash()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector3 forceToApply = new Vector3();

        pm.dashing = true;

        if (dashCdTimer > 0) return;
        if (horizontalInput != 0 && Input.GetKey(dashKey)) // ตรวจสอบว่ามีการกดปุ่มทางขวาหรือซ้าย
        { 
            dashForce = Mathf.Abs(dashForce) * Mathf.Sign(horizontalInput); // กำหนด dashForce ให้เป็นค่าบวกหรือลบตามทิศทาง
            dashCdTimer = dashCd;
            if (dashForce > 0)
            {
                vfxDashingLeft.Play();

            }
            if (dashForce < 0)
            {
                vfxDashingRight.Play();
            }

            Invoke(nameof(DelayedDashForce), 0.025f);
            Invoke(nameof(ResetDash), dashDuration);
        }
        else
        {
            vfxDashingRight.Stop();
            vfxDashingLeft.Stop();
        }
        forceToApply = orientation.right * dashForce;
        delayedForceToApply = forceToApply;


    }

    private Vector3 delayedForceToApply;

    private void DelayedDashForce()
    {
        //rb.AddForce(delayedForceToApply, ForceMode.Impulse);
        transform.Translate(delayedForceToApply * Time.deltaTime);
    }
    private void ResetDash()
    {
        pm.dashing = false;
    }*/
}
