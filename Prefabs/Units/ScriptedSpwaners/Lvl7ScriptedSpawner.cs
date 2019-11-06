using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Lvl7ScriptedSpawner : ScriptedSpawner
{
    [SerializeField]
    private UnitSpawner Spawner;

    private Coroutine SpawnRoutine;

    void Start() {
    }

    public override UnitSpawner GetSpawner() {
        return Spawner;
    }

    public override void StartSpawning() {
        LevelManagerService.GetInstance().SetTimeRemaining(360);
        SpawnRoutine = StartCoroutine(SpawnLoop());
    }

    public override void StopSpawning() {
        StopCoroutine(SpawnRoutine);
    }

    private IEnumerator SpawnLoop() {
        yield return new WaitForSeconds(3f);
        Spawner.SpawnSoldier();
        yield return new WaitForSeconds(3f);
        Spawner.SpawnWizard();
        yield return new WaitForSeconds(3f);
        Spawner.SpawnArcher();
        yield return new WaitForSeconds(1f);
        List<GameObject> Units = Spawner.GetAllSpawnedUnits();
        foreach(GameObject u in Units) {
            if(u != null) {
                if(u.transform.localScale.x < 2f){
                    StartCoroutine(SpellEffectsService.GetInstance().EmbiggenEffect(u));
                }
            }
        }
        SpawnRoutine = StartCoroutine(SpawnLoop());
    }

    public override void SetSpawner(UnitSpawner Spawner) {
        this.Spawner = Spawner;
    }
}
