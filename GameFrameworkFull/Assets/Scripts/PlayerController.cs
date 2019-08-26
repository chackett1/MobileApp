using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameFramework.GameStructure.Levels;
using GameFramework.GameStructure;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text timerText;
    //public SimpleTouchController leftController;
    public VirtualJoystick joystick;
    public CameraController camCtrl;

    private Rigidbody rb;
    static public int count;
    private float speedUp;
    //private int speedUpLimit;
    private Transform canTransform;
    private float timeLeft = 300.0f;
    private string mostRecentItem;
    //private string[] mostRecentItem;
    List<string> stringList;
    //private int index = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        speedUp = 1;
        //speedUpLimit = 100;
        stringList = new List<string>();

        canTransform = Camera.main.transform;
    }

    public void setMostRecentItem(string itemName)
    {
        stringList.Add(itemName);

        //mostRecentItem[index] = itemName;
        //index++;

        //mostRecentItem = itemName;
    }

    public string getMostRecentItem()
    {
        if(stringList.Count > 0)
        {
            return stringList[stringList.Count - 1];
        }
        else
        {
            return null;
        }
        
        //return mostRecentItem[index - 1];
        
        // return mostRecentItem;
    }

    public int getCount()
    {
        return count;
    }
    void Update()
    {
        timeLeft -= Time.deltaTime;
        SetTimerText(timeLeft);
        if (timeLeft <= 0)
        {
            LevelManager.Instance.GameOver(true);
        }
    }

    void FixedUpdate()
    {
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");
        //Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        ////Vector3 movementT = new Vector3(moveVertical, 0f, -moveHorizontal);

        //float moveHorizontal = Input.GetAxis("Vertical");
        //float moveVertical = Input.GetAxis("Horizontal");
        //Vector3 movement = new Vector3(moveHorizontal, 0f, -moveVertical);
        //Vector3 movementT = new Vector3(-moveVertical, 0f, -moveHorizontal);

        // ********************** Keeper Code ********************** //
        Vector3 rotatedDir = canTransform.TransformDirection(joystick.InputDirection);
        rotatedDir = new Vector3(rotatedDir.x, 0, rotatedDir.z);
        rotatedDir = rotatedDir.normalized * rotatedDir.magnitude;

        
        Vector3 rotatedDirT = canTransform.TransformDirection(joystick.InputDirection);
        rotatedDirT = new Vector3(rotatedDirT.z, 0, -rotatedDirT.x);
        rotatedDirT = rotatedDirT.normalized * rotatedDirT.magnitude;
        //
        

        //rb.AddForce(rotatedDir * speed);
        //rb.AddTorque(movementT * (speed / 2));

        rb.AddForce(rotatedDir * speed * speedUp);// * Time.deltaTime);
        rb.AddTorque(rotatedDirT * speed * speedUp);

        //Debug.Log(speedUp);
        var vel = rb.velocity.magnitude;
        if (vel < 3 && rotatedDir.magnitude != 0f)
        {
            speedUp = speedUp + 0.01f;
            //rb.AddForce(rotatedDir * speed);
            //rb.AddTorque(rotatedDirT * (speed));

            //countText.text = "Slow";
            //rb.AddForce(movement * speed * speedUp);
            //rb.AddTorque(movementT * speed * (speedUp * 1.5f));
            //speedUp = speedUp + .5f;
            //if (speedUp > speedUpLimit)
            //{
            //speedUp = 10;
            //}

            //rb.AddForce(joystick.InputDirection * speed);// * Time.deltaTime);
            //rb.AddTorque(joystick.InputDirection * (speed));

            //rb.AddForce(rotatedDir * speed);// * Time.deltaTime);
            //rb.AddTorque(rotatedDir * (speed));
        }
        else
        {
            speedUp = 1;
            //countText.text = "Fast";
            //rb.AddForce(movement * speed);
            //rb.AddTorque(movementT * speed);
            //speedUp = 0;

            //rb.AddForce(joystick.InputDirection * speed);// * Time.deltaTime);
            //rb.AddTorque(joystick.InputDirection * (speed / 2));
        }

        //if (vel > 5)
        //{
        //    // Slow down
        //}

    }
    public void SetCountText()
    {
        //transform.localScale += new Vector3(0.1F, 0.1F, 0.1F);

        //GameManager.Instance.Player.AddCoin();
        GameManager.Instance.Levels.Selected.AddCoin();

        count++;
        countText.text = "Count: " + count.ToString();

        if (count % 10 == 0)
        {
            camCtrl.IncreaseOffset();
        }

        /*
        if (count > 20 && speedUpLimit != 40)
        {
            speedUpLimit = 40;
        }
        else if (count > 40 && speedUpLimit != 80)
        {
            speedUpLimit = 80;
        }*/
    }
    public void ReduceCountText()
    {
        //transform.localScale += new Vector3(0.1F, 0.1F, 0.1F);

        count--;
        stringList.RemoveAt(stringList.Count - 1);
        //index--;

        countText.text = "Count: " + count.ToString();

        if (count % 10 == 0)
        {
            camCtrl.IncreaseOffset();
        }
    }
    public void SetTimerText(float timeLeft)
    {
        timerText.text = "Timer: " + timeLeft.ToString("f0");
    }

    //void OnCollisionEnter(Collision c)
    //{
    //    // force is how forcefully we will push the player away from the enemy.
    //    float force = 10000;

    //    // If the object we hit is the enemy
    //    if (c.gameObject.tag == "Size1")
    //    {
    //        // Calculate Angle Between the collision point and the player
    //        Vector3 dir = c.contacts[0].point - transform.position;
    //        // We then get the opposite (-Vector3) and normalize it
    //        //dir = -dir.normalized;
    //        dir = dir.normalized;
    //        // And finally we add force in the direction of dir and multiply it by force. 
    //        // This will push back the player
    //        rb.AddForce(dir * force);

    //        Debug.Log("pushed");
    //    }
    //}
}
