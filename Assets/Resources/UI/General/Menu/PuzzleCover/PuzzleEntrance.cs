using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleEntrance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void JumpToPuzzle(string puzzleName)
    {
        Global.currentPuzzleName = puzzleName;
        SceneManager.LoadScene("GamePlay");
    }
}
