using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestImg : MonoBehaviour
{
    public Image imageToMove;
    public Transform target;

    void Start()
    {
        // สร้างคำสั่งเริ่มต้น (ถ้าต้องการ)
    }

    void Update()
    {
        // เลื่อนตำแหน่งของ UI Image ไปที่ตำแหน่งของ Target
        MoveImageToTargetPosition();
    }

    void MoveImageToTargetPosition()
    {
        if (imageToMove != null && target != null)
        {
            // กำหนดตำแหน่งใหม่ของ UI Image ไปที่ตำแหน่งของ Target
            imageToMove.rectTransform.position = Camera.main.WorldToScreenPoint(target.position);
        }
        else
        {
            Debug.LogWarning("Image or target is not assigned.");
        }
    }
}
