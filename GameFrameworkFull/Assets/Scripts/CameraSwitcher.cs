using UnityEngine;
using System.Collections;
 
 public class CameraSwitcher : MonoBehaviour
{
    public GameObject camera1;
    public GameObject camera2;
    //public GameObject camera3;
    //public GameObject camera4;

    private int swapper = 0;

    void Start()
    {
        //camera1.SetActive(true);
        //camera2.SetActive(true);

        camera1.SetActive(false);
    }

    void SwitchCamera()
    {
        if (swapper % 2 == 0)
        {
            //camera1.GetComponent<Camera>();
            camera1.SetActive(true);
            camera2.SetActive(false);
        }
        else
        {
            camera1.SetActive(false);
            camera2.SetActive(true);
        }
        swapper++;
    }
}