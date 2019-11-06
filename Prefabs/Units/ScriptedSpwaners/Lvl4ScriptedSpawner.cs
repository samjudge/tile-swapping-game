using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Lvl4ScriptedSpawner : ScriptedSpawner
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
        LevelManagerService.GetInstance().SetTimeRemaining(120);
        SpawnRoutine = StartCoroutine(SpawnLoop());
    }

    public override void StopSpawning() {
        StopCoroutine(SpawnRoutine);
    }

    private IEnumerator SpawnLoop() {
        Spawner.SpawnWizard();
        Spawner.SpawnWizard();
        Spawner.SpawnWizard();
        yield return new WaitForSeconds(4f);
        Spawner.SpawnSoldier();
        Spawner.SpawnSoldier();
        yield return new WaitForSeconds(4.5f);
        Spawner.SpawnArcher();
        Spawner.SpawnArcher();
        Spawner.SpawnArcher();
        yield return new WaitForSeconds(4.5f);
        SpawnRoutine = StartCoroutine(SpawnLoop());
    }

    public override void SetSpawner(UnitSpawner Spawner) {
        this.Spawner = Spawner;
    }
}
