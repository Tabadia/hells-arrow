using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveLoad
{
    public static void SaveData(GameObject samurai)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        Debug.Log(Application.persistentDataPath);
        string path = Application.persistentDataPath + "/myhearthurtsman.shesaidforever";

        FileStream stream = new FileStream(path, FileMode.Create);

        SamuraiData charData = new SamuraiData(samurai);

        formatter.Serialize(stream, charData);
        stream.Close();
    }

    public static SamuraiData LoadData()
    {
        string path = Application.persistentDataPath + "/myhearthurtsman.shesaidforever";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SamuraiData data = formatter.Deserialize(stream) as SamuraiData;

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