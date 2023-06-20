using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    static GameUIManager instance;
    public MyPanel resultPanelInTheScene;
    public GameObject dialogueBoxPrefab;
    public Transform instructorCat;
    public List<MyPanel> panelsInControl;
    public List<MyPanel> editPanelsInControl;
    static Slider speedSlider;
    static public GameObject HelpCardPrefab;
    static public GameObject curHelpCardPrefab;
    
    void Awake()
    {
        instance = this;
        instructorCat = GameObject.Find("Instructor")?.transform;
        speedSlider = GameObject.Find("SpeedController")?.GetComponentInChildren<Slider>();
        if (speedSlider != null)
        {
            speedSlider.onValueChanged.AddListener(ChangeSpeed);
        }
    }
    static public GameObject PopOutPanel(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, instance.transform);
        //panel.Appear();
        return obj;
    }
    static public GameObject PopOutDialogueBox()
    {
        GameObject obj = Instantiate(instance.dialogueBoxPrefab, instance.transform);
        MyPanel panel = obj.GetComponent<MyPanel>();
        //panel.Appear();
        return obj;        
    }
    static public MyPanel PopOutResultPanel()
    {
        instance.resultPanelInTheScene.Appear();
        return instance.resultPanelInTheScene;
    }
    public void PopOutHelpPanel()
    {
        if (curHelpCardPrefab != null) return;
        if (HelpCardPrefab == null) return;
        curHelpCardPrefab = PopOutPanel(HelpCardPrefab);
    }
    static public void CloseHelpPanel()
    {
        curHelpCardPrefab = null;
    }
    static public void FoldUI()
    {
        foreach (var panel in instance.panelsInControl)
        {
            panel.Disappear();
        }
    }
    static public void UnFoldUI()
    {
        foreach (var panel in instance.panelsInControl)
        {
            panel.Appear();
        }
    }
    static public void SetPuzzleTarget(string target)
    {

    }
    static public void FoldEditUI()
    {
        foreach (var panel in instance.editPanelsInControl)
        {
            panel.Disappear();
        }
    }
    static public void UnFoldEditUI()
    {
        foreach (var panel in instance.editPanelsInControl)
        {
            panel.Appear();
        }
    }
    void ChangeSpeed(float value)
    {
        Global.nekoPlaySpeed = value;
    }
}
