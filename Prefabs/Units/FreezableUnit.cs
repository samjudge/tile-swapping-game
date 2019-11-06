using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FreezableUnit : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D Body;
    [SerializeField]
    private Animator Animator;
    [SerializeField]
    private SpriteRenderer Sprite;
    [SerializeField]
    private TextEmitter TextEmitter;
    [SerializeField]
    private JumpingUnit JumpingUnit;
    private Color SavedColor;
    private bool IsFrozen = false;
    private float cFreezeTimeRemaining = 0f;

    public void FreezeFor(float duration) {
        cFreezeTimeRemaining += duration;
        if(!IsFrozen) {
            SavedColor = Sprite.color;
            Sprite.color = new Color(0.75f, 0.75f, 1f);
            IsFrozen = true;
            Body.velocity = Vector3.zero;
            Animator.speed = 0f;
        }
    }

    void Update() {
        if(IsFrozen) {
            JumpingUnit.AllowMovement = false;
            cFreezeTimeRemaining -= Time.deltaTime;
            if(cFreezeTimeRemaining <= 0) {
                Sprite.color = SavedColor;
                JumpingUnit.AllowMovement = true;
                IsFrozen = false;
                cFreezeTimeRemaining = 0;
                Animator.speed = 1f;
            }
        }
    }

    public bool GetIsFrozen() {
        return IsFrozen;
    }
}