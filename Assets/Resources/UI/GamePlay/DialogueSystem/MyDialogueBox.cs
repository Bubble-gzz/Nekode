using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class MyDialogueBox : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 defaultBoxSize;
    TMP_Text content;
    RectTransform rect;
    List<float> charSizes;
    public bool isPlaying;
    public bool fastforward;
    MyPanel panel;
    Transform targetObject = null;
    public Vector2 boxOffset;
    Transform tail;
    Camera mainCam;
    public Vector2 tailOffset;
    public TMP_FontAsset font_CH, font_EN;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        content = GetComponentInChildren<TMP_Text>();
        isPlaying = false;
        panel = GetComponent<MyPanel>();
        fastforward = false;
        tail = transform.Find("Tail");
    }
    void Start()
    {
        mainCam = Global.mainCam;
        targetObject = DrBubble.instance.transform;
        if (Settings.language == "CH") content.font = font_CH;
        else content.font = font_EN;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            Play("So let's start with the basics.\nSo let's start with the basics.\nSo let's start with the basics.\nSo let's start with the basics.", new Vector2(400, 400));//Play("So let's start with the basics.", new Vector2(550,150));
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            //StartCoroutine(UpdateMesh(0));
        }
        if (Input.anyKeyDown) fastforward = true;
        tail.position = rect.position + (Vector3)tailOffset; //mainCam.ScreenToWorldPoint(tailOffset);
        //UpdateMesh();
    }
    public void Open(Vector2 size)
    {
        rect.sizeDelta = size;
        Open();
    }
    public void Open()
    {
        if (mainCam == null) mainCam = Global.mainCam;
        if (targetObject == null) targetObject = DrBubble.instance.transform;
        Debug.Log("dialogue.mainCam " + mainCam + "  /  global:" + Global.mainCam);
        transform.position = mainCam.WorldToScreenPoint(targetObject.transform.position) + (Vector3)boxOffset;
        panel.Appear();
    }
    public void Close(bool destroy = false)
    {
        panel.Disappear(destroy);
    }
    public void Play(string newContent)
    {
        Play(newContent, defaultBoxSize);
    }

    void UpdateMesh()
    {   
        if (charSizes == null || content.text == "" || !isPlaying) return;
        content.ForceMeshUpdate();
        var textInfo = content.textInfo;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;
 
            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            int baseIndex = charInfo.vertexIndex;
            Vector3 center = Vector3.zero;
            for (int j = 0; j < 4; j++) center += verts[baseIndex + j];
            center = center / 4;
            for (int j = 0; j < 4; j++)
                verts[baseIndex + j] = center + (verts[baseIndex + j] - center) * charSizes[i];
        }
        for (int i = 0; i < textInfo.meshInfo.Length; i++)//写入刚刚信息
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            content.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    public void Play(string newContent, Vector2 newBoxSize)
    {
        isPlaying = true;
        fastforward = false;
        StartCoroutine(C_Play(newContent, newBoxSize));
    }
    IEnumerator C_Play(string newContent, Vector2 newBoxSize)
    {
        rect.DOSizeDelta(newBoxSize, 0.2f);
        content.text = "";
        charSizes = new List<float>();
        isPlaying = true;
        for (int i = 0; i < newContent.Length; i++)
        {
            content.text += newContent[i];
            yield return new WaitForSeconds(0.02f);
            if (fastforward) {
                content.text = newContent;
                break;
            }
            //charSizes.Add(0);
            //StartCoroutine(PopOutChar(i));
            //await Task.Delay(50);
        }
        yield return new WaitForEndOfFrame();
        while (!Input.anyKeyDown) yield return null;
        //await Task.Delay(1000);
        isPlaying = false;
    }
    IEnumerator PopOutChar(int i)
    {
        var complete = false;
        //content.ForceMeshUpdate();
        //一个meshInfo里面可能包含很多个字符的vertices
        TweenCallback<float> updateMesh = (t)=>{
                charSizes[i] = t;
            };

        Sequence sequence = DOTween.Sequence();
        sequence.Append(DOVirtual.Float(0, 1.4f, 0.5f, updateMesh).SetEase(Ease.InOutSine));
        sequence.Append(DOVirtual.Float(1.4f, 0.8f, 0.5f, updateMesh).SetEase(Ease.InOutSine));
        sequence.Append(DOVirtual.Float(0.8f, 1f, 0.5f, updateMesh).SetEase(Ease.InOutSine));
        sequence.OnComplete(()=>{
            complete = true;
            Debug.Log(i + "complete");
        });
        while (!complete)
        {
            yield return null;
        }
    }
    public void SetPos(Vector2 newPos)
    {

    }
    public void SetPivot(Vector2 newPivot)
    {
        rect.pivot = newPivot;
        rect.anchoredPosition = Vector2.zero;
    }
    public void SetTailOffset(Vector2 newOffset)
    {
        tailOffset = newOffset;
    }
    public void SetTailRotation(Vector3 rotation)
    {
        tail.rotation = Quaternion.Euler(rotation);
    }
    public void SetTailLR()
    {
        SetPivot(new Vector2(1, 0));
        SetTailOffset(new Vector2(-25, 20));
        SetTailRotation(new Vector3(0, 0, -30));
        boxOffset = new Vector2(-50, 50);
    }
    public void SetTailLM()
    {
        SetPivot(new Vector2(0.5f, 0));
        SetTailOffset(new Vector2(0, 0));
        SetTailRotation(new Vector3(0, 0, -90));
        boxOffset = new Vector2(0, 80);        
    }
}