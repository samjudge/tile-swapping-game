using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierWeaponBehaviour : MonoBehaviour
{
    [SerializeField]
    private SoldierBehaviour Owner;

    public void OnTriggerEnter2D(Collider2D col) {
        DamagableUnit u = col.GetComponent<DamagableUnit>(); 
        if(u != null){
            if(u.gameObject.layer != Owner.gameObject.layer) {
                u.TakeDamage(Owner.HitDamage);
            }
        }
    }
}
