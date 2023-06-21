using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class CursorManager : MonoBehaviour
{
    // Start is called before the first frame update
    static CursorManager Instance;
    static Dictionary<string, Sprite> cursors = new Dictionary<string, Sprite>();
    [Serializable]
    public class CursorInfo{
        public string name;
        public Sprite image;
    }
    public List<CursorInfo> cursorPresets = new List<CursorInfo>();
    Image texture;
    Transform cursor;
    CanvasGroup canvasGroup;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            foreach(var cursorInfo in cursorPresets)
                if (!cursors.ContainsKey(cursorInfo.name))
                    cursors.Add(cursorInfo.name, cursorInfo.image);
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1;

            texture = GetComponentInChildren<Image>();
            cursor = transform.Find("Cursor");
            Cursor.visible = false;
        }
        else {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Show();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    Sequence animSeq;
    void Update()
    {
        cursor.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            animSeq?.Kill();
            animSeq = DOTween.Sequence();
            animSeq.Append(cursor.DOScale(Vector3.one * 0.5f, 0.05f));
            animSeq.Append(cursor.DOScale(Vector3.one * 1.2f, 0.1f));
            animSeq.Append(cursor.DOScale(Vector3.one * 0.9f, 0.1f));
            animSeq.Append(cursor.DOScale(Vector3.one * 1f, 0.1f));
        }
    }
    static public void SetCursor(string name)
    {
        if (Instance == null) return;
        var cursorTexture = cursors[name];
        if (cursorTexture == null)
        {
            Debug.Log("Cursor of this name does not exist!");
            return;
        }
        Instance.texture.sprite = cursorTexture;
    }
    static public void Show()
    {
        Instance.canvasGroup.alpha = 1;
    }
    static public void Hide()
    {
        Instance.canvasGroup.alpha = 0;
    }
    
}
