using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour {
    
    public static GameMode GM;
    [HideInInspector]
    public bool GameState = true;
    [HideInInspector]
    public  UIX ui;
    public int gold;
    public int nowWave;
    [HideInInspector]
    public GameObject[] monsters;

    public int RadishHp = 10; //萝卜的血量

    public Radish radish;

    public bool MonsterOver;

    public bool Lost = false;

    public Rect MapRect ;//屏幕范围

    void Start ()
    {
        //Object.DontDestroyOnLoad(this.gameObject);
        radish = FindObjectOfType<Radish>();
        nowWave = 0;
        gold = 100;
        ui = FindObjectOfType<UIX>();
        ui.Golds.text = "" + gold;
        GM = this;
    }
	


	void Update () {

        monsters= GameObject.FindGameObjectsWithTag("Monster");
        if (RadishHp <= 0)
            radish.Die();

        if (Lost)
        {
            ui.LostWave.text = ui.nowWave.text;
            ui.LostUI.SetActive(true); }
    }

    public void ConsumeGold(int G)
    {
        gold -= G;
    }

    public void AddGold(int G)
    {
        gold += G;
        ui.Golds.text = ""+gold;
    }

}
