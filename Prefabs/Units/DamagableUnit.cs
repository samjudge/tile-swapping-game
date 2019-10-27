using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DamagableUnit : MonoBehaviour
{
    [SerializeField]
    public float Hp = 10;
    [SerializeField]
    public TextEmitter TextEmitter;

    void Update() {
        if(Hp <= 0) {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float Dmg) {
        Text t = TextEmitter.MakeText(
            Dmg.ToString()
        );
        t.transform.position = transform.position;
        Hp -= Dmg;
    }
}