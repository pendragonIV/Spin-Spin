using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private List<Level> levels = new List<Level>();

    public Level GetTheLevelAtGivenIndex(int index)
    {
        return levels[index];
    }

    public List<Level> GiveAllLevelAssigned()
    {
        return levels;
    }

    public void ReSetGivenLevelData(int levelIndex, bool isPlayable, bool isCompleted)
    {
        levels[levelIndex].isPlayable = isPlayable;
        levels[levelIndex].isCompleted = isCompleted;
    }

    #region Save and Load
    public void SaveThisDataToJsonFile()
    {
        string content = JsonHelper.ChangeArrayDataToJsonFomat(levels.ToArray(), true);
        WriteToFileCreated(content);
    }

    public void LoadDataFromJsonFileAndOverride()
    {
        string content = ReadDataFromCreatedFileInDevice();
        if (content != null)
        {
            List<Level> levelsData = new List<Level>(JsonHelper.ReadAndGetDataFromJson<Level>(content).ToList());
            for (int i = 0; i < levelsData.Count; i++)
            {
                levels[i].SetFixedDataForThisLevel(levelsData[i]);
            }
        }
    }

    private void WriteToFileCreated(string content)
    {
        FileStream file = new FileStream(Application.persistentDataPath + "/Levels.json", FileMode.Create);

        using (StreamWriter writer = new StreamWriter(file))
        {
            writer.Write(content);
        }
    }

    private string ReadDataFromCreatedFileInDevice()
    {
        if (File.Exists(Application.persistentDataPath + "/Levels.json"))
        {
            FileStream file = new FileStream(Application.persistentDataPath + "/Levels.json", FileMode.Open);

            using (StreamReader reader = new StreamReader(file))
            {
                return reader.ReadToEnd();
            }
        }
        else
        {
            return null;
        }
    }
    #endregion
}

[System.Serializable]
public class Level
{
    public GameObject map;
    public int moveLimit;
    public bool isCompleted;
    public bool isPlayable;

    public void SetFixedDataForThisLevel(Level levelData)
    {
        this.isCompleted = levelData.isCompleted;
        this.isPlayable = levelData.isPlayable;
    }
}

public static class JsonHelper
{
    public static T[] ReadAndGetDataFromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ChangeArrayDataToJsonFomat<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ChangeArrayDataToJsonFomat<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}


