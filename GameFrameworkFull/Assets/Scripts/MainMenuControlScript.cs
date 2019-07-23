using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuControlScript : MonoBehaviour {

    public Button level02Button, level03Button;
    int levelPassed;

	void Start () {
        levelPassed = PlayerPrefs.GetInt("LevelPassed");
        Debug.Log(levelPassed + "!");
        level02Button.interactable = false;
        level03Button.interactable = false;

        switch(levelPassed) {
            case 2:
                level02Button.interactable = true;
                break;
            case 3:
                level02Button.interactable = true;
                level03Button.interactable = true;
                break;
        }
	}

    public void levelToLoad (string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void resetPlayerPrefs()
    {
        level02Button.interactable = false;
        level03Button.interactable = false;
        PlayerPrefs.DeleteAll();
    }
	
	void Update () {
		
	}
}
