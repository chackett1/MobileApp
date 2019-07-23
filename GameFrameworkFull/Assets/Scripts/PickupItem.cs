using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickupItem : MonoBehaviour
{
    public PlayerController player;
    public GameObject attractedTo;
    private float strengthOfAttraction = 40;
    private bool connectedItem = false;
    private bool pull = false;
    private string[] tags = { "Player", "PlayerLayer1", "PlayerLayer2", "PlayerLayer3" };
    private int tagIndex = 1;
    private bool layerFlag = false;
    private int requiredScore;
    //private bool hitFlag = false;

    // 1. If an object touches the ball or item on appropriate layer, attach item
    // 2. If an object touches an item that is an outter layer, pull item
    // 3. If an item is being pulled for too long, increase layer

    void Start()
    {
        if (GetComponentInChildren<Collider>().bounds.size.ToString().Contains("14."))
        {
            requiredScore = 280;
        }
        else if (GetComponentInChildren<Collider>().bounds.size.ToString().Contains("13."))
        {
            requiredScore = 260;
        }
        else if (GetComponentInChildren<Collider>().bounds.size.ToString().Contains("12."))
        {
            requiredScore = 240;
        }
        else if (GetComponentInChildren<Collider>().bounds.size.ToString().Contains("11."))
        {
            requiredScore = 220;
        }
        else if (GetComponentInChildren<Collider>().bounds.size.ToString().Contains("10."))
        {
            requiredScore = 200;
        }
        else if (GetComponentInChildren<Collider>().bounds.size.ToString().Contains("9."))
        {
            requiredScore = 180;
        }
        else if (GetComponentInChildren<Collider>().bounds.size.ToString().Contains("8."))
        {
            requiredScore = 160;
        }
        else if (GetComponentInChildren<Collider>().bounds.size.ToString().Contains("7."))
        {
            requiredScore = 140;
        }
        else if (GetComponentInChildren<Collider>().bounds.size.ToString().Contains("6."))
        {
            requiredScore = 120;
        }
        else if (GetComponentInChildren<Collider>().bounds.size.ToString().Contains("5."))
        {
            requiredScore = 100;
        }
        else if (GetComponentInChildren<Collider>().bounds.size.ToString().Contains("4."))
        {
            requiredScore = 80;
        }
        else if (GetComponentInChildren<Collider>().bounds.size.ToString().Contains("3."))
        {
            requiredScore = 60;
        }
        else if (GetComponentInChildren<Collider>().bounds.size.ToString().Contains("2."))
        {
            requiredScore = 40;
        }
        else if (GetComponentInChildren<Collider>().bounds.size.ToString().Contains("1."))
        {
            requiredScore = 20;
        }
        else if (GetComponentInChildren<Collider>().bounds.size.ToString().Contains("0."))
        {
            requiredScore = 0;
        }
        else
        {
            Debug.Log("OUT OF BOUNDS (0-3): " + GetComponentInChildren<Collider>().bounds.size.ToString());
            requiredScore = 100;
        }
    }

    void FixedUpdate()
    {
        if (pull == true)
        {
            Vector3 direction = attractedTo.transform.position - transform.position;
            GetComponent<Rigidbody>().AddForce(strengthOfAttraction * direction);
            Debug.Log("Pull");
        }
    }

    public void Reset()
    {
        connectedItem = false;
        pull = false;
        tagIndex = 1;
        layerFlag = false;
    }

    public bool ReadyToPickup()
    {
        if ((player.getCount() - requiredScore) >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        /*if (transform.parent == null && connectedItem == true)
        {
            Reset();
            //hitFlag = true;
            Vector3 direction = attractedTo.transform.position - transform.position;
            GetComponent<Rigidbody>().AddForce(direction * -100000);
        }*/

        //Debug.Log(collision.gameObject);
        if ((!connectedItem && (player.getCount() - requiredScore) >= 0 ) && collision.gameObject.tag.Equals("Player"))
        {
            if (collision.collider.gameObject.tag.Equals(tags[tagIndex - 1]) || tag.Equals("Enemy"))
            {
                AttachItem(collision);
            }
            else if (collision.collider.gameObject.tag.Equals(tags[0]) || collision.collider.gameObject.tag.Equals(tags[1]) || collision.collider.gameObject.tag.Equals(tags[2]) || collision.collider.gameObject.tag.Equals(tags[3]))
            {
                if (layerFlag == false)
                {
                    if (tag.Equals("Enemy"))
                    {
                        Debug.Log("Stop Walking");
                        Patrol.StopWalking();
                    }
                    pull = true;
                    //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    //GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezeRotationX;
                    //GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezeRotationZ;
                    //GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
                    layerFlag = true;
                    StartCoroutine(IncreaseLayerAfterTimeout());
                }
            }
        }
    }

    void AttachItem(Collision collision)
    {
        connectedItem = true;
        pull = false;

        Destroy(GetComponent<Rigidbody>());

        gameObject.transform.parent = collision.transform;

        player.SetCountText();

        gameObject.tag = tags[tagIndex];

        player.setMostRecentItem(gameObject.name);
    }

    IEnumerator IncreaseLayerAfterTimeout()
    {
        yield return new WaitForSeconds(3);

        if (pull == true)
        {
            tagIndex++;
            if (tagIndex == 4)
            {
                tagIndex = 1;
            }
            //Debug.Log(tagIndex);
        }
        layerFlag = false;
    }

    /*IEnumerator waitForItemToFallOff()
    {
        yield return new WaitForSeconds(1);
        hitFlag = false;
    }*/


    /* ---------------- old stuff ------
    void Start()
    {
    }
    
    void FixedUpdate()
    {
        if (pull == true)
        {
            Vector3 direction = attractedTo.transform.position - transform.position;
            GetComponent<Rigidbody>().AddForce(strengthOfAttraction * direction);
            Debug.Log("Pull");
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        rb = collision.gameObject.GetComponent<Rigidbody>();

        Collider myCollider = collision.contacts[0].thisCollider;
        Debug.Log(myCollider);

        if (collision.gameObject.tag.Equals("Player") && !hasJoint)
        {
            //Debug.Log(collision.contacts[1].thisCollider);

            if (player.getCount() < 10)
            {
                if (gameObject.tag.Equals("Size1"))
                {
                    StartCoroutine(waitAndHinge(collision));
                    pull = true;
                }
            }
            else if (player.getCount() < 80)
            {
                if (gameObject.tag.Equals("Size1") || gameObject.tag.Equals("Size2"))
                {
                    StartCoroutine(waitAndHinge(collision));
                    pull = true;
                }
            }
            else
            {
                if (gameObject.tag.Equals("Size1") || gameObject.tag.Equals("Size2") || gameObject.tag.Equals("Size3"))
                {
                    StartCoroutine(waitAndHinge(collision));
                    pull = true;
                }
            }
        }
        else if (rb != null && !hasJoint)
        {
            if (player.getCount() < 10)
            {
                if (gameObject.tag.Equals("Size1"))
                {
                    pull = true;
                }
            }
            else if (player.getCount() < 80)
            {
                if (gameObject.tag.Equals("Size1") || gameObject.tag.Equals("Size2"))
                {
                    pull = true;
                }
            }
            else
            {
                if (gameObject.tag.Equals("Size1") || gameObject.tag.Equals("Size2") || gameObject.tag.Equals("Size3"))
                {
                    pull = true;
                }
            }
        }
    }
    IEnumerator waitAndHinge(Collision collision)
    {
        yield return new WaitForSeconds(1);
        if (!hasJoint)
        {
            //gameObject.layer = 8;
            //Physics.IgnoreLayerCollision(5, 8);

            hasJoint = true;
            pull = false;

            Destroy(GetComponent<Rigidbody>());

            gameObject.transform.parent = collision.transform;
            //object1 is now the child of object2
            
            //gameObject.AddComponent<FixedJoint>();
            //gameObject.GetComponent<FixedJoint>().connectedBody = collision.rigidbody;

            //gameObject.GetComponent<Rigidbody>().useGravity = false;

            player.SetCountText();
        }
    }

    /*
public float pullRadius = 3;
public float pullForce = 5000;

public void FixedUpdate()
{
    foreach (Collider collider in Physics.OverlapSphere(transform.position, pullRadius)) {
        // calculate direction from target to me
        Vector3 forceDirection = transform.position - collider.transform.position;

        // apply force on target towards me
        GetComponent<Rigidbody>().AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);
        //collider.rigidbody.AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);
    }
}*/
    /*
    //Debug.Log("");
    private Rigidbody rb;
    private float force = 500;
    private bool addingForce = false;
    private bool flag = false;
    Collision c;
    void FixedUpdate()
    {
        if (addingForce)
        {
            if (flag)
            {
                StartCoroutine(waitForForce());
                flag = false;
            }
            Vector3 dir = c.contacts[0].point - transform.position;
            dir = dir.normalized;
            GetComponent<Rigidbody>().AddForce(dir * force);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.GetComponent<Rigidbody>() != null && gameObject.tag.Equals("Size1") && !hasJoint)
        if (collision.gameObject.tag == "Player")
        {
            addingForce = true;
            flag = true;
            c = collision;     

            StartCoroutine(waitAndHinge(collision));
        }
    }

    IEnumerator waitForForce()
    {
        yield return new WaitForSeconds(3);
        addingForce = false;

    }
    IEnumerator waitAndHinge(Collision collision)
    {
        yield return new WaitForSeconds(3);
        if (!hasJoint)
        {
            gameObject.AddComponent<FixedJoint>();
            gameObject.GetComponent<FixedJoint>().connectedBody = collision.rigidbody;
            hasJoint = true;
            Debug.Log("hinged");
        }
    }*/
}
