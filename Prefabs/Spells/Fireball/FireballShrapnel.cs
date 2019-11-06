using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballShrapnel : MonoBehaviour
{
    [SerializeField]
    public Rigidbody2D Body;
    [SerializeField]
    private float Lifetime = 1f;
    private float cLifetime = 0;
    [SerializeField]
    Vector3 InitalScale;
    [SerializeField]
    List<DamagableUnit> AlreadyDamagedUnits;

    void Awake() {
        AlreadyDamagedUnits = new List<DamagableUnit>();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        DamagableUnit damagable = collider.GetComponent<DamagableUnit>();
        if(damagable != null && !AlreadyDamagedUnits.Contains(damagable)) {
            damagable.TakeDamage(3f);
            AlreadyDamagedUnits.Add(damagable);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(RayCasts.IsGroundTileBelowBy(transform, 0.025f)) Destroy(gameObject);
        cLifetime += Time.deltaTime;
    }
}
