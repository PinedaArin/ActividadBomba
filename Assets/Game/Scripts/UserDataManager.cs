using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class UserDataManager : MonoBehaviour
{
    public List<UserInformation> dataBase = new List<UserInformation>();

    public InputField name;
    public InputField lastName;
    public InputField EMail;
    
    public void saveData()
    {
        UserInformation userInformation = new UserInformation();
        userInformation.name = name.text;
        userInformation.lastName = lastName.text;
        userInformation.EMail = EMail.text;
        dataBase.Add(userInformation);
        SaveAndSyncData();
    }

    public void SaveAndSyncData()
    {
        if (dataBase.Count > 0)
        {
            UserInformation[] castList = dataBase.ToArray();
            string json = JsonHelper.ToJson(castList, true);
            PlayerPrefs.SetString("PlayersData", json);

        }
        else
        {
            string json = PlayerPrefs.GetString("PlayersData");

            if (json != "")
            {
                UserInformation[] castList = JsonHelper.FromJson<UserInformation>(json);
                dataBase = new List<UserInformation>(castList);
            }

        }
        SaveFile();
    }

    public void LoadData()
    {
        string json = PlayerPrefs.GetString("PlayersData");

        if (json != "")
        {
            UserInformation[] castList = JsonHelper.FromJson<UserInformation>(json);
            dataBase = new List<UserInformation>(castList);
        }
    }

    public void SaveFile()
    {
        string filePath = Application.persistentDataPath + "/DB.DATA";
        File.WriteAllLines(filePath, new[] { PlayerPrefs.GetString("PlayersData") });
        Debug.Log("File saving: " + filePath);
    }

   
    void Start()
    {            
    }

    void Awake()
    {
        LoadData();

    }
}

[System.Serializable]
public struct UserInformation{
    public string name;
    public string lastName;
    public string EMail;


    }
