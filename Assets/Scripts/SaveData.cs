using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {
    public int xp;
    public int level;
    public int lastSceneIndex;
    public int[] abilities;

    public SaveData(int xp, int level, int lastSceneIndex, List<SpellCaster.Spell> abilities) {
        this.xp = xp;
        this.level = level;
        this.lastSceneIndex = lastSceneIndex;

        this.abilities = new int[4];

        // save ability 2
        // if (abilities[0].e == "LightningStorm")
        if (abilities.Count > 0 && abilities[0] == SpellCaster.Spell.LightningStorm)
        this.abilities[0] = 0;
        else
            this.abilities[0] = 1;

        // save ability 3
        // if (abilities[0].name == "EarthenSpike")
        if (abilities.Count > 1 && abilities[1] == SpellCaster.Spell.EarthenSpike)
            this.abilities[1] = 0;
        else
            this.abilities[1] = 1;

        // save ability 4
        // if (abilities[0].name == "ArcaneBlast")
        if (abilities.Count > 2 && abilities[2] == SpellCaster.Spell.ArcaneBlast)
            this.abilities[2] = 0;
        else
            this.abilities[2] = 1;

        // save ability 5
        //if (abilities[0].name == "Phoenix")
        if (abilities.Count > 3 && abilities[3] == SpellCaster.Spell.Phoenix)
            this.abilities[3] = 0;
        else
            this.abilities[3] = 1;
    }


}