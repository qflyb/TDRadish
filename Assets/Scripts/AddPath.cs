using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPath : MonoBehaviour {

    UIDropdown ui;
    MapManager Map;

    private void Start()
    {
        Map = FindObjectOfType<MapManager>();
        ui = FindObjectOfType<UIDropdown>();
    }

    private void OnMouseDrag()
    {
        if(ui.Custom)
        transform.position = MapManager.MouthDown();
    }

    private void OnMouseUp()
    {
        if (ui.Custom)
        {
            Grid mouthgird = MapManager.MouthGrid(MapManager.MouthDown());
            transform.position = MapManager.GetPosition(mouthgird);
            Map.NowPath = gameObject;
        }
            
    }

}
