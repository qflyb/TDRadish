using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

class Card
{
    public string Name;
    public string ImagePath;
}

public class CardUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool MouthClick = false;
    Vector3 mou;
    public Transform[] cardall;//包含锁图片
    List<Transform> card = new List<Transform>();//不包含锁图片
    public Image box;
    Transform LevelCard;
    string LevelName = "Level1";
    public Button StartButton;
    


    void Awake()
    {
        LevelCard = GetComponent<Transform>();
        cardall = GetComponentsInChildren<Transform>();
        foreach (var n in cardall)
        {
            if (n.CompareTag("Card"))
            {
                card.Add(n);
            }
        }
        for (int i = 1; i < card.Count; i++)
        {
            card[i].GetComponent<ChooseUI>().IsTranslucent = true;
        }
        card[0].GetComponent<ChooseUI>().IsTranslucent = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MouthClick = true;
        var mouseposition = Input.mousePosition;
        mou = mouseposition - transform.position;
        mou.y = 0;
    }

    void Update()
    {
        if (MouthClick)
        {
            var mouseposition = Input.mousePosition;
            mouseposition.y = transform.position.y;
            transform.position = mouseposition - mou;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        MouthClick = false;
        for (int i = 0; i < card.Count; i++)
        {
            for (int j = 0; j < card.Count - 1; j++)
            {
                if (Vector3.Distance(box.transform.position, card[j].position) > Vector3.Distance(box.transform.position, card[j + 1].position))
                {
                    var x = card[j + 1];
                    card[j + 1] = card[j];
                    card[j] = x;
                }
            }
        }
        for(int i = 1;i<card.Count;i++)
        {
            card[i].GetComponent<ChooseUI>().IsTranslucent = true;            
        }
        Transform Max = card[0];
        card[0].GetComponent<ChooseUI>().IsTranslucent = false;
        var Move = Max.position - box.transform.position;
        Move.y = 0;
        var moveto = LevelCard.position - Move;
        LevelCard.DOMove(moveto, 0.5f);
        LevelName = card[0].name;

        if (card[0].GetComponent<ChooseUI>().IsLock)
        {
            StartButton.gameObject.SetActive(false);
        }
        else if (!card[0].GetComponent<ChooseUI>().IsLock)
        {
            StartButton.gameObject.SetActive(true);
        }
    }

    public void ReturnLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(LevelName);
    }

    public void ReturnMap()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartLevel");
    }

}
