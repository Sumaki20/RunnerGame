using UnityEngine;

public class FadeHP : MonoBehaviour
{
    private Animator _animatorUI;
    private void Start()
    {
        _animatorUI = transform.GetComponent<Animator>();
    }

    public void ShowUIplayer()
    {
        _animatorUI.SetTrigger("GotDamage");
    }
}
