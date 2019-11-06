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
    public bool AllowMovement = true;
    public float GroundCheckDistance = 0.3f;
    public Vector2 JumpForce = new Vector2(0.1f, 0.5f);

    public virtual void Update() {
        if(IsMidJump && IsDecending()) {
            IsMidJump = false;
        }
        if(RayCasts.IsGroundTileBelowBy(transform, GroundCheckDistance) && !IsMidJump) {
            if(JumpDirection.x >= 0) {
                GroundClaimsService.GetInstance().LeftTeamClaimUpdate(transform.localPosition.x);
            } else {
                GroundClaimsService.GetInstance().RightTeamClaimUpdate(transform.localPosition.x);
            }
            if(AllowMovement) Jump();
        }
        //put the breaks on if moving too fast
        if(Mathf.Abs(Body.velocity.x) > JumpForce.x){
            if(JumpDirection.normalized.x > 0){
                if(Body.velocity.x > 0) {
                    //slow down
                    Body.AddForce(-JumpDirection.normalized * JumpForce.x);
                } else {
                    //speed up
                    Body.AddForce(JumpDirection.normalized * JumpForce.x);
                }
            } else {
                if(Body.velocity.x < 0) {
                    //slow down
                    Body.AddForce(-JumpDirection.normalized * JumpForce.x);
                } else {
                    //speed up
                    Body.AddForce(JumpDirection.normalized * JumpForce.x);
                }
            }
            
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
        Body.AddForce(Vector2.up * JumpForce.y, ForceMode2D.Impulse);
        if(Mathf.Abs(Body.velocity.x) < JumpForce.x){
            Body.AddForce(JumpDirection.normalized * JumpForce.x, ForceMode2D.Impulse);
        }
    }
}
