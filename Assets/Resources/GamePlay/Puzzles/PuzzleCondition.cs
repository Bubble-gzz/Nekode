using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleCondition : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<Sprite> stateTexture = new List<Sprite>();
    Image state;
    public bool satisfied;
    AnimationBuffer animationBuffer;
    void Awake()
    {
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        state = transform.Find("State").GetComponentInChildren<Image>();
        transform.Find("State").gameObject.AddComponent<PopAnimator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetState(bool newState, bool animated = true)
    {
        if (!newState) state.sprite = stateTexture[0];
        else state.sprite = stateTexture[1];
        if (satisfied == newState) return;
        satisfied = newState;
        if (animated) animationBuffer.Add(new PopAnimatorInfo(state.gameObject, PopAnimator.Type.Emphasize, 0.1f));
    }
}
