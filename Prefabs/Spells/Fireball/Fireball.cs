using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField]
    public Rigidbody2D Body;
    [SerializeField]
    public FireballShrapnel FireballShrapnelPrefab;

    void Update() {
        if(RayCasts.IsGroundTileBelowBy(transform, 0.1f)) {
            int roll = Dice.Roll(20, 25);
            for(int x = 0; x < roll; x++) {
                float xForce = Dice.Roll(-1f,1f);
                float yForce = Dice.Roll(0.5f,1f);
                FireballShrapnel shrapnel = Instantiate(FireballShrapnelPrefab);
                shrapnel.gameObject.layer = gameObject.layer;
                shrapnel.transform.position = transform.position;
                shrapnel.Body.AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse);
                shrapnel.transform.SetParent(transform.parent);
            }
            Destroy(gameObject);
        }
    }
}
