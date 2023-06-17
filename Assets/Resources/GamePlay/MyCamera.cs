using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Mode
    {
        Follow,
        WSAD
    }
    [SerializeField]
    public Mode mode;
    [SerializeField]
    float damping;
    [SerializeField]
    float accelRate = 1;
    [SerializeField]
    float speedLimit;
    Vector2 velocity;
    Vector3 lastMousePos;
    bool mouseMiddleButtonDown;
    public bool freelyMove;
    void Awake()
    {
        Global.mainCam = GetComponent<Camera>();
        mode = Mode.WSAD;
        velocity = new Vector2(0, 0);
        mouseMiddleButtonDown = false;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (freelyMove) CheckMove();
    }
    void CheckMove()
    {
        if (GameMessage.playingStory) return;
        if (mode == Mode.WSAD && !Global.isTyping)
        {
            Vector2 accel = new Vector2(0, 0);
            if (Input.GetKey(KeyCode.A)) accel.x -= 1;
            if (Input.GetKey(KeyCode.D)) accel.x += 1;
            if (Input.GetKey(KeyCode.S)) accel.y -= 1;
            if (Input.GetKey(KeyCode.W)) accel.y += 1;
            accel = accel.normalized * accelRate;
            velocity += accel * Time.deltaTime;
            if (velocity.magnitude > speedLimit) velocity = velocity.normalized * speedLimit;
            if (velocity.magnitude > damping * Time.deltaTime)
                velocity -= velocity.normalized * damping * Time.deltaTime;
            else velocity = Vector2.zero;

            Vector3 mousePos = Input.mousePosition;
            if (mouseMiddleButtonDown)
            {
                if (Global.mainCam != null)
                    transform.position += Global.mainCam.ScreenToWorldPoint(mousePos) - Global.mainCam.ScreenToWorldPoint(lastMousePos);
            }
            if (Input.GetMouseButton(2)) {
                velocity = Vector2.zero;
                mouseMiddleButtonDown = true;
                lastMousePos = mousePos;
            }
            else mouseMiddleButtonDown = false;

            transform.position += (Vector3)velocity * Time.deltaTime;
        }
        if (mode == Mode.Follow)
        {
            Neko neko = Global.currentNeko;
            Vector2 offset = neko.transform.position - transform.position;
            if (offset.magnitude > 0.01f)
                transform.position += (Vector3)offset.normalized * offset.magnitude * 4f * Time.deltaTime;
        }
    }
}
