using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveLoad
{
    public static void SaveData(GameObject samurai, GameObject shrine, GameObject enemyManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        Debug.Log(Application.persistentDataPath);
        string path = Application.persistentDataPath + "/blackicemachine.thequeenisdead";

        FileStream stream = new FileStream(path, FileMode.Create);

        GameData charData = new GameData(samurai, shrine, enemyManager);

        formatter.Serialize(stream, charData);
        stream.Close();
    }

    public static GameData LoadData()
    {
        string path = Application.persistentDataPath + "/blackicemachine.thequeenisdead";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Error: Save file not found in " + path);
            return null;
        }
    }
}