using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollMenu : MonoBehaviour
{
    // Start is called before the first frame update
    float damping;
    float maxVelocity;
    float scrollPosition;
    Camera mainCam;
    float lastPos, lastDist, lastInterval;
    float velocity;
    float flipAmplifier;
    float snapForce;
    bool isMouseHolding;
    List<GameObject> items = new List<GameObject>();
    List<float> offsets = new List<float>();
    float totalLength;
    float interval;
    [SerializeField] bool debugMode;
    [SerializeField] bool loop;
    [SerializeField] float edgeDragForce;
    [SerializeField] float dragBoundary;

    Transform itemRoot;
    class Moment{
        public float deltaPos,deltaTime;
        public Moment(float deltaPos, float deltaTime)
        {
            this.deltaPos = deltaPos;
            this.deltaTime = deltaTime;
        }
    }
    Queue<Moment> queue = new Queue<Moment>();
    int movementBufferSize;

    [SerializeField]
    TextMeshProUGUI debugText;
    void Start()
    {
        SetParameters();
        GetChildren();
        if (loop) GenerateBuffer();
    }
    void SetParameters()
    {
        mainCam = Global.mainCam;
        itemRoot = transform.Find("Items");

        velocity = 0;
        flipAmplifier = 0.5f;
        damping = 120; //400
        snapForce = 200; //500
        maxVelocity = 100;
        interval = 10;

        lastDist = 0;
        lastInterval = 0;
        movementBufferSize = 5;

        isMouseHolding = false;
        scrollPosition = 0;
    }
    void GetChildren()
    {
        items.Clear();
        //Transform[] children = itemRoot.GetComponentsInChildren<Transform>();
        foreach(Transform child in itemRoot.gameObject.transform) items.Add(child.gameObject);
        totalLength = items.Count * interval;
        float offset = 0;
        for (int i = 0; i < items.Count; i++)
        {
            offsets.Add(offset);
            offset += interval;
        }
    }
    void GenerateBuffer()
    {
        int count = items.Count;
        for (int i = 0; i < count; i++)
        {
            items.Add(Instantiate(items[i], itemRoot));
            offsets.Add(offsets[i] - totalLength);
        }
        for (int i = 0; i < count; i++)
        {
            items.Add(Instantiate(items[i], itemRoot));
            offsets.Add(offsets[i] + totalLength);
        }
    }
    // Update is called once per frame
    void Update()
    {
        DragChecking();
        MovementChecking();
        Render();
    }
    void DragChecking()
    {
        bool inBoundary = loop || (-scrollPosition > -dragBoundary && -scrollPosition < totalLength - interval + dragBoundary);
        //Debug.Log("totalLenght: " + totalLength);
        if (Input.GetMouseButton(0) && inBoundary)
        {
            float pos = mainCam.ScreenToWorldPoint(Input.mousePosition).x;
            if (!isMouseHolding)
            {
                isMouseHolding = true;
            }
            else
            {
                velocity = lastDist * flipAmplifier / lastInterval ; 
                velocity = Mathf.Clamp(velocity, -maxVelocity, maxVelocity);
                scrollPosition += (pos - lastPos) * 2;
            }
            queue.Enqueue(new Moment(pos - lastPos, Time.deltaTime));
            lastDist += pos - lastPos;
            lastInterval += Time.deltaTime;
            if (queue.Count > movementBufferSize) {
                Moment moment = queue.Peek();
                lastDist -= moment.deltaPos;
                lastInterval -= moment.deltaTime;
                queue.Dequeue();
            }
            lastPos = pos;

        }
        else {
            if (isMouseHolding) {
                isMouseHolding = false;
            }
        }
        if (!loop)
        {
            if (-scrollPosition < 0)
                velocity -= scrollPosition *  edgeDragForce * Time.deltaTime;
            if (-scrollPosition > totalLength - interval)
                velocity += (-scrollPosition - totalLength + interval) * edgeDragForce * Time.deltaTime;
        }
    }
    void MovementChecking()
    {
        if (isMouseHolding) return;
        scrollPosition += velocity * Time.deltaTime;
        if (loop)
        {
            if (scrollPosition > totalLength) scrollPosition -= totalLength;
            if (scrollPosition < -totalLength) scrollPosition += totalLength;
        }
        float delta = -damping * Time.deltaTime * Mathf.Abs(velocity) / velocity;
        if (Mathf.Abs(velocity) > Mathf.Abs(delta)) velocity += delta;
        else velocity = 0;
        float minDist = (float)1e9;
        int minID = -1;
        for (int i = 0; i < offsets.Count; i++)
        {
            float dist = Mathf.Abs(offsets[i] - (-scrollPosition));
            if (minID == -1 || dist < minDist) {
                minDist = dist;
                minID = i;
            }
        }
        //Debug.Log("minID:" + minID + " minDist:" + minDist);
        if (minDist > 0.01f)
            velocity += -((offsets[minID] - (-scrollPosition)) / minDist) * snapForce * Time.deltaTime;
    }
    void Render()
    {
        if (debugMode && debugText != null) debugText.text = velocity.ToString("f2");
        for (int i = 0; i < items.Count; i++)
        {
            GameObject obj = items[i];
            Vector3 pos = obj.transform.position;
            pos.x = scrollPosition + offsets[i];
            obj.transform.position = pos;
        }
    }
}
