using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour {

    private float speed = 2;
    private float waitTime = 5;
    private float startWaitTime = 5;
    private static bool stop = false;
    private float walkingTime = 0;
    private float walkingTimeMax = 10;

    public PlayerController playerController;
    //public PickupItem pickupItem;
    public Transform player;
    public Transform moveSpot;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    Animator anim;

    // Use this for initialization
    void Start () {
        //waitTime = startWaitTime;
        anim = GetComponent<Animator>();
        moveSpot.position = new Vector3(Random.Range(minX, maxX), 0.3f, Random.Range(minY, maxY));

        AnimationEvent ae = new AnimationEvent(); ae.messageOptions = SendMessageOptions.DontRequireReceiver;

        //Debug.Log("Size " + GetComponentInChildren<Collider>().bounds.size.ToString());
    }

    void Hit()
    {
        //SM_Env_Flowers_07 (12)
        //PlayerController abc = new PlayerController();
        //player.
        if(playerController.getMostRecentItem() != null) // TEMP 40 IS CURRENTLY HARDCODED
        {
            for(int i = 0; i < 5; i++)
            {
                Transform child = player.Find(playerController.getMostRecentItem());
                if (child != null)
                {
                    Rigidbody gameObjectsRigidBody = child.gameObject.AddComponent<Rigidbody>();
                    // Check if player has objects to be knocked off
                    if(gameObjectsRigidBody != null)
                    {
                        gameObjectsRigidBody.mass = 1;

                        playerController.ReduceCountText();

                        //Vector3 direction = child.transform.position - transform.position;
                        //GetComponent<Rigidbody>().AddForce(direction * -100000);


                        child.transform.parent = null;

                        // Deletes item after fallen to floor
                        StartCoroutine(deleteItem(child.gameObject));
                    }
                }
            }
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
       /* if (collision.gameObject.tag.Equals("Player"))
        {
            stop = true;
        }*/
    }

    public static void StopWalking()
    {
        stop = true;
    }

    // Update is called once per frame
    void Update () {
        //if (self.transform.parent.tag == "Player")
        if (transform.parent != null)
        {
            //Debug.Log("Stopping Patrol");
            stop = true;
        }

        if (!stop)
        {


            //Vector3 direction = player.position - this.transform.position;
            Vector3 direction;
            if (Vector3.Distance(player.position, this.transform.position) < 10)
            {
                Debug.Log("Close to player");

                direction = player.position - this.transform.position;
                direction.y = 0;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.2f);

                if (direction.magnitude > 2)
                {
                    this.transform.Translate(0, 0, 0.05f);
                    //transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

                    //Debug.Log("Set Walking");
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isIdle", false);
                    anim.SetBool("isAttacking", false);
                }
                else if (playerController.getCount() < 40) // TEMP: HARDCODED BUT SHOULD BE DYNAMIC
                {
                    //Debug.Log("Set Attacking");
                    player.GetComponent<Rigidbody>().AddForce(direction * 50000);
                    //StartCoroutine(IncreaseLayerAfterTimeout());

                    anim.SetBool("isAttacking", true);
                    anim.SetBool("isIdle", false);
                    anim.SetBool("isWalking", false);
                }
            }

            else
            {
                walkingTime += Time.deltaTime;
                if (walkingTime > walkingTimeMax)
                {
                    Debug.Log("Stuck");
                    //moveSpot.position = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minY, maxY));
                    moveSpot.position = new Vector3(Random.Range(minX, maxX), 0.3f, Random.Range(minY, maxY));

                    walkingTime = 0;
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isIdle", false);
                    anim.SetBool("isAttacking", false);
                }
                //Debug.Log("Not Stuck");


                direction = moveSpot.position - this.transform.position;
                //if (waitTime > 4)
                //{
                    //Vector3 zeroVector = new Vector3(0, 0, 0);
                    //if (direction != zeroVector)
                    //{
                    //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                    //}
                //}

                //Debug.Log("Moving Towards Waypoint");
                transform.position = Vector3.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.3f);


                if (Vector3.Distance(transform.position, moveSpot.position) < 0.2f)
                {
                    anim.SetBool("isIdle", true);
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isAttacking", false);
                    if (waitTime <= 0)
                    {
                        moveSpot.position = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minY, maxY));
                        waitTime = startWaitTime;
                        walkingTime = 0;
                        anim.SetBool("isWalking", true);
                        anim.SetBool("isIdle", false);
                        anim.SetBool("isAttacking", false);
                    }
                    else
                    {
                        waitTime -= Time.deltaTime;
                    }
                }
            }
        }
	}



    IEnumerator deleteItem(GameObject itemToDisappear)
    {
        yield return new WaitForSeconds(3);

        Destroy(itemToDisappear);
    }
}
