using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    //components
    [SerializeField]
    private Image Image;
    private Camera Camera;
    [SerializeField]
    private Animator Animator;
    //data (from SOs)
    public Sprite Sprite;
    public String Name;

    void Awake() {
        Image.sprite = Sprite;
        Camera = Camera.main;
    }

    public void Remove(){
        Animator.SetTrigger("RemoveTile");
    }
    
    public void Delete(){
        Destroy(gameObject);
    }

    public IEnumerator TransitionTo(Vector3 pos, After then){
        float tCurrent = 0;
        float tMax = 0.1f;
        while(tCurrent < tMax){
            transform.position = Vector3.Lerp(
                transform.position,
                pos,
                tCurrent / tMax
            );
            tCurrent += Time.deltaTime;
            yield return null;
        }
        transform.position = pos;
        then();
    }

    public delegate void After();
}
