using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Animator animator;
    int layerIndex;

    // Start is called before the first frame update
    void Start()
    {
        
        animator = GetComponent<Animator>();
        layerIndex = animator.GetLayerIndex("AAA");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            animator.SetLayerWeight(layerIndex, 1);
            animator.SetTrigger("Slash");
        }
        
    }
}
