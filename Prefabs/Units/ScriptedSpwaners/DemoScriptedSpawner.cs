using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DemoScriptedSpawner : ScriptedSpawner
{
    [SerializeField]
    private UnitSpawner Spawner;

    private Coroutine SpawnRoutine;

    public Team Owner;

    void Start() {
    }

    public override UnitSpawner GetSpawner() {
        return Spawner;
    }

    public override void StartSpawning() {
        SpawnRoutine = StartCoroutine(SpawnLoop());
    }

    public override void StopSpawning() {
        StopCoroutine(SpawnRoutine);
    }

    private IEnumerator SpawnLoop() {
        float relativeScore = 1f;
        if(Owner == Team.Left) {
            relativeScore = GroundClaimsService.GetInstance().GetLeftTeamCoverageScore() / 100;
        } else {
            relativeScore = GroundClaimsService.GetInstance().GetRightTeamCoverageScore() / 100;
        }
        Spawner.SpawnSoldier();
        yield return new WaitForSeconds(10f * relativeScore + 1);
        Spawner.SpawnArcher();
        yield return new WaitForSeconds(10f * relativeScore + 1);
        Spawner.SpawnWizard();
        yield return new WaitForSeconds(10f * relativeScore + 1);
        SpawnRoutine = StartCoroutine(SpawnLoop());
    }

    public override void SetSpawner(UnitSpawner Spawner)
    {
        this.Spawner = Spawner;
    }
}
