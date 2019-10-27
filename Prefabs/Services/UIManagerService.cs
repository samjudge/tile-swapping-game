using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManagerService : MonoBehaviour
{
    [SerializeField]
    private GameObject MenusContainer;
    [SerializeField]
    private GameObject MainMenu;
    [SerializeField]
    private GameObject ChallengeMenu;
    [SerializeField]
    private GameObject TilesContainer;
    private static UIManagerService Instance;
    [SerializeField]
    private Sprite CompletedLevelIcon;
    [SerializeField]
    private List<StringImagePair> LevelButtons;

    void Awake() {
        //720, 1280
        //360, 640
        Screen.SetResolution(360, 640, false);
        Instance = this;
    }

    public static UIManagerService GetInstance() {
        return Instance;
    }

    public void MarkLevelAsComplete(String LevelName) {
        foreach(StringImagePair p in LevelButtons){
            if(p.ButtonName == LevelName) {
                p.Image.sprite = CompletedLevelIcon;
                break;
            }
        }
    }

    public void ShowMainMenu() {
        MenusContainer.SetActive(true);
        MainMenu.SetActive(true);
        ChallengeMenu.SetActive(false);
        //
        TilesContainer.SetActive(false);
    }

    public void ShowChallengeMenu() {
        MenusContainer.SetActive(true);
        MainMenu.SetActive(false);
        ChallengeMenu.SetActive(true);
        //
        TilesContainer.SetActive(false);
    }

    public void ShowTilesContainer() {
        MenusContainer.SetActive(false);
        MainMenu.SetActive(false);
        ChallengeMenu.SetActive(false);
        //
        TilesContainer.SetActive(true);
    }
}