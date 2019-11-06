using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject SoldierPrefab;
    [SerializeField]
    private GameObject ArcherPrefab;
    [SerializeField]
    private GameObject WizardPrefab;
    [SerializeField]
    private Transform BattlefieldParent;
    [SerializeField]
    private string SpawnedUnitCollisionTag;
    [SerializeField]
    private Vector3 JumpDirection;
    [SerializeField]
    private Color UnitColor;
    [SerializeField]
    private float UnitRotation;
    [SerializeField]
    private float SpawnForceXMax;
    [SerializeField]
    private float SpawnForceYMin;
    [SerializeField]
    private float SpawnForceYMax;
    [SerializeField]
    private Canvas TextEmitterCanvas;
    [SerializeField]
    private GroundClaimsService GroundClaimsService;
    private List<GameObject> SpawnedUnits;
    private bool IsLocked = false;

    void Awake(){
        SpawnedUnits = new List<GameObject>();
    }

    public void LockSpawner() {
        IsLocked = true;
    }

    public void UnlockSpawner() {
        IsLocked = false;
    }

    public void SpawnSoldier(){
        if(IsLocked) return;
        GameObject solider = Instantiate(SoldierPrefab);
        BasicUnitSetup(solider);
    }

    public void SpawnArcher(){
        if(IsLocked) return;
        GameObject archer = Instantiate(ArcherPrefab);
        BasicUnitSetup(archer);
    }

    public void SpawnWizard(){
        if(IsLocked) return;
        GameObject wizard = Instantiate(WizardPrefab);
        BasicUnitSetup(wizard);
    }

    private void BasicUnitSetup(GameObject newUnit) {
        newUnit.transform.position = transform.position;
        newUnit.transform.SetParent(BattlefieldParent, true);
        newUnit.layer = LayerMask.NameToLayer(SpawnedUnitCollisionTag);
        newUnit.GetComponent<SpriteRenderer>().color = UnitColor;
        newUnit.GetComponent<JumpingUnit>().JumpDirection = JumpDirection;
        newUnit.GetComponent<DamagableUnit>().TextEmitter.Canvas = TextEmitterCanvas;
        newUnit.transform.rotation = Quaternion.Euler(0, UnitRotation, 0);
        newUnit.GetComponent<Rigidbody2D>().AddForce(
            new Vector2(
                Dice.Roll(0f,SpawnForceXMax),
                Dice.Roll(SpawnForceYMin, SpawnForceYMax)
            ),
            ForceMode2D.Impulse
        );
        SpawnedUnits.Add(newUnit);
    }

    public void RemoveAllSpawnedUnits() {
        foreach(GameObject u in SpawnedUnits) {
            if(u != null) Destroy(u);
        }
    }

    public List<GameObject> GetAllSpawnedUnits() {
        return SpawnedUnits;
    }
}
