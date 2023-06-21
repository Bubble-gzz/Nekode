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
            UnlockPuzzle(id + 1); // if (puzzleStar[id + 1] == -1) puzzleStar[id + 1] = 0;
        }
        
        BeKindtoPlayers(id);
        
        SaveData();
    }
    static void BeKindtoPlayers(int id)
    {
        /*
                    ------6-7..
                   /
            0-1-2-3-4-5
        */
        if (id == 3) UnlockPuzzle(6);

        /* 
              8--------
             / \       \
          6-7   10-11   12
            \  /       /
             9-------- 
        */
        if (id == 7) UnlockPuzzle(9);
        if (id == 8 || id == 9) {
            UnlockPuzzle(10);
            UnlockPuzzle(12);
        }
        /*       15-16-17
                /    \
           12-13-14  18----..
        */
        if (id == 13) UnlockPuzzle(15);
        if (id == 16) UnlockPuzzle(18);
        
        /*                -----24
                         /
            18-19-20-21-22-23
                    \__/
        */
        if (id == 20) UnlockPuzzle(22);
        if (id == 22) UnlockPuzzle(24);
    }
    static void UnlockPuzzle(int id)
    {
        if (puzzleStar[id] != -1) return;
        puzzleStar[id] = 0;
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
