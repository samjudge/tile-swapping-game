using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class JumpingUnit : MonoBehaviour
{
    [SerializeField]
    public Rigidbody2D Body;
    private bool IsMidJump = false;
    [SerializeField]
    public Vector3 JumpDirection;
    [SerializeField]
    public GroundClaimsService GroundClaimsService;

    public virtual void Update() {
        if(IsMidJump && IsDecending()) {
            IsMidJump = false;
        }
        if(RayCasts.IsGroundTileBelowBy(transform, 0.3f) && !IsMidJump) {
            if(JumpDirection.x >= 0) {
                GroundClaimsService.LeftTeamClaimUpdate(transform.localPosition.x);
            } else {
                GroundClaimsService.RightTeamClaimUpdate(transform.localPosition.x);
            }
            Jump();
        }
        //put the breaks on if moving too fast
        if(Mathf.Abs(Body.velocity.x) > 0.1f){
            Body.AddForce(-JumpDirection.normalized * 0.1f);
        }
    }

    protected bool IsDecending() {
        if(Body.velocity.y < 0) {
            return true;
        }
        return false;
    }

    protected void Jump() {
        IsMidJump = true;
        Body.AddForce(Vector2.up * 0.5f, ForceMode2D.Impulse);
        if(Mathf.Abs(Body.velocity.x) < 0.1f){
            Body.AddForce(JumpDirection.normalized * 0.1f, ForceMode2D.Impulse);
        }
    }
}
