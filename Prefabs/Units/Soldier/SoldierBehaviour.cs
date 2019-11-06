using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBehaviour : JumpingUnit
{
    [SerializeField]
    private Animator Animation;
    [SerializeField]
    private SoldierWeaponBehaviour Weapon;
    [SerializeField]
    public float AttackReach;
    [SerializeField]
    public float HitDamage = 3f;
    [SerializeField]
    private FreezableUnit Freezable;

    public override void Update(){
        base.Update();
        if(!Freezable.GetIsFrozen()) {
            AttackNearbyEnemies();
        }
    }

    public void AttackNearbyEnemies() {
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            transform.position,
            new Vector2(
                Body.velocity.x / Mathf.Abs(Body.velocity.x),
                0
            ),
            AttackReach
        );
        foreach(RaycastHit2D hit in hits) {
            DamagableUnit u = hit.transform.gameObject.GetComponent<DamagableUnit>();
            if(u != null){
                if(u.gameObject.layer != gameObject.layer) {
                    Animation.SetTrigger("SoldierAttack");
                    Weapon.gameObject.SetActive(true);
                }
            }
        }
    }

    public void EndAttack() {
        Animation.SetTrigger("SoldierAttackEnd");
        Weapon.gameObject.SetActive(false);
    }
}
