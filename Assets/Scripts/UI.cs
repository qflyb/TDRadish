using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UI : MonoBehaviour
{

    public GameObject Shit;
    public GameObject Bottle;
    public GameObject Fan;

    public Button openBuilding;
    public Button closeBuilding;

    //显示放塔点相关的东西
    [HideInInspector]
    public List<GameObject> TowerPoint = new List<GameObject> ();
    public GameObject TowerP;

    bool Towrn;
    GameObject TowrnAdd; //创建塔
    bool Building = false;

    //展开和关闭UI
    public RectTransform MovePos;
    public RectTransform BottenPos;
    public RectTransform ShitPos;
    public RectTransform FanPos;

    StartMap Map;
   



    void Start()
    {
        Map = FindObjectOfType<StartMap>();
        Tweener botten = BottenPos.DOLocalMove(MovePos.localPosition, 0.5f).From().OnRewind(() => { BottenPos.gameObject.SetActive(false); }).OnPlay(() => { BottenPos.gameObject.SetActive(true); });
        botten.SetAutoKill(false);
        botten.Pause();
        Tweener shit = ShitPos.DOLocalMove(MovePos.localPosition, 0.5f).From().OnRewind(() => { ShitPos.gameObject.SetActive(false); }).OnPlay(() => { ShitPos.gameObject.SetActive(true); }); ;
        shit.SetAutoKill(false);
        shit.Pause();
        Tweener fan = FanPos.DOLocalMove(MovePos.localPosition, 0.5f).From().OnRewind(() => { FanPos.gameObject.SetActive(false); }).OnPlay(() => { FanPos.gameObject.SetActive(true); }); ;
        fan.SetAutoKill(false);
        fan.Pause();
        CanTowrn();
    }

    

    void Update()
    {
        ChickDown();

        HaveMoney();
    }

    public void HaveMoney()//判断是否有足够的钱进行建塔
    {
        if (GameMode.GM.gold - 100 < 0)
            BottenPos.gameObject.GetComponent<Image>().color = Color.gray;
        else
            BottenPos.gameObject.GetComponent<Image>().color = Color.white;

        if (GameMode.GM.gold - 160 < 0)
            FanPos.gameObject.GetComponent<Image>().color = Color.gray;
        else
            FanPos.gameObject.GetComponent<Image>().color = Color.white;
    }

    public void CanTowrn()
    {
        foreach(var path in Map.Towrnpath)
        {
            var tow = Instantiate(TowerP, Map.GetPosition(path), Quaternion.identity);
            tow.SetActive(false);
            TowerPoint.Add(tow);
        }
    }
    public void ShowTowrnP(bool b)
    {
        foreach (var tow in TowerPoint)
        {
            tow.SetActive(b);
        }
    }

    public void ChickDown()
    {
        if (Towrn)
        {
            TowrnAdd.transform.position = MapManager.MouthDown();
            if (Input.GetMouseButtonDown(0))
            {
                Grid mouthgird = MapManager.MouthGrid(MapManager.MouthDown());
                bool Cantowrn = false;
                foreach (var path in Map.Towrnpath)
                {
                    if(path.x ==mouthgird.x&&path.y ==mouthgird.y&&!path.CanTower)
                    {
                        Cantowrn = true;
                        path.CanTower = true;
                    }
                }
                if (!Cantowrn)
                    return;

                GameMode.GM.gold -= TowrnAdd.GetComponent<Tower>().Money;
                GameMode.GM.ui.Golds.text = "" + GameMode.GM.gold;
                TowrnAdd.transform.position = MapManager.GetPosition(mouthgird);
                TowrnAdd.GetComponent<Tower>().isBuild = true;
                TowrnAdd.GetComponent<Tower>().Plan.transform.position = TowrnAdd.GetComponent<Tower>().transform.position;
                Towrn = false;
                ShowTowrnP(false);
            }
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(TowrnAdd);
                Towrn = false;
                ShowTowrnP(false);
            }
        }
    }

  

    public void OpenBuilding()
    {
        BottenPos.DOPlayForward();
        ShitPos.DOPlayForward();
        FanPos.DOPlayForward();
        closeBuilding.gameObject.SetActive(true);
        openBuilding.gameObject.SetActive(false);
    }

    public void CloseBuilding()
    {
        BottenPos.DOPlayBackwards();
        ShitPos.DOPlayBackwards();
        FanPos.DOPlayBackwards();

        openBuilding.gameObject.SetActive(true);
        closeBuilding.gameObject.SetActive(false);
    }

    public void ChickBottle()
    {
        if(GameMode.GM.gold>=100)
        {
            ShowTowrnP(true);
            Towrn = true;
            TowrnAdd = Instantiate(Bottle, MapManager.MouthDown(), Quaternion.identity);
        }
        
    }

    public void ChickShit()
    {
        if(GameMode.GM.gold >= 120)
        {
            ShowTowrnP(true);
            Towrn = true;
            TowrnAdd = Instantiate(Shit, MapManager.MouthDown(), Quaternion.identity);
        }
        
    }

    public void ChickFan()
    {
        if (GameMode.GM.gold >= 160)
        {
            ShowTowrnP(true);
            Towrn = true;
            TowrnAdd = Instantiate(Fan, MapManager.MouthDown(), Quaternion.identity);
        }
    }


  
}
