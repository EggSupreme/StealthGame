using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SavePlayer(FPSController player)
    {
        BinaryFormatter formatter1 = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.sav";
        FileStream stream1 = new FileStream(path, FileMode.Create);

        LocationData data1 = new LocationData(player);

        formatter1.Serialize(stream1, data1);
        stream1.Close();
    }

    public static void SaveTools(Tools_Abilities toolbelt)
    {
        BinaryFormatter formatter2 = new BinaryFormatter();

        string path = Application.persistentDataPath + "/tools.sav";
        FileStream stream2 = new FileStream(path, FileMode.Create);

        ToolData data2 = new ToolData(toolbelt);

        formatter2.Serialize(stream2, data2);
        stream2.Close();
    }

    public static LocationData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.sav";
        if (File.Exists(path))
        {
            BinaryFormatter formatter1 = new BinaryFormatter();
            FileStream stream3 = new FileStream(path, FileMode.Open);

            LocationData data1 = formatter1.Deserialize(stream3) as LocationData;

            stream3.Close();

            return data1;

            
        }

        else 
        {
            Debug.LogError("Data for Player not found in " + path);
            return null;
        }

    }

    public static ToolData LoadTools()
    {
        string path = Application.persistentDataPath + "/tools.sav";
        if (File.Exists(path))
        {
            BinaryFormatter formatter2 = new BinaryFormatter();
            FileStream stream4 = new FileStream(path, FileMode.Open);

            ToolData data2 = formatter2.Deserialize(stream4) as ToolData;

            stream4.Close();

            return data2;
        }

        else
        {
            Debug.LogError("Data for Tools not found in " + path);
            return null;
        }

    }

}
