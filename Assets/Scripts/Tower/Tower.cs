using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType
{
    Bottle,
    Shit,
    Fun
}

public class Tower : MonoBehaviour
{
    public GameObject Chickplan;
    [HideInInspector]
    public GameObject Plan;
    public int Attack;
    public float AttackCD;
    public float attackTime;
    public float Scope;
    [HideInInspector]
    public bool isBuild;
    public AudioClip ATKSource;

    public int Money;

    public int Level; //塔现在的等级

    bool OpenPlan = false;

    public bool IslevelUp; //是否能升级

    //塔升级和销毁
    public int LevelUp1;
    public int LevelUp2;

    public int Dele1;
    public int Dele2;
    public int Dele3;

    public GameObject Bullet;
    [HideInInspector]
    public Monster monster;
    [HideInInspector]
    public StartMap map;
    [HideInInspector]
    public Animator Anima;
    [HideInInspector]
    public Queue<Monster> mons = new Queue<Monster>();
    

    public virtual void Update()
    {
        if (!isBuild||!GameMode.GM.GameState)
        {
            return;
        }
        attackTime -= Time.deltaTime;
        SearchMonster();

        if (isBuild && OpenPlan)
        {
            if(Input.GetMouseButtonDown(0))
            {
                OpenPlan = false;
                Plan.SetActive(false);
            }
        }
    }
    private void LateUpdate()
    {
        
    }
    public int Nowlevel()
    {
        switch (Level)
        {
            case 1:
                return LevelUp1;
            case 2:
                return LevelUp2;
        }
        return 0;
    }

    public virtual void LevelUp()
    {
        switch (Level)
        {
            case 1:
                {
                    if (GameMode.GM.gold - LevelUp1 < 0)
                        return;
                    Level = 2;
                    GameMode.GM.gold -= LevelUp1;
                    GameMode.GM.ui.Golds.text = "" + GameMode.GM.gold;
                    gameObject.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
                    OpenPlan = false;
                    Plan.SetActive(false);
                    break;
                }
            case 2:
                {
                    if (GameMode.GM.gold - LevelUp2 < 0)
                        return;
                    Level = 3;
                    GameMode.GM.gold -= LevelUp2;
                    GameMode.GM.ui.Golds.text = "" + GameMode.GM.gold;
                    gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
                    Plan.SetActive(false);
                    OpenPlan = false;
                    break;
                }
        }
    }

    public virtual void Dele()
    {
        switch (Level)
        {
            case 1:
                {
                    GameMode.GM.gold += Dele1;
                    GameMode.GM.ui.Golds.text = ""+GameMode.GM.gold;
                    Destroy(Plan);
                    Destroy(gameObject);
                    break;
                }
            case 2:
                {
                    GameMode.GM.gold += Dele2;
                    GameMode.GM.ui.Golds.text = "" + GameMode.GM.gold;
                    Destroy(Plan);
                    Destroy(gameObject);
                    break;
                }
            case 3:
                {
                    GameMode.GM.gold += Dele3;
                    GameMode.GM.ui.Golds.text = "" + GameMode.GM.gold;
                    Destroy(Plan);
                    Destroy(gameObject);
                    break;
                }
        }
    }

    public virtual void Start()
    {
        Level = 1;
        Anima = GetComponent<Animator>();
        Plan = Instantiate(Chickplan, transform.position, Quaternion.identity);
        Plan.SetActive(false);
        map = FindObjectOfType<StartMap>();
        Scope = 4f;
    }

    private void OnMouseDown()
    {
        
        if (isBuild && !OpenPlan)
        {
            Invoke("Setactive",0.2f);
            Plan.SetActive(true);
        }
    }

    public void Setactive()
    {
        OpenPlan = true;
    }


    public virtual void SearchMonster()  //搜寻怪物
    {
        LookAt(monster);

        if (monster == null)
        {
            GameObject[] monsters = GameMode.GM.monsters;

            //搜索离终点最近的怪
            for (int i = 0; i < monsters.Length; i++)
            {
                var m = monsters[i].GetComponent<Monster>();
                if (!m.Die && Vector3.Distance(m.transform.position, transform.position) < Scope)
                {
                    bool IsHave = false;
                    foreach (var n in mons)
                    {
                        if (n == m)
                        {
                            IsHave = true;
                        }
                    }
                    if (!IsHave)
                        mons.Enqueue(m);
                }
            }
            if (mons.Count > 0)
            {
                monster = mons.Dequeue();
            }

        }

        else
        {
            if (!monster.Die && Vector3.Distance(monster.transform.position, transform.position) < Scope && monster.live)
            {
                if (attackTime < 0)
                {
                    attack();
                }
            }
            else
            {
                monster = null;
            }
        }
    }

    void LookAt(Monster target)
    {
        //if (mons.Count == 0 && target == null)
        //{
        //    Vector3 eulerAngles = transform.eulerAngles;
        //    eulerAngles.z = 0;
        //    transform.eulerAngles = eulerAngles;
        //}
        if (target != null)
        {
            Vector3 vec = (target.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(vec.y, vec.x);
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.z = angle * Mathf.Rad2Deg - 90;
            transform.eulerAngles = eulerAngles;
        }
    }


    public virtual void attack()
    {
        var bullet = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<BottleBullet>();
        bullet.monster = monster;
        bullet.Level = Level;
        bullet.LookAt();
        Anima.SetTrigger("IsAttack");
        AudioSource.PlayClipAtPoint(ATKSource,transform.position);
        attackTime = AttackCD;
    }

   

}
