using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dele : MonoBehaviour {
    [HideInInspector]
    public Tower tow;

    public void Load(Tower tower)
    {
        tow = tower;
    }




    private void OnMouseDown()
    {
        tow.Dele();
    }
}
