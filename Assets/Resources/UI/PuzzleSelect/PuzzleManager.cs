using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    // Start is called before the first frame update
    static PuzzleManager Instance;
    static bool newSave = true;
    static public int calendarPage;
    List<int> puzzleStar = new List<int>(); //-1 locked  0 newPuzzle 1~3 stars
    static public int puzzleCount = 30;
    static public int currentPuzzleID = 0;
    static string saveName = "showcase";
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (newSave) InitSave();
            calendarPage = 0;
        }
        else {
            Destroy(gameObject);
        }
    }
    void InitSave()
    {
        for(int i = 0; i < puzzleCount; i++)
            puzzleStar.Add(-1);
        puzzleStar[0] = 0;
//        puzzleStar[1] = 2;
  //      puzzleStar[2] = 1;
    //    puzzleStar[3] = 0;
    }
    static public int PuzzleInfo(int id)
    {
        if (Instance == null) return -1;
        return Instance.puzzleStar[id];
    }
    static public void UpdatePuzzleInfo(int id, int starCount)
    {
        if (id < 0 || id >= puzzleCount) return;
        if (starCount > Instance.puzzleStar[id]) Instance.puzzleStar[id] = starCount;
        if (id < puzzleCount - 1) // Unlock new puzzle
        {
            if (Instance.puzzleStar[id + 1] == -1) Instance.puzzleStar[id + 1] = 0;
        }
        SaveData();
    }
    static public void SaveData()
    {

    }
    static public void LoadData()
    {

    }
}
