using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class ChooseUI : MonoBehaviour
{
    public Image Lock;
    public Image CardBG;
    public Image CardPath;
    public Image CardID;
    Image LevelCard;
    string CardImage;
    string background;

    bool isTranslucent=false;
    bool isLock;

    public bool IsLock
    {
        get
        {
            return isLock;
        }
        set
        {
            isLock = value;
            Lock.gameObject.SetActive(value);
        }
    }

    public bool IsTranslucent
    {
        get
        {
            return isTranslucent;
        }

        set
        {
            isTranslucent = value;
            Image[] image = { Lock, CardBG, CardPath, CardID };
            foreach(var n in image)
            {
                Color c = n.color; 
                c.a = value ? 0.3f : 1f;
                n.color = c;
            }
        }
    }

    
    

    private void Start()
    {
        LevelCard = GetComponent<Image>();
        ImportCard();
    }

    public void ImportCard()
    {
        LoadMap(name);
        IsLock = IsLook.isLook.GetLook(name);
    }
    public void LoadMap(string Path)
    {
        FileStream fs = new FileStream(Application.dataPath + "/" + Path + ".csv", FileMode.Open, FileAccess.Read);
        StreamReader read = new StreamReader(fs, Encoding.Default);
        if (read == null)
        {
            Debug.Log("读取失败");
            return;
        }

        string[] row = read.ReadLine().Replace("\r", "").Split(new char[] { '\n' });
        for (int i = 0; i < row.Length; i++)
        {
            string[] line = row[i].Split(new char[] { ',' });
            if (line.Length < 6)
            {
                continue;
            }
            int j = 0;
            string name = line[j++];
            CardImage = line[j++];
            background = line[j++];
            read.Close();
        }
        SetBG(CardImage, background);
    }

    public void SetBG(string BGName, string roadName)
    {
        CardBG.sprite = Resources.Load<Sprite>("Res/Maps/" + BGName);
        CardPath.sprite = Resources.Load<Sprite>("Res/Maps/" + roadName);
    }

}
