using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Saving : MonoBehaviour
{
    string password = "casdrASJAFj23";
    CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            save();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            load();
        }
    }
    public void save()
    {

        //string result = EncryptDecryptData("a");
        //Debug.Log(result);
        SaveData myData = new SaveData();
        myData.x = transform.position.x;
        myData.y = transform.position.y;
        myData.z = transform.position.z;
        string myDataString = JsonUtility.ToJson(myData);
        myDataString = EncryptDecryptData(myDataString);
        //Debug.Log(myDataString);
        string file = Application.persistentDataPath + "/" + gameObject.name + ".json";
        System.IO.File.WriteAllText(file, myDataString);
        //Debug.Log(file);
    }
    public void load()
    {
        string file = Application.persistentDataPath + "/" + gameObject.name + ".json";
        if (File.Exists(file))
        {
            string jsonData = File.ReadAllText(file);
            jsonData = EncryptDecryptData(jsonData);
            SaveData myData = JsonUtility.FromJson<SaveData>(jsonData);
            characterController.enabled = false;
            transform.position = new Vector3(myData.x, myData.y, myData.z);
            characterController.enabled = true;
        }
    }
    public string EncryptDecryptData(string data)
    {
        string result = "";
        for (int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ password[i % password.Length]);
        }
        return result;
    }
}

[System.Serializable]
public class SaveData
{
    public float x;
    public float y;
    public float z;

}