using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Lvl6ScriptedSpawner : ScriptedSpawner
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

    private float SpawnTime = 4f;

    private IEnumerator SpawnLoop() {
        float relativeScore = (360 - LevelManagerService.GetInstance().GetTimeRemaining()) / 360;
        if(LevelManagerService.GetInstance().GetTimeRemaining() < 300) {
            if(SpawnTime > 2f) {
                LevelManagerService.GetInstance().SplashLabel.text = "Speeding Up!";
                yield return new WaitForSeconds(3);
                LevelManagerService.GetInstance().SplashLabel.text = "";
                SpawnTime = 2f;
            }
        }
        if(LevelManagerService.GetInstance().GetTimeRemaining() < 240) {
            if(SpawnTime > 1.5f) {
                LevelManagerService.GetInstance().SplashLabel.text = "Speeding Up!!";
                yield return new WaitForSeconds(3);
                LevelManagerService.GetInstance().SplashLabel.text = "";
                SpawnTime = 1.5f;
            }
        }
        if(LevelManagerService.GetInstance().GetTimeRemaining() < 180) {
            if(SpawnTime > 1.5f) {
                LevelManagerService.GetInstance().SplashLabel.text = "Speeding Up!!!";
                yield return new WaitForSeconds(3);
                LevelManagerService.GetInstance().SplashLabel.text = "";
                SpawnTime = 1.5f;
            }
        }
        if(LevelManagerService.GetInstance().GetTimeRemaining() < 60) {
            if(SpawnTime > 1f) {
                LevelManagerService.GetInstance().SplashLabel.text = "Speeding Up!!!!";
                yield return new WaitForSeconds(3);
                LevelManagerService.GetInstance().SplashLabel.text = "";
                SpawnTime = 1f;
            }
        }
        if(LevelManagerService.GetInstance().GetTimeRemaining() < 60) {
            if(SpawnTime > 0.75f) {
                LevelManagerService.GetInstance().SplashLabel.text = "Speeding Up!!!!!";
                yield return new WaitForSeconds(3);
                LevelManagerService.GetInstance().SplashLabel.text = "";
                SpawnTime = 0.75f;
            }
        }
        Spawner.SpawnSoldier();
        yield return new WaitForSeconds(SpawnTime);
        Spawner.SpawnArcher();
        yield return new WaitForSeconds(SpawnTime);
        Spawner.SpawnWizard();
        yield return new WaitForSeconds(SpawnTime);
        SpawnRoutine = StartCoroutine(SpawnLoop());
    }

    public override void SetSpawner(UnitSpawner Spawner) {
        this.Spawner = Spawner;
    }
}
