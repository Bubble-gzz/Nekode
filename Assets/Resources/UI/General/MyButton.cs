using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Start is called before the first frame update
    [SerializeField]
    public UnityEvent onClick = new UnityEvent();
    public AnimationBuffer animationBuffer;
    Vector3 originSize;
    bool mouseEnter;
    void Awake()
    {
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        gameObject.AddComponent<PopAnimator>();
        gameObject.AddComponent<UpdatePosAnimator>();
        gameObject.AddComponent<ChangeColorAnimator>();
        mouseEnter = false;
    }
    virtual protected void Start()
    {
        originSize = transform.localScale;
    }
    virtual protected void Update()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        transform.localScale = originSize * 0.7f;
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopOut, 0.07f));
        onClick.Invoke();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopOut, 0.07f));
        //transform.localScale = originSize * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopBack, 0.07f));
        //transform.localScale = originSize * 1.0f;
    }
    public void OnMouseEnter()
    {
        mouseEnter = true;
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopOut, 0.07f)); 
    }
    void OnMouseExit()
    {
        mouseEnter = false;
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopBack, 0.07f));        
    }
    void OnMouseDown()
    {
        if (!mouseEnter) return;
        transform.localScale = originSize * 0.7f;
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopOut, 0.07f));
        onClick.Invoke();
    }
    virtual public void Appear()
    {

    }
    virtual public void Disappear()
    {

    }
}
