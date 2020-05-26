using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    public float Speed;
    public float HP;
    public float MaxHp;
    public int Gold;
    bool die;

    public bool live ;

    Vector3 GO;
    Vector3 go;

    StartMap map;
    Queue<Vector3> path ;

    Animator Anima;

    public bool Die
    {
        get { return die; }
        set { die = value; }
    }

    private void Awake()
    {
        Anima = GetComponent<Animator>();
        map = FindObjectOfType<StartMap>();
        MaxHp = HP;
    }



    private void Start()
    {
    }

    public void MovePath()
    {
        path = new Queue<Vector3>();
        foreach (var n in map.path)
        {
            path.Enqueue(map.GetPosition(n));

        }
        GO = path.Dequeue();
        go = (GO - transform.position).normalized;

    }

    private void Update()
    {
        if(!GameMode.GM.GameState)
        {
            return;
        }
        Move(go);
        if (HP <= 0)
        {
            Dead();
        }
    }
    

    public void BeHit(float hit)
    {
        if (die)
            return;
        HP -= hit;
    }

    public void Dead()
    {
        if (die || !live)
            return;
        die = true;
        live = false;
        Anima.SetTrigger("Die");
        HP = MaxHp;
        GameMode.GM.AddGold(Gold);
        Invoke("SetAct", 0.5f);
    }
    
    public void SetAct()
    {
        gameObject.SetActive(false);
    }


    public void Move(Vector3 vect)
    {
        transform.Translate(vect * Speed * Time.deltaTime);
        if(Vector3.Distance(GO,transform.position)<0.05f)
        {
            if(path.Count<1)
            {
                gameObject.SetActive(false);
                live = false;
                GameMode.GM.RadishHp -= 1;
                GameMode.GM.radish.BiHit();
                return;
            }
            transform.position = GO;
            GO = path.Dequeue();
            go = (GO - transform.position).normalized;
        }
    }

  

}
