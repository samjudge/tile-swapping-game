using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SfxManagerService : MonoBehaviour
{
    [SerializeField]
    private AudioSource Audio;
    [SerializeField]
    private AudioClip TileDing;
    [SerializeField]
    private AudioClip TilePop;
    [SerializeField]
    private AudioClip SkullPop;

    private static SfxManagerService Instance;

    void Awake() {
        Instance = this;
    }

    public static SfxManagerService GetInstance() {
        return Instance;
    }
    
    public void PlayTileDing() {
        Audio.PlayOneShot(TileDing, 0.5f);
    }

    public void PlayTilePop() {
        Audio.PlayOneShot(TilePop, 0.3f);
    }

    public void PlaySkullPop() {
        Audio.PlayOneShot(SkullPop, 1f);
    }
}