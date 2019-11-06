using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellEffectsService : MonoBehaviour
{
    private static SpellEffectsService Instance;

    public static SpellEffectsService GetInstance() {
        return Instance;
    }

    public void Awake() {
        Instance = this;
    }

    public IEnumerator EmbiggenEffect(GameObject Unit) {
        float timer = 1f;
        float cTimer = 0f;
        Vector3 initalUnitScale = Unit.transform.localScale;
        Vector3 finalUnitScale = new Vector3(
            initalUnitScale.x * 1.25f,
            initalUnitScale.y * 1.25f,
            initalUnitScale.y * 1.25f
        );
        while(cTimer < timer) {
            if(Unit != null){
                Unit.transform.localScale = Vector3.Lerp(
                    initalUnitScale,
                    finalUnitScale,
                    cTimer / timer
                );
            }
            cTimer += Time.deltaTime;
            yield return null;
        }
        if(Unit != null){
            Unit.transform.localScale = finalUnitScale;
            JumpingUnit ju = Unit.GetComponent<JumpingUnit>();
            if(ju != null) {
                ju.GroundCheckDistance = ju.GroundCheckDistance * 1.25f;
                ju.Body.mass = ju.Body.mass * 1.05f;
                ju.JumpForce = new Vector2(
                    ju.JumpForce.x,
                    ju.JumpForce.y * 1.2f
                );
            }
            DamagableUnit du = Unit.GetComponent<DamagableUnit>();
            if(du != null) {
                du.Hp += 10;
            }
            WizardBehaviour wb = Unit.GetComponent<WizardBehaviour>();
            if(wb != null) {
                wb.FireballSpellScale = finalUnitScale;
            }
        }
    }

    public void FreezeEffect(GameObject Unit, float forTime) {
        FreezableUnit fu = Unit.GetComponent<FreezableUnit>();
        if(fu != null) {
            fu.FreezeFor(forTime);
        }
    }
}
