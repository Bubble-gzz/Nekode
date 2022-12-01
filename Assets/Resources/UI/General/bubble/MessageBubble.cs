using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageBubble : MonoBehaviour
{
    // Start is called before the first frame update
    AnimationBuffer animationBuffer;
    TMP_Text message;
    void Awake()
    {
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        gameObject.AddComponent<PopAnimator>();
        message = GetComponentInChildren<TMP_Text>();
    }
    void OnEnable()
    {
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopOut, 0.1f));
    }
    public void SetMessage(string text)
    {
        message.text = text;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
