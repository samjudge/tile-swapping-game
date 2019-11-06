using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastFreezeButton : MonoBehaviour
{
    [SerializeField]
    private UnitSpawner OpponentSpawner;
    [SerializeField]
    private Image ButtonImage;
    [SerializeField]
    private Button Interactable;

    private bool IsAiming = false;

    void Update() {
        if(!CanCast()) {
            ButtonImage.color = new Color(0.5f,0.5f,0.5f);
            Interactable.interactable = false;
        } else {
            ButtonImage.color = new Color(0.5f,0.5f,1f);
            Interactable.interactable = true;
        }
    }

    public void CastFreeze() {
        if(CanCast()) {
            OpponentSpawner.LockSpawner();
            LevelManagerService.GetInstance().CurrentPlayerMana -= 3;
            StartCoroutine(UnlockSpawnerAfter(10f));
            List<GameObject> units = OpponentSpawner.GetAllSpawnedUnits();
            foreach(GameObject u in units) {
                if(u != null) {
                    SpellEffectsService.GetInstance().FreezeEffect(u, 10f);
                }
            }
        }
    }

    private IEnumerator UnlockSpawnerAfter(float t) {
        yield return new WaitForSeconds(t);
        OpponentSpawner.UnlockSpawner();
    }

    private bool CanCast() {
        return LevelManagerService.GetInstance().CurrentPlayerMana >= 3;
    }
}
