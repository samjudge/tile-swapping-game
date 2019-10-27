using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Lvl5ScriptedSpawner : ScriptedSpawner
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
        LevelManagerService.GetInstance().SetTimeRemaining(180);
        SpawnRoutine = StartCoroutine(SpawnLoop());
    }

    public override void StopSpawning() {
        StopCoroutine(SpawnRoutine);
    }

    private IEnumerator SpawnLoop() {
        float relativeScore = 1f;
        relativeScore = GroundClaimsService.GetInstance().GetRightTeamCoverageScore() / 100;
        yield return new WaitForSeconds(0.5f + ((15f * relativeScore) * 3f));
        Spawner.SpawnSoldier();
        Spawner.SpawnArcher();
        Spawner.SpawnWizard();
        SpawnRoutine = StartCoroutine(SpawnLoop());
    }

    public override void SetSpawner(UnitSpawner Spawner) {
        this.Spawner = Spawner;
    }
}
