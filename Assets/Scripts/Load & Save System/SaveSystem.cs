using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/User.settings";

    /// <summary>
    /// Saves the PlayerSettings
    /// </summary>
    public static void SaveSettings()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, UserData.singleton);
        stream.Close();
    }

    /// <summary>
    /// Loads the player settings
    /// </summary>
    /// <returns>UserData object</returns>
    public static UserData LoadUserData()
    {
        if (File.Exists(path) && new FileInfo(path).Length != 0)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            UserData data = (UserData)formatter.Deserialize(stream);
            stream.Close();

            return data;
        }
        else Debug.Log("Save file not found in " + path);

        return null;
    }

}
