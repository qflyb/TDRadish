using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Speed
{
    speed1,
    speed2
}


public class UIX : MonoBehaviour {

    public Image Pause;
    bool GameState = true;
    public bool gameState
    {
        get { return GameState; }
    }
    public GameObject Timing; // 开始倒计时
    public Image StartTime;
    int TiTime = 3;
    public GameObject Choose;//菜单选择
    bool OnChoose = false;
    public GameObject Wave; //显示波数
    public Text nowWave;
    public Button PauseButton;
    public Button StartButton;
    public Image BGgray;//使背景变灰
    //控制倍数按钮
    public Button Speed1B; 
    public Button Speed2B;

    public Text Golds; //金币

    public GameObject Fire;

    public GameObject WinUI;
    public GameObject LostUI;

    public Text LostWave;

    Speed speed;
    IEnumerator StartGame() //开始游戏前倒计时控制
    {
        GameState = false;
        BGgray.gameObject.SetActive(true);
        for (int i=0;i<4;i++)
        {
            if (TiTime == 3)
            {
                
                StartTime.sprite = Resources.Load<Sprite>("Res/Scene/Items02-hd.pvr/countdown_01");
            }
            else if(TiTime == 2)
            {
                StartTime.sprite = Resources.Load<Sprite>("Res/Scene/Items02-hd.pvr/countdown_02");
            }
            else if(TiTime == 1)
            {
                StartTime.sprite = Resources.Load<Sprite>("Res/Scene/Items02-hd.pvr/countdown_03");
            }
            else if(TiTime == 0)
            {
                Timing.SetActive(false);
                GameState = true;
                BGgray.gameObject.SetActive(false);
            }
            TiTime--;
            yield return new WaitForSeconds(1);
        }
        
    }

    private void Start()
    {
        StartCoroutine("StartGame");
    }

    public void Speed1() //一倍速
    {
        if (OnChoose) return;
        speed = Speed.speed2;
        Time.timeScale = 2;
        Speed1B.gameObject.SetActive(false);
        Speed2B.gameObject.SetActive(true);
    }

    public void Speed2() //二倍速
    {
        if (OnChoose) return;

        speed = Speed.speed1;
        Time.timeScale = 1;
        Speed1B.gameObject.SetActive(true);
        Speed2B.gameObject.SetActive(false);
    }

    void Update()
    {
        Fire.transform.Rotate(Vector3.forward, Time.deltaTime * 360,Space.World);
    }

    public void ShowGame() //继续或暂停时界面显示
    {
        if(GameMode.GM.GameState)
        {
            GameMode.GM.GameState = false;
        }
        else
            GameMode.GM.GameState = true;
        if (OnChoose) return;

        if (GameState)
        {
            Pause.gameObject.SetActive(true);
            Wave.SetActive(false);
            StartButton.gameObject.SetActive(true);
            PauseButton.gameObject.SetActive(false);
            BGgray.gameObject.SetActive(true);
            GameState = false;
        }
        else
        {
            Pause.gameObject.SetActive(false);
            Wave.SetActive(true);
            PauseButton.gameObject.SetActive(true);
            StartButton.gameObject.SetActive(false);
            BGgray.gameObject.SetActive(false);
            GameState = true;
        }
    }

    public void ContinueGame()
    {
        ShowGame();
    }

    public void PauseGame()
    {
        ShowGame();
    }

    public void GameMenu()   //游戏菜单
    {
        if (OnChoose)
        {
            if (GameState)
                BGgray.gameObject.SetActive(false);
            OnChoose = false;
            Choose.SetActive(false);
        }
        else
        {
            if (GameState)
                BGgray.gameObject.SetActive(true);
            OnChoose = true;
            Choose.SetActive(true);
        }
    }

    public void Win()
    {
        WinUI.SetActive(true);
    }
    public void Lose()
    {
        LostUI.SetActive(true);
    }

    public void GoGame()
    {
        BGgray.gameObject.SetActive(false);
        OnChoose = false;
        Choose.SetActive(false);
    }
    public void selectLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("choiceLevel");
    }

    public void ResteLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

}
