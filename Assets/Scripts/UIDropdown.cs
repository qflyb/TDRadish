using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIDropdown : MonoBehaviour
{
    Dropdown Drop;
    public Text Level;
    public Text BG;
    public Text Path;

    string level;
    string path;
    string BGChoose;
    MapManager map;

    public  bool Bulding;
    public bool Custom;

    public RectTransform LevelUIPos;
    public RectTransform PathUIPos;
    public RectTransform CustomPath;
    public RectTransform UIGO;

    private void Awake()
    {
        Drop = GetComponent<Dropdown>();
        map = FindObjectOfType<MapManager>();

        Tweener uigo = LevelUIPos.DOLocalMove(UIGO.localPosition, 0.5f).OnPlay(() => { LevelUIPos.gameObject.SetActive(true); });
        uigo.SetAutoKill(false);
        uigo.Pause();
        Tweener Pathuigo = PathUIPos.DOLocalMove(UIGO.localPosition, 0.5f).From().OnRewind(() => { PathUIPos.gameObject.SetActive(false); }).OnPlay(() => { PathUIPos.gameObject.SetActive(true); });
        Pathuigo.SetAutoKill(false);
        Pathuigo.Pause();
        Tweener custompath = CustomPath.DOLocalMove(UIGO.localPosition, 0.5f).From().OnRewind(() => { CustomPath.gameObject.SetActive(false); }).OnPlay(() => { CustomPath.gameObject.SetActive(true); });
        custompath.SetAutoKill(false);
        custompath.Pause();
    }

    private void Update()
    {
        if (level != Level.text)
            level = Level.text;
        if (BGChoose != BG.text)
        {
            BGChoose = BG.text;
            map.ChooseBG(BGChoose, BG.text);
        }
        if(path != Path.text)
        {
            path=Path.text;
            map.pathChoose(path);
        }
    }

    public void SaveMap()
    {
        int n = level[level.Length - 1];
        Debug.Log(level[level.Length - 1]);
        map.SaveMap(level, (LevelName)GetLevelN(level));
    }
    public void LoadMap()
    {
        map.LoadMap(level);
    }

    public int GetLevelN(string path)
    {
        switch (path)
        {
            case "Level1":
                return 0;
            case "Level2":
                return 1;
            case "Level3":
                return 2;
            case "Level4":
                return 3;
            case "Level5":
                return 4;
        }
        return -1;
    }

    public void returnChoose()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartLevel");
    }

    public void EditorPath() //编辑路径云
    {
        if (Custom)
        {
            Custom = false;
            PathUIPos.DOPlayForward();
            CustomPath.DOPlayBackwards();
        }
        else
        {
            Custom = true;
            PathUIPos.DOPlayBackwards();
            CustomPath.DOPlayForward();
        }
    }

    public void ChangeUI()
    {
        if(Bulding)
        {
            Bulding = false;
            LevelUIPos.DOPlayBackwards();
            PathUIPos.DOPlayBackwards();
        }
        else
        {
            Bulding = true;
            LevelUIPos.DOPlayForward();
            PathUIPos.DOPlayForward();
        }
    }

}
