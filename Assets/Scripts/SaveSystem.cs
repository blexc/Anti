using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    public static void SavePlayer(PlayerController player)
    {
        string path = Application.persistentDataPath + "/player.die";
        BinaryFormatter formatter = new BinaryFormatter();
        path = Application.persistentDataPath + "/player.die";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData pdata = new PlayerData(player);

        formatter.Serialize(stream, pdata);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.die";
        //Debug.Log("path " + path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            return data;
        }
        else
        {
            //Debug.LogError("Save file not found in " + path + " creating new player save");
            return null;
        }
    }

    public static void NewPlayer()
    {
        string path = Application.persistentDataPath + "/player.die";
        File.Delete(path);
    }

    public static bool IsNewPlayer()
    {
        string path = Application.persistentDataPath + "/player.die";
        return !File.Exists(path);
    }
}
