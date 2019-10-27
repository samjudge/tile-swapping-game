using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * @enum ArrowDirection
 * A enmerated list of cardinal NESW directions (tied to float values of the angle in degrees)
 */
public enum ArrowDirection { Right = 0, Up = 90, Left = 180, Down = 270, Undefined = -1 };

/**
 * @class ArrowDirectionUtil 
 * A utility for interacting with ArrowDirection
 */
public class ArrowDirectionUtil {

    /**
     * @method GetRelativeCardinalDirection 
     * @return ArrowDirection
     * @param Vector2 PointA the anchor position
     * @param Vector2 PointB the position to get the direction of, relative to PointA
     * return a arrow direction that fits the direction of PointA to PointB. Difference between points must be accurate.
     */
    public static ArrowDirection GetRelativeCardinalDirection(Vector2 PointA, Vector2 PointB) {
        float d = Mathf.Atan2(PointA.x - PointB.x, PointA.y - PointB.y) / Mathf.PI;
        d += 1;
        if (d == 0) {
            return ArrowDirection.Up;
        } else if (d == 0.5) {
            return ArrowDirection.Right;
        } else if (d == 1) {
            return ArrowDirection.Down;
        } else if (d == 1.5) {
            return ArrowDirection.Left;
        } else {
            return ArrowDirection.Undefined;
        }
    }
    /**
     * @method GetBestFitRelativeCardinalDirection 
     * @return ArrowDirection
     * @param Vector2 PointA the anchor position
     * @param Vector2 PointB the position to get the direction of, relative to PointA
     * return a arrow direction that best fits the direction of PointA to PointB.
     */
    public static ArrowDirection GetBestFitRelativeCardinalDirection(Vector2 PointA, Vector2 PointB) {
        float d = Mathf.Atan2(PointA.x - PointB.x, PointA.y - PointB.y) / Mathf.PI;
        d += 1;
        if (d >= 0.249999 && d <= 0.7499999) {
            return ArrowDirection.Up;
        } else if (d > 0.749999 && d <= 1.249999) {
            return ArrowDirection.Right;
        } else if (d > 1.249999 && d <= 1.749999) {
            return ArrowDirection.Down;
        } else if (d > 1.749999 || d < 0.249999) {
            return ArrowDirection.Left;
        } else {
            return ArrowDirection.Undefined;
        }
    }

    public static Vector2 ToVector(ArrowDirection Direction) {
        Vector2 output;
        switch (Direction) {
            case ArrowDirection.Up:
                output = Vector2.right;
                break;
            case ArrowDirection.Left:
                output = Vector2.up;
                break;
            case ArrowDirection.Down:
                output = Vector2.left;
                break;
            case ArrowDirection.Right:
                output = Vector2.down;
                break;
            default:
                output = Vector2.zero;
                break;
        }
        return output;
    }
}