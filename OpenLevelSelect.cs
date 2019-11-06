using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLevelSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject WorldSelectUI;
    [SerializeField]
    private GameObject ToOpenLevelSelectUI;

    public void OpenUI() {
        WorldSelectUI.SetActive(false);
        ToOpenLevelSelectUI.SetActive(true);
    }
}
