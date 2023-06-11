using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FileExplorer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    [SerializeField]
    Transform contentRoot;
    [SerializeField]
    GameObject itemPrefab;
    string curPath;
    [SerializeField]
    public string initialPath = "";
    List<string> targetExtensions = new List<string>();
    string rootPath;
    public UnityEvent<string, bool> openFile = new UnityEvent<string, bool>();
    public bool loadFromResource;
    void Awake()
    {
        rootPath = Application.dataPath;

        //Debug.Log(curPath);
        targetExtensions.Add(".json");
    }
    void Start()
    {
        if (loadFromResource) curPath = initialPath;
        else curPath = Application.dataPath + "/" + initialPath;
        ReloadItems(curPath);
    }
    void ResetPath(string newPath, bool reload = true)
    {
        curPath = newPath;
        Debug.Log(curPath);
        if (reload) ReloadItems(curPath);
    }
    bool FilterCheck(FileInfo file)
    {
        foreach (var ext in targetExtensions)
            if (file.Extension == ext) return true;
        return false;
    }
    void ClearItems()
    {
        foreach (Transform item in contentRoot)
            Destroy(item.gameObject);
    }
    void ReloadItems(string curPath)
    {
        if (loadFromResource)
        {
            Object[] objs = Resources.LoadAll(curPath);
            foreach (var obj in objs)
            {
                FileEntry newFileEntry = Instantiate(itemPrefab, contentRoot).GetComponent<FileEntry>();
                newFileEntry.fileExplorer = this;
                newFileEntry.SetName(obj.name, curPath + "/" + obj.name);
            }
        }
        else {
            DirectoryInfo curDir = new DirectoryInfo(curPath);
            if (curDir.Exists) curDir.Create();
            ClearItems();
            DirectoryInfo[] directories = curDir.GetDirectories();
            foreach (var directory in directories)
            {
                FileEntry newFileEntry = Instantiate(itemPrefab, contentRoot).GetComponent<FileEntry>();
                newFileEntry.isFile = false;
                newFileEntry.fileExplorer = this;
                newFileEntry.SetName(directory.Name, directory.FullName);
            }
            FileInfo[] files = curDir.GetFiles();
            foreach (var file in files)
            {
                if (!FilterCheck(file)) continue;
                FileEntry newFileEntry = Instantiate(itemPrefab, contentRoot).GetComponent<FileEntry>();
                newFileEntry.fileExplorer = this;
                newFileEntry.SetName(file.Name, file.FullName);
            }
        }
    }
    public void EnterDirectory(string dirName)
    {
        ResetPath(curPath + "/" + dirName);
    }
    public void ReturnToLastLevelDirectory()
    {
        if (curPath == rootPath) return;
        int i;
        for (i = curPath.Length - 1; i > 0; i--)
            if (curPath[i] == '/') break;
        string lastLevelPath = curPath.Substring(0, i);
        ResetPath(lastLevelPath);
    }
    public void SelectFile(string filePath)
    {
        openFile.Invoke(filePath, false);
        gameObject.SetActive(false);
    }
    void OnMouseEnter()
    {

    }
    void OnMouseExit()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Global.mouseOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {    
        Global.mouseOverUI = false;
    }
}