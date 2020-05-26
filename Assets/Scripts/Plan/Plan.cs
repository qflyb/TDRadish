using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plan : MonoBehaviour {

    public SpriteRenderer LevelUp;
    public SpriteRenderer dele;
    public int Level;

    public Dele delel;
    public LevelUp levelup;

    private void Start()
    {

    }

    public void HaveGold()
    {

    }

    public void Load(Tower tower)
    {
        delel.Load(tower);
        levelup.Load(tower);
    }


}
