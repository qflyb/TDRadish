using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour {
    [HideInInspector]
    public Tower tow;
    SpriteRenderer Levelup;
    Sprite Levelup1;
    Sprite Levelup2;
    Sprite LevelMax;

    

    private void Start()
    {
        Levelup1 = Resources.Load<Sprite>("Res/Scene/Items02-hd.pvr/upgrade_180");
        Levelup2 = Resources.Load<Sprite>("Res/Scene/Items02-hd.pvr/upgrade_220");
        LevelMax = Resources.Load<Sprite>("Res/Scene/Items02-hd.pvr/upgrade_0_CN");

        //Dele1.sprite = Resources.Load<Sprite>("Res/Scene/upgrade_-180");
        //Dele2.sprite = Resources.Load<Sprite>("Res/Scene/upgrade_-180");
        //Dele3.sprite = Resources.Load<Sprite>("Res/Scene/upgrade_-180");
        Levelup = GetComponent<SpriteRenderer>();
    }

    public void Load(Tower tower)
    {
        tow = tower;
    }

    private void Update()
    {
        GetLevel();
        if (GameMode.GM.gold - tow.Nowlevel() < 0)
            Levelup.color = Color.gray;
        else
            Levelup.color = Color.white;
    }

    private void OnMouseDown()
    {
        tow.LevelUp();
    }

    void GetLevel()
    {
        switch(tow.Level)
        {
            case 1:
                {
                    Levelup.sprite = Levelup1;
                    break;
                }
            case 2:
                {
                    Levelup.sprite = Levelup2;
                    break;
                }
            case 3:
                {
                    Levelup.sprite = LevelMax;
                    break;
                }
        }
    }

}
