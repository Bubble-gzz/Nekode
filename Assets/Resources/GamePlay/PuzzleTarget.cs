using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PuzzleTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_FontAsset Font_CH, Font_EN;
    TMP_Text message;
    AnimationBuffer animationBuffer;
    MyPanel panel;
    void Awake()
    {
        Global.puzzleTarget = this;
        message = GetComponent<TMP_Text>();
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        gameObject.AddComponent<PopAnimator>();
        message.text = "";
        if (Settings.language.ToUpper() == "CH") message.font = Font_CH;
        else message.font = Font_EN; 
        panel = GetComponent<MyPanel>();
        GameMessage.OnPlay.AddListener(Disappear);
        GameMessage.OnReset.AddListener(Appear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetNewMessage(string newMessage)
    {
        message.text = newMessage;
        panel.Appear();
        panel.SetStyle(MyPanel.Style.Fold);
        //animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.Emphasize, 0.1f));
    }
    public void Appear()
    {
        panel.Appear();
    }
    public void Disappear()
    {
        panel.Disappear();
    }
}
