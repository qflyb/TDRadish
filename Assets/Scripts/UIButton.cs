using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour {

    public void TurnLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("choiceLevel");
    }

    public void BoosLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("BossLevel");
    }

    public void Building()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelCompile");
    }
}
