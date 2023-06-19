using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningStory : MonoBehaviour
{
    // Start is called before the first frame update
    public MyDialogueBox dialogue;
    void Start()
    {
        StartCoroutine(Story());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Story()
    {
        yield return null;
    }
}
