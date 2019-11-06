using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehaviour : JumpingUnit
{
    private float FireTimer = 6f;
    private float cFireTimer = 0f;

    [SerializeField]
    private Arrow ProjectilePrefab;
    [SerializeField]
    private SpriteRenderer Renderer;
    [SerializeField]
    private Animator Animation;
    [SerializeField]
    private FreezableUnit Freezable;

    public override void Update() {
        base.Update();
        cFireTimer += Time.deltaTime;
        if(cFireTimer > FireTimer) {
            cFireTimer = 0f;
            if(!Freezable.GetIsFrozen()) {
                BeginAttack();
            }
        }
    }

    private void BeginAttack() {
        Animation.SetTrigger("ArcherAttack");
    }

    //used in animation event
    public void FireProjectile() {
        Arrow Projectile = Instantiate(ProjectilePrefab);
        Projectile.GetComponentInChildren<SpriteRenderer>().color = Renderer.color;
        Projectile.transform.position = transform.position;
        Projectile.transform.SetParent(transform.parent, true);
        Projectile.SetOwner(gameObject);
        Projectile.Shoot(
            new Vector3(
                Body.velocity.x > 0 ? 1f : -1f,
                0.65f,
                0f
            ),
            0.2f
        );
    }
}