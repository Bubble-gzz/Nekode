using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    // Start is called before the first frame update
    MyPanel panel;
    void Awake()
    {
        panel = GetComponent<MyPanel>();
        GameMessage.OnPlay.AddListener(Appear);
        GameMessage.OnReset.AddListener(Disappear);
        GameMessage.OnPuzzleComplete.AddListener(Disappear);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Appear()
    {
        panel?.Appear();
    }
    void Disappear()
    {
        panel?.Disappear();
    }
}
