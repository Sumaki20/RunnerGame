using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelClickHandler : MonoBehaviour
{
    private void Start()
    {
        EventTriggerListener.GetListener(gameObject).onClick += OnPanelClick;
    }

    private void OnPanelClick(GameObject go)
    {
        // โหลดฉากที่คุณต้องการ (เปลี่ยน "SceneName" เป็นชื่อของฉากที่คุณต้องการโหลด)
        SceneManager.LoadScene("MainMenu");
    }
}