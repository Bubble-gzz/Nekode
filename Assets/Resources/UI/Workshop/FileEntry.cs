using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FileEntry : MonoBehaviour
{
    // Start is called before the first frame update
    TMP_Text text;
    public bool isFile = true;
    string fileName;
    string filePath;
    public FileExplorer fileExplorer;
    void Awake()
    {
        text = transform.Find("Text").GetComponent<TMP_Text>();
    }
    public void SetName(string newName, string path)
    {
        fileName = newName;
        filePath = path;
        SetText(fileName);
    }
    public void SetText(string newString)
    {
        text.text = newString;
    }
    public void OnClicked()
    {
        if (!isFile) fileExplorer.EnterDirectory(fileName);
        else fileExplorer.SelectFile(filePath);
    }
}