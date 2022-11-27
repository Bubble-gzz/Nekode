using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameMessage : MonoBehaviour
{
    // Start is called before the first frame update
    TMP_Text message;
    AnimationBuffer animationBuffer;
    void Awake()
    {
        Global.gameMessage = this;
        message = GetComponent<TMP_Text>();
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        gameObject.AddComponent<PopAnimator>();
        message.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetNewMessage(string newMessage)
    {
        message.text = newMessage;
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.Emphasize, 0.1f));
    }
}
