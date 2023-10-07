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
            return _levelBrackets[Unity.Mathematics.math.max(Level - 1, 0)];
        }
    }

    public void GainXP(int xpToGain) {
        Debug.Log("Gained " + xpToGain + "xp");
        if (xpToGain > 0) {
            XP += xpToGain;
            while(XP > LevelBracket && Level < 5) {
                Level += 1;
                Debug.Log("Level up!");
            }
        }


    }

    private void FixedUpdate() {
        //Debug.Log(Level);
    }

}
