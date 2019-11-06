using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LevelManagerService : MonoBehaviour
{
    [SerializeField]
    public float LevelTimeRemaining = 120;
    private float CurrentLevelTimeRemaining;
    public float CurrentPlayerMana = 0f; //0 -> 3
    private static LevelManagerService Instance;
    [SerializeField]
    private Text TimerLabel;
    [SerializeField]
    public Text SplashLabel;
    private ScriptedSpawner PlayerScriptedSpawner;
    private ScriptedSpawner OpponentScriptedSpawner;
    [SerializeField]
    private UnitSpawner PlayerSpawner;
    [SerializeField]
    private UnitSpawner OpponentSpawner;
    public List<String> CompletedChallenges;
    private String LoadedChallenge;
    [SerializeField]
    private Image ManaBar;

    void Awake() {
        Instance = this;
        CompletedChallenges = new List<String>();
    }

    void Start() {
        //start in demo mode
        LoadDemoMode();
    }

    public static LevelManagerService GetInstance() {
        return Instance;
    }

    void Update(){
        CurrentPlayerMana += Time.deltaTime / 10f;
        if(CurrentPlayerMana >= 3) CurrentPlayerMana = 3;
        ManaBar.fillAmount = CurrentPlayerMana / 3f;
    }

    public void LoadDemoMode(){
        //assume demo mode here for now
        GroundClaimsService.GetInstance().ResetCoverage();
        GroundClaimsService.GetInstance().TrackClaims = false;
        var Spawner = PlayerSpawner.gameObject.AddComponent<DemoScriptedSpawner>();
        Spawner.Owner = Team.Left;
        PlayerScriptedSpawner = Spawner;
        PlayerScriptedSpawner.SetSpawner(PlayerSpawner);
        PlayerScriptedSpawner.StartSpawning();
        Spawner = OpponentSpawner.gameObject.AddComponent<DemoScriptedSpawner>();
        Spawner.Owner = Team.Right;
        OpponentScriptedSpawner = Spawner;
        OpponentScriptedSpawner.SetSpawner(OpponentSpawner);
        OpponentScriptedSpawner.StartSpawning();
    }

    public void SetTimeRemaining(float t) {
        CurrentLevelTimeRemaining = t;
    }

    public float GetTimeRemaining() {
        return CurrentLevelTimeRemaining;
    }

    public void LoadLevel(string SpawnerName) { 
        LoadedChallenge = SpawnerName;
        Destroy(OpponentScriptedSpawner);
        Destroy(PlayerScriptedSpawner);
        CurrentLevelTimeRemaining = LevelTimeRemaining;
        CurrentPlayerMana = 0f;
        Type spawner = Type.GetType(SpawnerName);
        OpponentScriptedSpawner =
            (ScriptedSpawner) OpponentSpawner.gameObject.AddComponent(spawner);
        OpponentScriptedSpawner.SetSpawner(OpponentSpawner);
        GroundClaimsService.GetInstance().ResetCoverage();
        StartCoroutine(LevelStartCountdown());
    }

    private IEnumerator LevelStartCountdown(){
        EndLevel();
        PlayerSpawner.UnlockSpawner();
        OpponentSpawner.UnlockSpawner();
        UIManagerService.GetInstance().ShowTilesContainer();
        SplashLabel.color = new Color(1f,1f,1f);
        TimerLabel.gameObject.SetActive(true);
        SplashLabel.gameObject.SetActive(true);
        SplashLabel.text = "Level Starting In...\n3";
        yield return new WaitForSeconds(1);
        SplashLabel.text = "Level Starting In...\n2";
        yield return new WaitForSeconds(1);
        SplashLabel.text = "Level Starting In...\n1";
        yield return new WaitForSeconds(1);
        SplashLabel.text = "GO!";
        TileManagerService.GetInstance().InitTiles();
        TileManagerService.GetInstance().IsLocked = false;
        GroundClaimsService.GetInstance().TrackClaims = true;
        OpponentScriptedSpawner.StartSpawning();
        StartCoroutine(UpdateTimer());
        yield return new WaitForSeconds(1);
        SplashLabel.text = "";
    }

    private IEnumerator UpdateTimer() {
        string timerMins = Mathf.Floor(CurrentLevelTimeRemaining / 60).ToString("00");
        string timerSeconds = (CurrentLevelTimeRemaining % 60).ToString("00");
        TimerLabel.text = timerMins + ":" + timerSeconds;
        if(IsLevelOver()) {
            OpponentScriptedSpawner.StopSpawning();
            Team Winner = GetWinningTeam();
            switch(Winner) {
                case Team.Left:
                    SplashLabel.color = new Color(0,0,1f);
                    SplashLabel.text = "You Win!";
                    CompletedChallenges.Add(LoadedChallenge);
                    UIManagerService.GetInstance().MarkLevelAsComplete(LoadedChallenge);
                    break;
                case Team.Right:
                    SplashLabel.color = new Color(1f,0,0);
                    SplashLabel.text = "You Lose...";
                    break;
                default:
                    SplashLabel.color = new Color(0,0,0);
                    SplashLabel.text = "Draw";
                    break;
            }
            EndLevel();
            yield return new WaitForSeconds(3);
            TimerLabel.gameObject.SetActive(false);
            SplashLabel.gameObject.SetActive(false);
            TileManagerService.GetInstance().RemoveCurrentTiles();
            GroundClaimsService.GetInstance().ResetCoverage();
            UIManagerService.GetInstance().ShowChallengeMenu();
            LoadDemoMode();
        } else {
            yield return new WaitForSeconds(1);
            CurrentLevelTimeRemaining--;
            StartCoroutine(UpdateTimer());
        }
    }

    private void EndLevel() {
        TileManagerService.GetInstance().IsLocked = true;
        PlayerSpawner.LockSpawner();
        PlayerSpawner.RemoveAllSpawnedUnits();
        OpponentSpawner.LockSpawner();
        OpponentSpawner.RemoveAllSpawnedUnits();
    }

    private Team GetWinningTeam(){
        float ltcs = GroundClaimsService.GetInstance().GetLeftTeamCoverageScore();
        float rtcs = GroundClaimsService.GetInstance().GetRightTeamCoverageScore();
        if(ltcs > rtcs) {
            return Team.Left;
        } else if (ltcs < rtcs) {
            return Team.Right;
        } else {
            return Team.None;
        }
    }

    private bool IsLevelOver(){
        float ltcs = GroundClaimsService.GetInstance().GetLeftTeamCoverageScore();
        float rtcs = GroundClaimsService.GetInstance().GetRightTeamCoverageScore();
        if(ltcs >= 100 || rtcs >= 100) {
            return true;
        }
        if(CurrentLevelTimeRemaining == 0) return true;
        return false;
    }

    public void CloseGame(){
        Application.Quit();
    }
}
