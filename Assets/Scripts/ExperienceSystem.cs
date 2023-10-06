using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceSystem : MonoBehaviour
{
    public static int _xp;
    public static int _level;
    private List<int> _levelBrackets = new List<int> { 30, 80, 150, 250, 400 };

    public int XP {
        get { return _xp; }
        set {  _xp = value; }
    }

    public int Level {
        get { return _level; }
        set { _level = value; }
    }

    public int LevelBracket {
        get {
            return _levelBrackets[Unity.Mathematics.math.min(_level - 1, 3)];
        }
    }

    public void GainXP(int xpToGain) {
        if (xpToGain > 0) {
            _xp += xpToGain;
            _level = 3;


        }
    }

}
