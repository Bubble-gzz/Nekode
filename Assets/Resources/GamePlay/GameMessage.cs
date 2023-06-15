using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMessage : MonoBehaviour
{
    // Start is called before the first frame update
    static public UnityEvent OnArrowIsDeleted = new UnityEvent();
    static public UnityEvent OnToolReturnedToSlot = new UnityEvent();
    static public UnityEvent OnResetGridState = new UnityEvent();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
