using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControlScript : MonoBehaviour {

    int levelPassed;
    int sceneIndex;

    public GameObject starWon1;
    public GameObject starLost1;
    public GameObject starWon2;
    public GameObject starLost2;
    public GameObject starWon3;
    public GameObject starLost3;

    // Use this for initialization
    void Start () {
        levelPassed = PlayerPrefs.GetInt("LevelPassed");
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void YouWin()
    {
        if (levelPassed < sceneIndex)
        {
            PlayerPrefs.SetInt("LevelPassed", sceneIndex);
        }
    }
}
