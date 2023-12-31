﻿using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    // Makes it a singleton / single instance
    static public SaveSystem instance;
    string filePath;

    private void Awake()
    {
        // Check there are no other instances of this class in the scene
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        filePath = Application.persistentDataPath + "/save.data";

    }
    public void CreateGame(GameData saveData)
    {
        if (File.Exists(filePath))
        {
            // ถ้าไฟล์มีอยู่แล้ว
            Debug.LogWarning("File already exists at path: " + filePath);

            // ทำอย่างอื่นตามที่คุณต้องการ, เช่น การโหลดข้อมูล, แจ้งเตือนผู้ใช้, หรืออื่น ๆ
        }
        else
        {
            // ถ้าไฟล์ไม่มีอยู่
            FileStream dataStream = new FileStream(filePath, FileMode.Create);

            BinaryFormatter converter = new BinaryFormatter();
            converter.Serialize(dataStream, saveData);

            dataStream.Close();
        }
    }
    public void SaveGame(GameData saveData)
    {
        FileStream dataStream = new FileStream(filePath, FileMode.Create);

        BinaryFormatter converter = new BinaryFormatter();
        converter.Serialize(dataStream, saveData);

        dataStream.Close();
    }

    public GameData LoadGame()
    {
        if (File.Exists(filePath))
        {
            // File exists 
            FileStream dataStream = new FileStream(filePath, FileMode.Open);

            BinaryFormatter converter = new BinaryFormatter();
            GameData saveData = converter.Deserialize(dataStream) as GameData;

            dataStream.Close();
            return saveData;
        }
        else
        {
            // File does not exist
            Debug.LogError("Save file not found in " + filePath);
            return null;
        }
    }
}