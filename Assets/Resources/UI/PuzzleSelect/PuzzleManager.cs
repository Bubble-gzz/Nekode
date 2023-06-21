using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class PuzzleManager : MonoBehaviour
{
    // Start is called before the first frame update
    static PuzzleManager Instance;
    static public int calendarPage;
    static List<int> puzzleStar = new List<int>(); //-1 locked  0 newPuzzle 1~3 stars
    static public int puzzleCount = 31;
    static public int currentPuzzleID = 0;
    static public string saveName = "save1";
    static string saveNickname = "Fully Unlocked Save";
    
    [Serializable]
    public class PuzzleData{
        public List<int> puzzleStar;
        public string nickName;
        public PuzzleData(List<int> _puzzleStar, string _nickName)
        {
            puzzleStar = _puzzleStar;
            nickName = _nickName;
        }
    }
    PuzzleData puzzleData;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //LoadData();
            calendarPage = 0;
        }
        else {
            Destroy(gameObject);
        }
    }
    void InitSave(string nickName = "nickName")
    {
        puzzleStar.Clear();
        if (nickName == "Dr.Bubble")
        {
            for(int i = 0; i < puzzleCount; i++)
                puzzleStar.Add(UnityEngine.Random.Range(1, 4));
        }
        else
        {
            for(int i = 0; i < puzzleCount; i++)
                puzzleStar.Add(-1);
            puzzleStar[0] = 0;
        }
        saveNickname = nickName;
    }
    static public int PuzzleInfo(int id)
    {
        if (Instance == null) return -1;
        return puzzleStar[id];
    }
    static public void UpdatePuzzleInfo(int id, int starCount)
    {
        if (id < 0 || id >= puzzleCount) return;
        if (starCount > puzzleStar[id]) puzzleStar[id] = starCount;
        if (id < puzzleCount - 1) // Unlock new puzzle
        {
            if (puzzleStar[id + 1] == -1) puzzleStar[id + 1] = 0;
        }
        SaveData();
    }
    static public void SaveData()
    {
        string savePath = Application.dataPath + "/" + saveName + ".json";
        Debug.Log("puzzleStar: " + puzzleStar);
        PuzzleData data = new PuzzleData(puzzleStar, saveNickname);
        string data_text = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, data_text);
    }
    static public void LoadData()
    {
        LoadData(saveName);
    }
    static public void LoadData(string _saveName)
    {
        saveName = _saveName;
        string savePath = Application.dataPath + "/" + saveName + ".json";
        if (!File.Exists(savePath))
        {
            NewSave("");
        }
        else {
            string data_text = File.ReadAllText(savePath);
            PuzzleData data = JsonUtility.FromJson<PuzzleData>(data_text);
            puzzleStar = data.puzzleStar;
            saveNickname = data.nickName;
        }
    }
    static public PuzzleData GetData(string saveName)
    {
        string savePath = Application.dataPath + "/" + saveName + ".json";
        if (!File.Exists(savePath))
        {
            return null;
        }
        else {
            string data_text = File.ReadAllText(savePath);
            PuzzleData data = JsonUtility.FromJson<PuzzleData>(data_text);
            return data;
        }
    }
    static public void NewSave(string nickName)
    {
        Instance.InitSave(nickName);
        SaveData();
    }
}
