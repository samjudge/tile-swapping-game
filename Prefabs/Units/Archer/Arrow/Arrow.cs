using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D Body;
    [SerializeField]
    private Transform Renderable;
    private bool StuckInGround = false;
    [SerializeField]
    private float HitDamage = 4f;

    public void Update() {
        if(!StuckInGround){
            Vector3 normVelocity = Body.velocity.normalized;
            //Calculate renderable rotation
            Renderable.right = transform.position - (transform.position + normVelocity);
            if(RayCasts.IsGroundTileBelowBy(transform, 0.05f)) {
                StuckInGround = true;
                transform.position = transform.position + (Vector3) Body.velocity * Time.deltaTime;
                StartCoroutine(StayStillThenDestroy());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D Collider) {
        DamagableUnit u = Collider.gameObject.GetComponent<DamagableUnit>();
        if(u != null) {
            if(u.gameObject.layer != gameObject.layer) {
                u.TakeDamage(HitDamage);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator StayStillThenDestroy() {
        Destroy(Body);
        yield return new WaitForSeconds(2f);
        if(gameObject != null) {
            Destroy(gameObject);
        }
    }

    public void SetOwner(GameObject Owner) {
        gameObject.layer = Owner.layer;
    }

    public void Shoot(
        Vector3 inDirection,
        float withForce
    ) {
        Body.AddForce(inDirection.normalized * withForce, ForceMode2D.Impulse);
    }

    
}