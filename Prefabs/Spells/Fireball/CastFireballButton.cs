using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastFireballButton : MonoBehaviour
{
    [SerializeField]
    private Fireball FireballPrefab;
    [SerializeField]
    private UnitSpawner OpponentSpawner;
    [SerializeField]
    private UnitSpawner PlayerSpawner;
    [SerializeField]
    private Transform Battlefield;
    [SerializeField]
    private Image ButtonImage;
    [SerializeField]
    private Button Interactable;

    private bool IsAiming = false;

    public void PrepareFireball() {
        if(!IsAiming) {
            IsAiming = true;
            LevelManagerService.GetInstance().CurrentPlayerMana -= 3;
        }
    }

    void Update(){
        if(!CanCast()) {
            ButtonImage.color = new Color(0.5f,0.5f,0.5f);
            Interactable.interactable = false;
        } else {
            ButtonImage.color = new Color(1f,0f,0f);
            Interactable.interactable = true;
        }
        if(IsAiming) {
            if(Input.GetKeyDown(KeyCode.Mouse0)) {
                Vector3 castTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CastFireball(castTo);
                IsAiming = false;
            }
        }
    }

    public void CastFireball(Vector3 to) {
        Fireball fireball = Instantiate(FireballPrefab);
        fireball.transform.SetParent(Battlefield);
        Vector2 diff = to - PlayerSpawner.transform.position;
        fireball.Body.AddForce(
            diff / 2,
            ForceMode2D.Impulse
        );
        fireball.gameObject.layer = LayerMask.NameToLayer("PlayerUnits");
        fireball.transform.position = new Vector3(
            PlayerSpawner.transform.position.x,
            5f,
            PlayerSpawner.transform.position.z
        );
    }

    private bool CanCast() {
        return LevelManagerService.GetInstance().CurrentPlayerMana >= 3;
    }
}
