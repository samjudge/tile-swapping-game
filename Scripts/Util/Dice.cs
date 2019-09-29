using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Utilies for vector manipulation
 */
public class Dice {

    private static System.Random Die;

    static Dice() {
        Dice.Die = new System.Random();
    }

    /**
     * Limits the component values of In to the component values of Max
     */
    public static int Roll(int min, int max){
        return Die.Next(min, max);
    }

    public static float Roll(float min, float max){
        return (float) Die.NextDouble() * (max - min) + min;
    }
}
