using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyUnitOnTouch : MonoBehaviour
{
   public void OnTriggerEnter2D(Collider2D collider){
        DamagableUnit u = collider.gameObject.GetComponent<DamagableUnit>();
        if(u != null) {
            u.TakeDamage(999);
        }
        Arrow a = collider.gameObject.GetComponent<Arrow>();
        if(a != null) {
            Destroy(a.gameObject);
        }
        Fireball f = collider.gameObject.GetComponent<Fireball>();
        if(f != null) {
            Destroy(f.gameObject);
        }
   }
}
