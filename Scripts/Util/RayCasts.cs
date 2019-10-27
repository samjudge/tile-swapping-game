using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RayCasts
{
    public static bool IsGroundTileBelowBy(Transform transform, float distance) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, distance);
        foreach(RaycastHit2D hit in hits) {
            GroundTile t = hit.transform.GetComponent<GroundTile>();
            if(t != null) {
                return true;
            }
        }
        return false;
    }

    public static KeyValuePair<Vector2,GroundTile> GetGroundTileRaycastHit(Transform transform, float distance) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, distance);
        foreach(RaycastHit2D hit in hits) {
            GroundTile t = hit.transform.GetComponent<GroundTile>();
            if(t != null) {
                return new KeyValuePair<Vector2, GroundTile>(hit.point, t);
            }
        }
        throw new Exception("No Tile Hit Point Found!");
    }
}