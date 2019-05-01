using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FormController : MonoBehaviour
{



    public InputField inpNombre ;
    public InputField inpTelefono;
    public InputField inpMail;
    public InputField inpEmpresa; 

    public GameObject alertWindows;
    public GameObject exitWindow;
    public GameObject holdWindow;

    public List<InputField> requiredFields = new List<InputField>();  
    public List<Customer> customers = new List<Customer>();

    private void Start()
    {
        LoadData();
    }

    public void RegisterButton()
    {
        bool isOk = true;
        foreach (var item in requiredFields)
        {
            item.placeholder.color = Color.gray ;
            if (string.IsNullOrEmpty(item.text))
            {
                item.placeholder.color = Color.red; 
                isOk = false;
            }
        }

        if (isOk)     
        {
            Customer client = new Customer();
            client.name =  inpNombre.text ;
            client.telefono = inpTelefono.text;
            client.mail = inpMail.text;
            client.empresa = inpEmpresa.text;
            client.date = DateTime.Now.ToString("dd/MM/yyyy-hh:mm");
            customers.Add(client);
             foreach (var item in requiredFields)
            {
                item.placeholder.color = Color.gray;
                item.text = string.Empty;
            }
            SaveAndSyncData();
            holdWindow.SetActive(true);
        }
         else
        {
            alertWindows.SetActive(true); 
         }
     }

     public void SaveAndSyncData()
    {
        if (customers.Count > 0)
        {
            Customer [] castList = customers.ToArray();
            string json = JsonHelper.ToJson(castList, true);
            PlayerPrefs.SetString("PlayersData", json);
            
        }
        else
        {
            string json = PlayerPrefs.GetString("PlayersData");

            if (json != "")
            {
                Customer[] castList = JsonHelper.FromJson<Customer>(json);
                customers = new List<Customer>(castList);
            }
            
        }
    }
    
    public void LoadData ()
    {
        string json = PlayerPrefs.GetString("PlayersData");

        if (json != "")
        {
            Customer[] castList = JsonHelper.FromJson<Customer>(json);
            customers = new List<Customer>(castList);
        }
    }

    public void ShowAppPath()
    {
        string filePath = Application.persistentDataPath + "/DB.DATA";
        File.WriteAllLines(filePath, new [] { PlayerPrefs.GetString("PlayersData") });
        exitWindow.SetActive(true);
        Application.Quit();
    }

}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }


}

[System.Serializable]
public class Customer        
{
    public string name;
    public string telefono;
    public string mail;
    public string empresa;
    public string date;
}  
