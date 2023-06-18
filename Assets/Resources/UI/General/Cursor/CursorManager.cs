using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class CursorManager : MonoBehaviour
{
    // Start is called before the first frame update
    static CursorManager Instance;
    static Dictionary<string, Image> cursors = new Dictionary<string, Image>();
    [Serializable]
    public class CursorInfo{
        public string name;
        public Image image;
    }
    public List<CursorInfo> cursorPresets = new List<CursorInfo>();
    Image texture;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            foreach(var cursorInfo in cursorPresets)
                if (!cursors.ContainsKey(cursorInfo.name))
                    cursors.Add(cursorInfo.name, cursorInfo.image);
            texture = GetComponent<Image>();
            //Cursor.visible = false;
        }
        else {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }
    static public void SetCursor(string name)
    {
        if (Instance == null) return;
        var cursor = cursors[name];
        if (cursor == null)
        {
            Debug.Log("Cursor of this name does not exist!");
            return;
        }
        Instance.texture = cursor;
    }
    
}
