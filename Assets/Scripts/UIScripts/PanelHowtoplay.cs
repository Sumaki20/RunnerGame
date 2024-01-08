using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelHowtoplay : MonoBehaviour
{

    
    private void Start()
    {
        EventTriggerListener.GetListener(gameObject).onClick += OnPanelClick;
    }

    private void OnPanelClick(GameObject go)
    {
        MainMenu.inst.HowToPlayOff();
        MainMenu.inst.SettingMenuOff();
        MainMenu.inst.UpgradeOff();
    }
}
