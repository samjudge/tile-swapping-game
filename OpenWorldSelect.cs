using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWorldSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject WorldSelectUI;
    [SerializeField]
    private GameObject LevelSelectUI;

    public void OpenUI() {
        LevelSelectUI.SetActive(false);
        WorldSelectUI.SetActive(true);
    }
}
