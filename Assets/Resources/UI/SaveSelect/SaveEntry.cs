using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
public class SaveEntry : MonoBehaviour
{
    // Start is called before the first frame update
    static public UnityEvent CreateNewSave = new UnityEvent();
    static public UnityEvent CancelCreateNewSave = new UnityEvent();
    static public UnityEvent ConfirmCreateNewSave = new UnityEvent();
    public string saveName;
    public string nickName;
    public List<Sprite> covers = new List<Sprite>();
    MyPanel coverPanel, namePanel;
    public MyButtonImage bg;
    Image cover;
    TMP_Text title;
    bool newSave;
    bool choosed;
    Vector2 originalPos;
    RectTransform rect;
    static public string choosedName;
    MyButtonController panelButton;
    void Awake()
    {
        cover = transform.Find("bg/Cover/cover").GetComponentInChildren<Image>();
        title = transform.Find("bg/Cover/title").GetComponentInChildren<TMP_Text>();
        coverPanel = transform.Find("bg/Cover").GetComponent<MyPanel>();
        namePanel = transform.Find("Name").GetComponent<MyPanel>();
        panelButton = transform.Find("PanelArea").GetComponentInChildren<MyButtonController>();
        rect = GetComponent<RectTransform>();
        CreateNewSave.AddListener(OnCreateNewSave);
        CancelCreateNewSave.AddListener(OnCancelCreateNewSave);
        ConfirmCreateNewSave.AddListener(OnConfirmCreateNewSave);
        Init();
    }
    void Start()
    {
        coverPanel.Appear();
        namePanel.Disappear();
    }
    void Init()
    {
        originalPos = rect.anchoredPosition;
        choosed = false;
        PuzzleManager.PuzzleData data = PuzzleManager.GetData(saveName);
        if (data == null)
        {
            cover.sprite = covers[0];
            title.text = nickName = "Someone New";
            newSave = true;
        }
        else
        {
            cover.sprite = covers[1];
            title.text = nickName = data.nickName;
            newSave = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnChoosed()
    {
        if (!newSave)
        {
            PuzzleManager.LoadData(saveName);
            SceneSwitcher.SwitchTo("PuzzleSelect");
        }
        else
        {
            choosed = true;
            choosedName = saveName;
            //bg.OnMouseExit();
            CreateNewSave.Invoke();
        }
    }
    void OnCreateNewSave()
    {
        if (choosed) StartCoroutine(ZoomIn());
        else ZoomOut();
    }
    void OnCancelCreateNewSave()
    {
        if (choosed) nickName = "Someone New";
        StartCoroutine(ZoomBack());
    }
    IEnumerator ZoomIn()
    {
        nickName = "";
        panelButton.SetActive(false, false);
        coverPanel.Disappear();
        namePanel.Appear();
        if (saveName != "save2") yield return new WaitForSeconds(0.2f);
        originalPos = rect.anchoredPosition;
        rect.DOAnchorPos(new Vector2(0, -40), 0.7f).SetEase(Ease.InOutCubic);
    }
    void ZoomOut()
    {
        rect.DOAnchorPos(originalPos + new Vector2(0, 1000), 0.7f).SetEase(Ease.OutCubic);
    }
    IEnumerator ZoomBack()
    {
        coverPanel.Appear();
        namePanel.Disappear();
        if (!choosed && choosedName != "save2") yield return new WaitForSeconds(0.3f);
        rect.DOAnchorPos(originalPos, 0.7f).SetEase(Ease.OutCubic);
        choosed = false;
        panelButton.SetActive(true, false);
        yield return null;
    }
    void OnConfirmCreateNewSave()
    {
        if (!choosed) return;
        if (nickName == "") return;
        PuzzleManager.saveName = saveName;
        PuzzleManager.NewSave(nickName);
        SceneSwitcher.SwitchTo("PuzzleSelect");
    }
    public void UpdateName(string newName)
    {
        nickName = newName;
    }
}
