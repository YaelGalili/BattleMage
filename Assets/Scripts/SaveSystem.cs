using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
    
    public static void SaveGame(int xp, int level, int lastSceneIndex, List<SpellCaster.Spell> abilitiesa) {
        Debug.Log("saving game");
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gameData.binary";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(xp, level, lastSceneIndex, abilitiesa);

        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadGame() {
        string path = Application.persistentDataPath + "/gameData.binary";
        if (File.Exists(path)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = binaryFormatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}