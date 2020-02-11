/*  Author: Jonah Bui
	Purpose: To save and load player data.
	Date: January 14, 2020
*/
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    //Function: saves player data
	//Parameter: takes in a player in which to store data
	public static void SaveData(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/quick.dat";
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        stream.Position = 0;
        //Convert player data into components
        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }
	
	//Function: load player data
    public static PlayerData LoadData()
    {
        string path = Application.persistentDataPath + "/quick.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                stream.Position = 0;
                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                stream.Close();
                return data;
            }
        }
        else
        {
            Debug.Log("File not found");
            return null;
        }
    }
}
