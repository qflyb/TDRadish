using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StartUI : MonoBehaviour
{

    public RectTransform Bird;
    public RectTransform birdToPosY;

    public RectTransform cloudPos;
    public RectTransform Cloud1;
    public RectTransform Cloud2;
    float CloudPosX;

    private void Awake()
    {
        Screen.SetResolution(960, 640, false); //强制使窗口的分辨率定位960*640
        CloudPosX =  cloudPos.localPosition.x;
        
    }
    private void Start()
    {
        Tweener birdFly = Bird.transform.DOLocalMove(birdToPosY.localPosition, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        birdFly.SetAutoKill(false);
        Tweener Cloud1M = Cloud1.transform.DOLocalMoveX(CloudPosX, 10).SetLoops(-1).SetEase(Ease.Linear);
        Cloud1M.SetAutoKill(false);
        Tweener Cloud2M = Cloud2.transform.DOLocalMoveX(CloudPosX, 10).SetLoops(-1).SetEase(Ease.Linear);
        Cloud2M.SetAutoKill(false);

    }

    private void Update()
    {
        
    }

   




}
