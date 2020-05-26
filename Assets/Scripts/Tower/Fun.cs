using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fun : Tower {

    public override void Start()
    {
        base.Start();
        Money = 160;
        //Plan.GetComponent<Plan>().Load(this);
        AttackCD = map.tower["Fun"].AttackCD;
        Scope = map.tower["Fun"].Scope;
        LevelUp1 = map.tower["Fun"].LevelUp1;
        LevelUp2 = map.tower["Fun"].LevelUp2;
        Dele1 = map.tower["Fun"].Dele1;
        Dele2 = map.tower["Fun"].Dele2;
        Dele3 = map.tower["Fun"].Dele3;

    }
    public int BuCount = 6;

    public override void attack()
    {
        for(int i =0;i<BuCount;i++)
        {
            float R = (Mathf.PI * 2f / BuCount) * i;
            Vector3 vect = new Vector3(Mathf.Cos(R), Mathf.Sin(R), 0f);

            var bullet = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<FunBullet>();
            bullet.Level = Level;
            bullet.vect = vect;

        }
        Anima.SetTrigger("IsAttack");
        attackTime = AttackCD;
    }

    public override void Update()
    {
        base.Update();
        transform.Rotate(transform.forward * 180 * Time.deltaTime);
    }

    public override void SearchMonster()
    {
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


}
