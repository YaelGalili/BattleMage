using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadGameManager : MonoBehaviour
{

    public void Save() {
        GameObject player = GameObject.Find("Player");
        SpellCaster spellCaster = player.GetComponent<SpellCaster>();
        ExperienceSystem expSystem = player.GetComponent<ExperienceSystem>();
        SaveSystem.SaveGame(expSystem.XP, expSystem.Level, SceneManager.GetActiveScene().buildIndex, spellCaster.chosenSpells);
    }

    public static void Load() {
        SaveData data = SaveSystem.LoadGame();

        GameObject player = GameObject.Find("Player");
        ExperienceSystem expSystem = player.GetComponent<ExperienceSystem>();
        SpellCaster spellCaster = player.GetComponent<SpellCaster>();

        expSystem.Level = data.level;
        expSystem.XP = data.xp;

        spellCaster.LoadSpellsFomData(data.abilities);
    }
}
