using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleBullet : Bullet
{
    [HideInInspector]
    public Monster monster; //目标
    Vector3 Direction; //方向
    
    public void Start()
    {        
        Speed = 10f;
        attack = 50;
        Anima = GetComponent<Animator>();
    }



    private void Update()
    {
        Move();
    }

    public override void Move()
    {
        if (IsBoom)
            return;
        if (monster != null)
        {
            Direction = (monster.transform.position - transform.position).normalized;
            LookAt();
            transform.Translate(Direction * Speed * Time.deltaTime, Space.World);
            if (Vector3.Distance(transform.position, monster.transform.position) < 0.1f)
            {
                if (!IsBoom)
                {
                    monster.BeHit(Attack);
                    boom();
                }
            }
        }

        if (!IsBoom && !GameMode.GM.MapRect.Contains(transform.position))
            boom();

    }

    public void LookAt()
    {
        float angle = Mathf.Atan2(Direction.y, Direction.x);
        Vector3 eulerAngles = transform.eulerAngles;
        eulerAngles.z = angle * Mathf.Rad2Deg - 90;
        transform.eulerAngles = eulerAngles;
    }


    

}
