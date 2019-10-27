using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSpell : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D Body;
    [SerializeField]
    private float HitDamage = 1f;
    private float LifetimeTimer = 2.5f;
    private float cLifetimeTimer = 0f;
    public void SetOwner(GameObject Owner) {
        gameObject.layer = Owner.layer;
    }

    void Update() {
        cLifetimeTimer += Time.deltaTime;
        if(cLifetimeTimer > LifetimeTimer) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D Collider) {
        DamagableUnit u = Collider.gameObject.GetComponent<DamagableUnit>();
        if(u != null) {
            if(u.gameObject.layer != gameObject.layer) {
                u.TakeDamage(HitDamage);
            }
        }
    }

    public void Shoot(
        Vector3 inDirection,
        float withForce
    ) {
        Body.AddForce(inDirection.normalized * withForce, ForceMode2D.Impulse);
    }
}
