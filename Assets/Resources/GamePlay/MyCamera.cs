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
    void Awake()
    {
        Global.mainCam = GetComponent<Camera>();
        mode = Mode.WSAD;
        velocity = new Vector2(0, 0);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == Mode.WSAD)
        {
            Vector2 accel = new Vector2(0, 0);
            if (Input.GetKey(KeyCode.A)) accel.x -= 1;
            if (Input.GetKey(KeyCode.D)) accel.x += 1;
            if (Input.GetKey(KeyCode.S)) accel.y -= 1;
            if (Input.GetKey(KeyCode.W)) accel.y += 1;
            accel = accel.normalized * accelRate;
            velocity += accel;
            if (velocity.magnitude > speedLimit) velocity = velocity.normalized * speedLimit;
            if (velocity.magnitude > damping * Time.deltaTime)
                velocity -= velocity.normalized * damping * Time.deltaTime;
            else velocity = new Vector2(0, 0);
            transform.position += (Vector3)velocity * Time.deltaTime;
        }
    }
}
