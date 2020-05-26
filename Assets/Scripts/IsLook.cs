using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsLook : MonoBehaviour
{
    public static IsLook isLook;

    private void Start()
    {
        Object.DontDestroyOnLoad(this.gameObject);
        isLook = this;
        Level1 = false;
        Level2 = true;
        Level3 = true;
        Level4 = true;
        Level5 = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartLevel");
    }
    
    bool Level1;
    bool Level2;
    bool Level3;
    bool Level4;
    bool Level5;

    public bool GetLook(string LevelName)
    {
        switch (LevelName)
        {
            case "Level1":
                {
                    return Level1;
                }
            case "Level2":
                {
                    return Level2;
                }
            case "Level3":
                {
                    return Level3;
                }
            case "Level4":
                {
                    return Level4;
                }
            case "Level5":
                {
                    return Level5;
                }
        }
        return false;
    }

    public void OpenLook(string LevelName)
    {
        switch (LevelName)
        {
            case "Level1":
                {
                    Level2 = false;
                    break;
                }
            case "Level2":
                {
                    Level3 = false;
                    break;
                }
            case "Level3":
                {
                    Level4 = false;
                    break;
                }
            case "Level4":
                {
                    Level5 = false;
                    break;
                }
        }
    }

}
