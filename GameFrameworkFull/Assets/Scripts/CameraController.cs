using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform lookAt;
    public VirtualJoystick joystick;

    private Vector3 offset;

    //private float distance = 5.0f;
    //private float yOffset = 3.5f;

    private void Start()
    {
        offset = (transform.position - lookAt.transform.position);// * 2;
        //offset = new Vector3(0, yOffset, -1f * distance);
    }

    public void IncreaseOffset()
    {
        offset = offset * (float) 1.03;
    }

    private void Update()
    {
        //float speed = 0.1f;

        //Vector3 rotatedDir = Camera.main.transform.TransformDirection(joystick.InputDirection);
        //rotatedDir = new Vector3(rotatedDir.x, 0, rotatedDir.z);
        //rotatedDir = rotatedDir.normalized * rotatedDir.magnitude;

        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        if (joystick.InputDirection.x > 0)
        {
            SlideCamera(true);


            //Debug.Log("Touch Count " + Input.touchCount);

            //Vector2 TouchDeltaPosition = Input.GetTouch(0).deltaPosition;
            //transform.Translate(-TouchDeltaPosition.x * speed, -TouchDeltaPosition.y * speed, 0);
       }
        else if(joystick.InputDirection.x < 0)
        {
            SlideCamera(false);
        }
       //Debug.Log("Touch Count " + Input.touchCount);


        transform.position = lookAt.transform.position + offset;
        //transform.position = lookAt.position + offset;

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SlideCamera(false);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SlideCamera(true);
        }

        transform.position = lookAt.position + offset;
        transform.LookAt(lookAt);
    }

    public void SlideCamera(bool right)
    {
        if (right)
        {
            //offset = Quaternion.Euler(0, 90, 0) * offset;
            offset = Quaternion.Euler(0, 2, 0) * offset;
        }
        else
        {
            //offset = Quaternion.Euler(0, -90, 0) * offset;
            offset = Quaternion.Euler(0, -2, 0) * offset;
        }
    }
    /*
    public GameObject player;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }
    
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }*/
}