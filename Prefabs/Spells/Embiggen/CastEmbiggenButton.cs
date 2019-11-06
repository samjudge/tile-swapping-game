using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastEmbiggenButton : MonoBehaviour
{
    [SerializeField]
    private Image ButtonImage;
    [SerializeField]
    private Button Interactable;
    [SerializeField]
    private UnitSpawner PlayerSpawner;

    private bool IsAiming = false;

    void Update(){
        if(!CanCast()) {
            ButtonImage.color = new Color(0.5f,0.5f,0.5f);
            Interactable.interactable = false;
        } else {
            ButtonImage.color = new Color(1f,0f,0f);
            Interactable.interactable = true;
        }
    }

    public void CastEmbiggen() {
        if(CanCast()){
            LevelManagerService.GetInstance().CurrentPlayerMana -= 3;
            List<GameObject> units = PlayerSpawner.GetAllSpawnedUnits();
            foreach(GameObject u in units) {
                if(u!= null) {
                    StartCoroutine(SpellEffectsService.GetInstance().EmbiggenEffect(u));
                }
            }
        }
    }

    private bool CanCast() {
        return LevelManagerService.GetInstance().CurrentPlayerMana >= 3;
    }
}
