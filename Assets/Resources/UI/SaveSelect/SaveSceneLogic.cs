using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSceneLogic : MonoBehaviour
{
    // Start is called before the first frame update
    MyPanel CreateNewSaveButtons;
    void Awake()
    {
        CreateNewSaveButtons = transform.Find("CreateNewSaveButtons").GetComponent<MyPanel>();
        SaveEntry.CreateNewSave.AddListener(OnCreateNewSave);
    }
    void Start()
    {
        AudioManager.PlayMusicByName("Puzzle_Select");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCreateNewSave()
    {
        StartCoroutine(ShowButtons());
    }
    IEnumerator ShowButtons()
    {
        if (SaveEntry.choosedName != "save2") yield return new WaitForSeconds(0.6f);
        CreateNewSaveButtons.Appear();
        yield return null;
    }
    public void CancelButtonClicked()
    {
        SaveEntry.CancelCreateNewSave.Invoke();
        CreateNewSaveButtons.Disappear();
    }
    public void OKButtonClicked()
    {
        SaveEntry.ConfirmCreateNewSave.Invoke();
    }
}
