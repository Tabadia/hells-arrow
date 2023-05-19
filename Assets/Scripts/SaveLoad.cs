using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoad
{
    public static void SaveData(GameObject samurai, GameObject shrine, GameObject enemyManager, GameObject scoreText)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/blackicemachine.thequeenisdead";

        FileStream stream = new FileStream(path, FileMode.Create);

        GameData charData = new GameData(samurai, shrine, enemyManager, scoreText);

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