using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundClaimsService : MonoBehaviour
{
    [SerializeField]
    private GameObject GroundClaimsLeftTeam;
    [SerializeField]
    private GameObject GroundClaimsRightTeam;
    [SerializeField]
    private Text LeftTeamFillPc;
    [SerializeField]
    private Text RightTeamFillPc;

    public float LeftTeamClaimPos;
    public float RightTeamClaimPos;

    public bool TrackClaims = true;

    private static GroundClaimsService Instance;

    void Awake() {
        Instance = this;
        ResetCoverage();
    }

    public static GroundClaimsService GetInstance() {
        return Instance;
    }

    public void ResetCoverage() {
        LeftTeamFillPc.text = "0";
        RightTeamFillPc.text = "0";
        LeftTeamClaimPos = 0f;
        RightTeamClaimPos = 0f;
        GroundClaimsRightTeam.transform.localPosition = new Vector2(0, GroundClaimsRightTeam.transform.localPosition.y);
        GroundClaimsLeftTeam.transform.localPosition = new Vector2(0, GroundClaimsLeftTeam.transform.localPosition.y);
    }

    public float GetLeftTeamCoverageScore() {
        return ((LeftTeamClaimPos / 6.5f) * 100);
    }

    public float GetRightTeamCoverageScore() {
        return ((Mathf.Abs(RightTeamClaimPos) / 6.5f) * 100);
    }

    public void LeftTeamClaimUpdate(float toPos) {
        toPos += 3.25f;
        if(toPos > LeftTeamClaimPos) {
            LeftTeamClaimPos = toPos;
            if(TrackClaims) {
                GroundClaimsLeftTeam.transform.localPosition = new Vector3(
                    LeftTeamClaimPos,
                    GroundClaimsLeftTeam.transform.localPosition.y,
                    -1
                );
            }
            //update for overlap
            float cov = Mathf.Abs(RightTeamClaimPos) + LeftTeamClaimPos;
            if(cov > 6.5f) {
                RightTeamClaimPos = LeftTeamClaimPos - 6.5f;
                if(TrackClaims) {
                    GroundClaimsRightTeam.transform.localPosition = new Vector3(
                        RightTeamClaimPos,
                        GroundClaimsRightTeam.transform.localPosition.y,
                        -1
                    );
                }
            }
        }
        LeftTeamFillPc.text = GetLeftTeamCoverageScore().ToString("000");
        RightTeamFillPc.text = GetRightTeamCoverageScore().ToString("000");
    }

    public void RightTeamClaimUpdate(float toPos) {
        toPos -= 3.25f;
        if(toPos < RightTeamClaimPos) {
            RightTeamClaimPos = toPos;
            if(TrackClaims) {
                GroundClaimsRightTeam.transform.localPosition = new Vector3(
                    RightTeamClaimPos,
                    GroundClaimsRightTeam.transform.localPosition.y,
                    -1
                );
            }
            //update for overlap
            float cov = Mathf.Abs(RightTeamClaimPos) + LeftTeamClaimPos;
            if(cov > 6.5f) {
                LeftTeamClaimPos = 6.5f - Mathf.Abs(RightTeamClaimPos);
                if(TrackClaims) {
                    GroundClaimsLeftTeam.transform.localPosition = new Vector3(
                        LeftTeamClaimPos,
                        GroundClaimsLeftTeam.transform.localPosition.y,
                        -1
                    );
                }
            }
        }
        LeftTeamFillPc.text = GetLeftTeamCoverageScore().ToString("000");
        RightTeamFillPc.text = GetRightTeamCoverageScore().ToString("000");
    }
}
