using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleEntry : MonoBehaviour
{
    // Start is called before the first frame update
    public int puzzleID;
    public string puzzleName;
    public Sprite fill1, fill2, fill3;
    public Color fillColor;
    CanvasGroup canvasGroup;
    Image image;
    int status;
    GameObject area;
    void Awake()
    {
        image = transform.Find("Fill").GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        area = transform.Find("Area").gameObject;
    }
    void OnEnable()
    {
        StartCoroutine(Init());
    }
    IEnumerator Init()
    {
        for (int i = 0; i < 3; i++)
            yield return new WaitForEndOfFrame();
        if (puzzleID < 0 || puzzleID >= PuzzleManager.puzzleCount) status = 0;
        else status = PuzzleManager.PuzzleInfo(puzzleID);
        if (status == -1)
        {
            canvasGroup.alpha = 0;
            area.SetActive(false);
        }
        else {
            canvasGroup.alpha = 1;
            area.SetActive(true);
            if (status > 0) image.color = fillColor;
            else image.color = Color.clear;
        }
        switch(status){
            case (1): image.sprite = fill1; break;
            case (2): image.sprite = fill2; break;
            case (3): image.sprite = fill3; break;
            default: break;
        }
    }

    // Update is called once per frame
    public void EnterPuzzle()
    {
        if (puzzleID < 0 || puzzleID >= PuzzleManager.puzzleCount) return;
        Global.currentPuzzleName = puzzleName;
        PuzzleManager.currentPuzzleID = puzzleID;
        SceneSwitcher.SwitchTo("GamePlay");
    }
    void Update()
    {
        
    }
}
