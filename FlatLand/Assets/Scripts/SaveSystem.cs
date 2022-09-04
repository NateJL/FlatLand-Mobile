using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    ///<summary>
    /// Return true if there is an instance of a game save on the device, otherwise returns false.
    /// </summary>
    public static bool CheckForPlayerData()
    {
        if (PlayerPrefs.HasKey("PlayerSave"))
        {
            if (PlayerPrefs.GetInt("PlayerSave") == 1)
                return true;
            else
                return false;
        }
        else
        {
            PlayerPrefs.SetInt("PlayerSave", 0);
            return false;
        }
    }

    ///<summary>
    /// Returns a playerData gameObject filled with data from the found save file.
    /// </summary>
    public static PlayerData LoadPlayerData()
    {
        PlayerData playerData = new PlayerData();

        playerData.playerName = PlayerPrefs.GetString("PlayerName");
        playerData.health = PlayerPrefs.GetInt("Health");
        playerData.speed = PlayerPrefs.GetFloat("Speed");
        playerData.edgeCount = PlayerPrefs.GetInt("EdgeCount");
        playerData.points = PlayerPrefs.GetInt("Points");

        playerData.playerPosition = new Vector3(PlayerPrefs.GetFloat("X"), 
                                                PlayerPrefs.GetFloat("Y"),  
                                                PlayerPrefs.GetFloat("Z"));
        playerData.polygonScalar = PlayerPrefs.GetFloat("PolygonScalar");

        return playerData;
    }

    ///<summary>
    /// Saves the given playerData object values to the playerprefs directory.
    /// </summary>
    public static void SavePlayerData(PlayerData playerData)
    {
        PlayerPrefs.SetString("PlayerName", playerData.playerName);
        PlayerPrefs.SetInt("Health", playerData.health);
        PlayerPrefs.SetFloat("Speed", playerData.speed);
        PlayerPrefs.SetInt("EdgeCount", playerData.edgeCount);
        PlayerPrefs.SetInt("Points", playerData.points);

        PlayerPrefs.SetFloat("X", playerData.playerPosition.x);
        PlayerPrefs.SetFloat("Y", playerData.playerPosition.y);
        PlayerPrefs.SetFloat("Z", playerData.playerPosition.z);

        PlayerPrefs.SetFloat("PolygonScalar", playerData.polygonScalar);

        PlayerPrefs.SetInt("PlayerSave", 1);
    }
}
