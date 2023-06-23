using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveButton : MyButton
{
    // Start is called before the first frame update
    [SerializeField]
    MyGrid grid;
    [SerializeField]
    Workshop workshop;
    override protected void Start()
    {
        base.Start();
        onClick.AddListener(Save);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    public void Save()
    {
        if (workshop.puzzleName == "") return;
        string data = JsonUtility.ToJson(grid.ConvertToData());
        DirectoryInfo root = new DirectoryInfo(Application.persistentDataPath + "/TempMapData/");
        if (!root.Exists) root.Create();
        string path = root + workshop.puzzleName + ".json";
        File.WriteAllText(path, data);
        Debug.Log(data);
    }
}
