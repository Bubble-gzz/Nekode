using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class MyButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UnityEvent onMouseEnter = new UnityEvent();
    public UnityEvent onMouseExit = new UnityEvent();
    public UnityEvent onMouseClick = new UnityEvent();
    public bool active = true;
    public MyButtonImage image;
    void Awake()
    {
        if (image)
        {
            onMouseClick.AddListener(image.OnClicked);
            onMouseEnter.AddListener(image.OnMouseEnter);
            onMouseExit.AddListener(image.OnMouseExit);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!active) return;
        onMouseClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!active) return;
        onMouseEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!active) return;
        onMouseExit.Invoke();

    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetActive(bool flag)
    {
        active = flag;
        if (flag) image?.Appear();
        else image?.Disappear();
    }
    public void Clicked()
    {
        image?.OnClicked();
    }
}
