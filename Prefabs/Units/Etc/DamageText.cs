using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class DamageText : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D Body;
    [SerializeField]
    Text Content;
    [SerializeField]
    private float BeginFadeAfterTimer = 1f;
    private float cBeginFadeAfterTimer = 0f;
    private bool IsFading = false;
    [SerializeField]
    private float DestroyAfterTimer = 3f;
    private float cDestroyAfterTimer = 0f;

    void Start() {
        Body.AddForce(new Vector2(Dice.Roll(-0.5f, 0.5f), 2f), ForceMode2D.Impulse);
    }

    void Update() {
        if(!IsFading) {
            cBeginFadeAfterTimer += Time.deltaTime;
            if(cBeginFadeAfterTimer > BeginFadeAfterTimer) {
                IsFading = true;
            }
        } else {
            cDestroyAfterTimer += Time.deltaTime;
            Color nColor = Content.color;
            nColor.a = 1 - cDestroyAfterTimer/DestroyAfterTimer;
            Content.color = nColor;
            if(cDestroyAfterTimer > DestroyAfterTimer) {
                Destroy(gameObject);
            }
        }
    }

}
