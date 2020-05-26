using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public enum LevelName
{
    Level1,
    Level2,
    Level3,
    Level4,
    Level5
}


public class Grid //格子
{
    public float x;
    public float y;

    public bool CanTower = false;

    public Grid(float x, float y)
    {
        this.x = x;
        this.y = y;
    }


}





public class GameLevel
{
    public string name;

    public string CardImage;

    public string background;

    public int Score;

    public List<Grid> TowrnPath = new List<Grid>();

    public List<Grid> path = new List<Grid>();

    public GameLevel(string name, string CardImage, string Background, int score, List<Grid> towrnpath, List<Grid> path)
    {
        this.name = name;
        this.CardImage = CardImage;
        background = Background;
        Score = score;
        TowrnPath = towrnpath;
        this.path = path;
    }

    public GameLevel(string name, string CardImage, string Background)
    {
        this.name = name;
        this.CardImage = CardImage;
        background = Background;
    }
}


public class MapManager : MonoBehaviour
{
    //地图宽高
    static float MapW;
    static float MapH;

    GameLevel level;
    public SpriteRenderer BG;
    public SpriteRenderer Road;

    const int Mapline = 12;
    const int Maprow = 8;

    public bool ShowGrid;

    List<Grid> AllGrid = new List<Grid>();
    List<Grid> path = new List<Grid>();
    List<Grid> Towrnpath = new List<Grid>();

    //格子宽高
    static float GridW;
    static float GridH;

    [HideInInspector]
    public string[] LN;

    public Dictionary<int, Sprite> BGChoose = new Dictionary<int, Sprite>(); //背景资源
    public Dictionary<string, Sprite> PathChoose = new Dictionary<string, Sprite>(); //路径资源

    public Text levelName;

    UIDropdown uidropdown;

    public GameObject start;
    public GameObject end;
    public GameObject holder;

    public GameObject PathCloud; //云预制体
    [HideInInspector]
    public GameObject NowPath; //当前的Path
    public List<GameObject> CustomList = new List<GameObject>();
    public List<Grid> CustomList1 = new List<Grid>();

    public int CustomPath; //自定义路径

    public LineRenderer line;
    public void AddMapN()
    {
        LN = new string[5];
        LN[0] = (LevelName.Level1.ToString());
        LN[1] = (LevelName.Level2.ToString());
        LN[2] = (LevelName.Level3.ToString());
        LN[3] = (LevelName.Level4.ToString());
        LN[4] = (LevelName.Level5.ToString());
    }



    public void LoadBG()
    {
        BGChoose[1] = Resources.Load<Sprite>("Res/Maps/bg1");
        BGChoose[2] = Resources.Load<Sprite>("Res/Maps/bg2");

        PathChoose["路径1"] = Resources.Load<Sprite>("Res/Maps/road1");
        PathChoose["路径2"] = Resources.Load<Sprite>("Res/Maps/road2");
        PathChoose["路径3"] = Resources.Load<Sprite>("Res/Maps/road3");
        PathChoose["路径4"] = Resources.Load<Sprite>("Res/Maps/road4");
        PathChoose["路径5"] = Resources.Load<Sprite>("Res/Maps/road5");

    }

    public void ChooseBG(string BG1, string BG2)
    {
        if (BG.sprite == BGChoose[1])
        {
            BG.sprite = BGChoose[2];
        }
        else
            BG.sprite = BGChoose[1];
    }
    public void pathChoose(string BG1)
    {
        Road.sprite = PathChoose[BG1];
    }

    void Start()
    {
        uidropdown = FindObjectOfType<UIDropdown>();
        LoadBG();
        AddMapN();

    }

    public void ClearTowrn()
    {
        foreach (var n in AllGrid)
        {
            if (n.CanTower)
            {
                n.CanTower = false;
            }
        }
        Towrnpath.Clear();
        foreach (var n in Holder)
        {
            Destroy(n);
        }
        Holder.Clear();
    }

    public void ClearPath()
    {
        path.Clear();
        line.positionCount = 0;
        Destroy(Starts);
    }



    public void SaveMap(string Path, LevelName nameL)
    {
        if (CustomList.Count != 0)
        {
            foreach (var n in CustomList)
            {
                CustomList1.Add(MouthGrid(n.transform.position));
            }
        }

        string fullPath = Application.dataPath + "/" + Path + ".csv";
        FileInfo level = new FileInfo(fullPath);
        if (!level.Directory.Exists)
        {
            Debug.Log("文件创建成功！");
            level.Directory.Create();
        }
        FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
        string data = "";



        data += levelName + ",";
        data += BG.sprite.name + ",";
        data += Road.sprite.name + ",";
        data += "0,";

        data += CustomPath + ",";
        data += CustomList1.Count.ToString() + ",";

        if (CustomList.Count != 0)
        {
            for (int i = 0; i < CustomList1.Count; i++)
            {
                data += CustomList1[i].x.ToString() + ",";
                data += CustomList1[i].y.ToString() + ",";
            }
        }


        data += Towrnpath.Count.ToString() + ",";
        for (int i = 0; i < Towrnpath.Count; i++)
        {
            data += Towrnpath[i].x.ToString() + ",";
            data += Towrnpath[i].y.ToString() + ",";
        }
        Debug.Log(Towrnpath.Count.ToString() + ",");

        data += path.Count.ToString() + ",";
        for (int i = 0; i < path.Count; i++)
        {
            data += path[i].x.ToString() + ",";
            data += path[i].y.ToString() + ",";
        }
        Debug.Log(data);
        Debug.Log(Towrnpath.Count);

        Debug.Log(path.Count);

        sw.WriteLine(data);
        sw.Close();
        fs.Close();

    }

    public void LoadMap(string Path)
    {
        ClearPath();
        ClearTowrn();
        ClearCloud();
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
            string CardImage = line[j++];
            string background = line[j++];
            int Score = int.Parse(line[j++]);

            int IsCustomPath = int.Parse(line[j++]);
            int CustomMuth = int.Parse(line[j++]);
            if(CustomMuth!=0)
            {
                for (int n = 0; n < CustomMuth; n++)
                {
                    float x = float.Parse(line[j++]);
                    float y = float.Parse(line[j++]);
                    Grid Custom = new Grid(x, y);
                    CustomList1.Add(Custom);
                }
            }
            

            int muth = int.Parse(line[j++]);
            List<Grid> TowrPath = new List<Grid>();
            for (int n = 0; n < muth; n++)
            {
                float x = float.Parse(line[j++]);
                float y = float.Parse(line[j++]);
                Grid Towr = new Grid(x, y);
                for (int m = 0; m < AllGrid.Count; m++)
                {
                    if (AllGrid[m].x == x && AllGrid[m].y == y)
                    {
                        AllGrid[m].CanTower = true;
                    }
                }
                Towrnpath.Add(Towr);
            }

            int muthpath = int.Parse(line[j++]);
            List<Grid> MovePath = new List<Grid>();
            for (int n = 0; n < muthpath; n++)
            {
                int x = int.Parse(line[j++]);
                int y = int.Parse(line[j++]);
                Grid MoveP = new Grid(x, y);
                path.Add(MoveP);
            }

            level = new GameLevel(name, CardImage, background, Score, TowrPath, MovePath);
            SetBG(CardImage, background);
        }
        foreach (var n  in CustomList1)
        {
            CustomList.Add(Instantiate(PathCloud,GetPosition(n),Quaternion.identity));
        }
        fs.Close();
        read.Close();
    }



    public static void GridSize() //计算格子的宽高
    {
        float leftBorder;
        float rightBorder;
        float topBorder;
        float downBorder;
        //获取屏幕的宽高
        Vector3 cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(Camera.main.transform.position.z)));

        leftBorder = Camera.main.transform.position.x - (cornerPos.x - Camera.main.transform.position.x);
        rightBorder = cornerPos.x;
        topBorder = cornerPos.y;
        downBorder = Camera.main.transform.position.y - (cornerPos.y - Camera.main.transform.position.y);

        MapW = rightBorder - leftBorder;
        MapH = topBorder - downBorder;

        GridW = MapW / Mapline;
        GridH = MapH / Maprow;
    }


    public void SetBG(string BGName, string roadName)
    {
        BG.sprite = Resources.Load<Sprite>("Res/Maps/" + BGName);
        Road.sprite = Resources.Load<Sprite>("Res/Maps/" + roadName);
    }

    public void ClearCloud() //清空路径云
    {
        foreach(var n in CustomList)
        {
            Destroy(n);
        }
        CustomList.Clear();
        CustomList1.Clear();
    }

    private void Awake()
    {
        ShowGrid = true;
        GridSize();

        //添加每个格子
        for (int i = 0; i < Maprow; i++)
        {
            for (int j = 0; j < Mapline; j++)
            {
                AllGrid.Add(new Grid(j, i));
            }
        }
    }

    public static Vector3 GetPosition(Grid g) //将格子坐标转为当前坐标
    {

        Vector2 vect = new Vector2(g.x * GridW - MapW / 2 + GridW / 2, g.y * GridH - MapH / 2 + GridH / 2);
        return vect;
    }

    void Update()
    {
        if (CustomList.Count != 0)
        {
            CustomPath = 1;
        }
        else
            CustomPath = 0;

        if (CustomList1.Count != 0||CustomList.Count!=0)
            Road.gameObject.SetActive(false);
        else
            Road.gameObject.SetActive(true);


        MapPath();
        DrawP();
    }

    public void AddPath()//添加路径云
    {
        var Cloud = Instantiate(PathCloud, new Vector3(0, 0, 0), Quaternion.identity);
        CustomList.Add(Cloud);
    }

    public void ClearNowPath() // 删除选中的路径云
    {
        CustomList.Remove(NowPath);
        Destroy(NowPath);
    }

    public void MapPath() //鼠标点击产生路径点或者塔点
    {
        if (!uidropdown.Bulding || uidropdown.Custom)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            var AddTowrn = MouthGrid(MouthDown());
            if (AddTowrn != null)
            {
                foreach (var n in AllGrid)
                {
                    if (n.x == AddTowrn.x && n.y == AddTowrn.y)
                    {
                        if (n.CanTower)
                        {
                            n.CanTower = false;
                            foreach (var w in Holder)
                            {
                                if (w.transform.position == GetPosition(AddTowrn))
                                {
                                    Destroy(w);
                                    Holder.Remove(w);
                                    foreach (var j in Towrnpath)
                                    {
                                        if (n.x == j.x && n.y == j.y)
                                        {
                                            Towrnpath.Remove(j);
                                            return;
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            n.CanTower = true;
                            Towrnpath.Add(n);
                        }
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            var AddTowrn = MouthGrid(MouthDown());
            if (AddTowrn != null)
            {
                for (int i = path.Count - 1; i >= 0; i--)
                {
                    if (path[i].x == AddTowrn.x && path[i].y == AddTowrn.y)
                    {
                        path.RemoveAt(i);

                        return;
                    }
                }
                path.Add(AddTowrn);
            }
        }

    }

    //鼠标位置
    public static Vector3 MouthDown()
    {
        GridSize();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var pos = hit.point;
            return pos;
        }
        return new Vector3(100, 100, 100);
    }
    //将鼠标位置转换为格
    public static Grid MouthGrid(Vector3 postion)
    {
        int tileX = (int)((postion.x + MapW / 2) / GridW);
        int tileY = (int)((postion.y + MapH / 2) / GridH);
        Grid MG = new Grid(tileX, tileY);
        return MG;
    }



    private void OnDrawGizmos()
    {
        if (!ShowGrid) return;
        GridSize();

        Gizmos.color = Color.black;
        for (int i = 0; i <= Maprow; i++)
        {
            Vector2 from = new Vector2(-MapW / 2, -MapH / 2 + i * GridH);
            Vector2 to = new Vector2(MapW / 2, -MapH / 2 + i * GridH);
            Gizmos.DrawLine(from, to);
        }
        for (int i = 0; i <= Mapline; i++)
        {
            Vector2 from = new Vector2(-MapW / 2 + i * GridW, -MapH / 2);
            Vector2 to = new Vector2(-MapW / 2 + i * GridW, MapH / 2);
            Gizmos.DrawLine(from, to);
        }


        Gizmos.color = Color.red;

        for (int i = 0; i < AllGrid.Count; i++)
        {
            if (AllGrid[i].CanTower)
            {
                Gizmos.DrawIcon(GetPosition(AllGrid[i]), "holder", true);
            }
        }

        //绘制路径
        for (int i = 0; i < path.Count; i++)
        {
            if (i == 0)
            {
                Gizmos.DrawIcon(GetPosition(path[i]), "start", true);
            }
            if (path.Count > 1 && i == path.Count - 1)
            {
                Gizmos.DrawIcon(GetPosition(path[i]), "end", true);
            }
            if (path.Count > 1 && i != 0)
            {
                Vector2 from = GetPosition(path[i - 1]);
                Vector2 to = GetPosition(path[i]);
                Gizmos.DrawLine(from, to);
            }
        }

    }

    List<GameObject> Holder = new List<GameObject>();
    GameObject Starts;
    GameObject Ends;
    public void DrawP()
    {
        for (int i = 0; i < AllGrid.Count; i++) //绘制塔点
        {
            if (AllGrid[i].CanTower)
            {
                bool Have = false;
                foreach (var n in Holder)
                {
                    if (n.transform.position == GetPosition(AllGrid[i]))
                    {
                        Have = true;
                    }
                }
                if (!Have)
                {
                    var ho = Instantiate(holder, GetPosition(AllGrid[i]), Quaternion.identity);
                    Holder.Add(ho);
                }

            }
        }
        List<Vector3> pos = new List<Vector3>();

        //绘制路径
        for (int i = 0; i < path.Count; i++)
        {
            if (i == 0)
            {
                if (Starts == null)
                {
                    Starts = Instantiate(start, GetPosition(path[i]), Quaternion.identity);
                }
                else
                {
                    Starts.transform.position = GetPosition(path[i]);
                }
            }
            if (path.Count > 1 && i == path.Count - 1)
            {
                if (Ends == null)
                {
                    Ends = Instantiate(end, GetPosition(path[i]), Quaternion.identity);
                }
                else
                {
                    Ends.transform.position = GetPosition(path[i]);
                }
            }
            if (path.Count > 1 && i != 0)
            {
                Vector3 from = GetPosition(path[i - 1]);
                pos.Add(from);
            }

        }
        if (path.Count > 0)
        {
            pos.Add(GetPosition(path[path.Count - 1]));
            Vector3[] posf = new Vector3[pos.Count];
            line.positionCount = pos.Count;
            for (int j = 0; j < pos.Count; j++)
            {
                var posy = pos[j];
                posy.z = -0.01f;
                posf[j] = posy;
            }
            line.SetPositions(posf);
        }


        if (path.Count < 2)
        {
            Destroy(Ends);
        }
        if (path.Count < 1)
        {
            Destroy(Starts);
        }
    }

}
