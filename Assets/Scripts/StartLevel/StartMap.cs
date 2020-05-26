using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using LitJson;

public class Tow
{
    public float AttackCD;
    public float Scope;
    public int LevelUp1;
    public int LevelUp2;
    public int Dele1;
    public int Dele2;
    public int Dele3;

    public Tow(float attackCd, float scope, int LevelUp1, int LevelUp2, int Dele1, int Dele2, int Dele3)
    {
        AttackCD = attackCd;
        Scope = scope;
        this.LevelUp1 = LevelUp1;
        this.LevelUp2 = LevelUp2;
        this.Dele1 = Dele1;
        this.Dele2 = Dele2;
        this.Dele3 = Dele3;
    }
}
public class Bull
{
    public float Speed;
    public int attack;
    public Bull(float speed, int attack)
    {
        Speed = speed;
        this.attack = attack;
    }
}

public class StartMap : MonoBehaviour
{

    //地图宽高
    float MapW;
    float MapH;

    GameLevel level;
    public SpriteRenderer BG;
    public SpriteRenderer Road;

    const int Mapline = 12;
    const int Maprow = 8;


    public List<Grid> path = new List<Grid>();
    public List<Grid> Towrnpath = new List<Grid>();
    List<Grid> AllGrid = new List<Grid>();

    //生成象池
    public Dictionary<string, GameObject[]> monster = new Dictionary<string, GameObject[]>();
    public Dictionary<string, Bull> bullet = new Dictionary<string, Bull>();
    public Dictionary<string, Tow> tower = new Dictionary<string, Tow>();

    //格子宽高
    float GridW;
    float GridH;

    public GameObject rad;
    GameObject radish;

    public GameObject mons1;
    public GameObject mons2;
    public GameObject mons3;
    public GameObject mons4;
    public GameObject mons5;

    public List<GameObject> CustomList = new List<GameObject>();
    public List<Grid> CustomList1 = new List<Grid>();
    public GameObject PathCloud; //云预制体


    public void InitializeMap()
    {

        var radpos = GetPosition(path[path.Count - 1]);
        radish = Instantiate(rad, radpos, Quaternion.identity);
    }

    IEnumerator monsterGO() //产生怪物
    {
        yield return new WaitForSeconds(4f);
        var pos = GetPosition(path[0]);
        foreach (var n in monster.Values)
        {
            GameMode.GM.nowWave++;
            GameMode.GM.ui.nowWave.text = "0" + GameMode.GM.nowWave;
            foreach (var mon1 in n)
            {

                mon1.SetActive(true);
                mon1.GetComponent<Monster>().live = true;
                mon1.transform.position = pos;
                mon1.GetComponent<Monster>().MovePath();
                yield return new WaitForSeconds(0.5f);
                while (!GameMode.GM.GameState)
                {

                    yield return new WaitForSeconds(0.5f);
                }
            }
            if (GameMode.GM.nowWave == 5)
            {
                break;
            }
            yield return new WaitForSeconds(3f);
        }
        GameMode.GM.MonsterOver = true;
    }

    IEnumerator monsterBoss() //Boos关的怪物
    {
        yield return new WaitForSeconds(4f);
        var pos = GetPosition(path[0]);
        while (true)
        {

            foreach (var n in monster.Values)
            {
                GameMode.GM.nowWave++;
                if(GameMode.GM.nowWave>9)
                {
                    GameMode.GM.ui.nowWave.text = "" + GameMode.GM.nowWave;
                }
                else
                GameMode.GM.ui.nowWave.text = "0" + GameMode.GM.nowWave;
                foreach (var mon1 in n)
                {
                    mon1.SetActive(true);
                    mon1.GetComponent<Monster>().Die = false;
                    mon1.GetComponent<Monster>().live = true;
                    mon1.transform.position = pos;
                    mon1.GetComponent<Monster>().MovePath();
                    mon1.GetComponent<Monster>().MaxHp += 300;
                    yield return new WaitForSeconds(0.5f);
                    while (!GameMode.GM.GameState)
                    {

                        yield return new WaitForSeconds(0.5f);
                    }
                }
                yield return new WaitForSeconds(3f);
            }
        }
    }

    private void Update()
    {
        if (GameMode.GM.MonsterOver)
        {
            int i = 0;
            foreach (var mon in monster["monster5"])
            {
                if (mon.GetComponent<Monster>().live)
                {
                    i++;
                }
            }
            if (i == 0)
            {
                GameMode.GM.ui.Win();
                Time.timeScale = 1;
                IsLook.isLook.OpenLook(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "BossLevel")
        {
            StartCoroutine("monsterBoss");
        }
        else
        StartCoroutine("monsterGO");


        GameMode.GM.MapRect = new Rect(-MapW / 2, -MapH / 2, MapW, MapH);
    }


    

    public void GridSize() //计算格子的宽高
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

    private void Awake()
    {
        GridSize();
        LoadBullet();
        LoadTowrn();
        LoadJson();
        LoadMap(SceneManager.GetActiveScene().name);
        InitializeMap();
        if (CustomList1.Count != 0 || CustomList.Count != 0)
            Road.gameObject.SetActive(false);
        else
            Road.gameObject.SetActive(true);

    }


    public void LoadMap(string Path)
    {
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
            if (CustomMuth != 0)
            {
                for (int n = 0; n < CustomMuth; n++)
                {
                    float x = float.Parse(line[j++]);
                    float y = float.Parse(line[j++]);
                    Grid Custom = new Grid(x, y);
                    CustomList1.Add(Custom);
                }
                foreach (var n in CustomList1)
                {
                    CustomList.Add(Instantiate(PathCloud, GetPosition(n), Quaternion.identity));
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
        
        
        fs.Close();
        read.Close();
    }

    public void SetBG(string BGName, string roadName)
    {
        Sprite Bg = Resources.Load<Sprite>("Res/Maps/" + BGName);
        BG.sprite = Bg;
        Sprite road = Resources.Load<Sprite>("Res/Maps/" + roadName);
        Road.sprite = road;
    }

    public Vector3 GetPosition(Grid g) //将格子坐标转为当前坐标
    {

        Vector2 vect = new Vector2(g.x * GridW - MapW / 2 + GridW / 2, g.y * GridH - MapH / 2 + GridH / 2);
        return vect;
    }

    //Josn读取
    public string Json()
    {
        JsonData Monster1 = new JsonData();
        Monster1["Name"] = "monster1";
        Monster1["Speed"] = 1;
        Monster1["Hp"] = 100;
        Monster1["Golds"] = 10;


        return Monster1.ToJson();
    }
    public void LoadJson()
    {
        var textAsset = Resources.Load<TextAsset>("Res/Data/Monster").text.ToString();
        var Data = JsonMapper.ToObject(textAsset);

        for (int i = 0; i < Data.Count; i++)
        {
            string Name = Data[i]["Name"].ToString();
            float Speed = float.Parse(Data[i]["Speed"].ToString());
            int Hp = (int)Data[i]["Hp"];
            int Golds = (int)Data[i]["Golds"];
            if (!monster.ContainsKey(Name))
            {
                monster[Name] = new GameObject[10];
                for (int j = 0; j < monster[Name].Length; j++)
                {
                    monster[Name][j] = Instantiate(GenerateMons(Name), new Vector3(0, 0, 0), Quaternion.identity);
                    monster[Name][j].SetActive(false);
                    var mons = monster[Name][j].GetComponent<Monster>();
                    mons.Speed = Speed;
                    mons.HP = Hp;
                    mons.Gold = Golds;
                }
            }
        }
    }
    public void LoadBullet()
    {
        var textAsset = Resources.Load<TextAsset>("Res/Data/Bullet").text.ToString();
        var Data = JsonMapper.ToObject(textAsset);

        for (int i = 0; i < Data.Count; i++)
        {
            string Name = Data[i]["Name"].ToString();
            float Speed = float.Parse(Data[i]["Speed"].ToString());
            int attack = (int)Data[i]["attack"];
            if (!bullet.ContainsKey(Name))
            {
                bullet[Name] = new Bull(Speed, attack);
            }
        }
    }
    public void LoadTowrn()
    {
        var textAsset = Resources.Load<TextAsset>("Res/Data/Tower").text.ToString();
        var Data = JsonMapper.ToObject(textAsset);

        for (int i = 0; i < Data.Count; i++)
        {
            string Name = Data[i]["Name"].ToString();
            float AttackCD = float.Parse(Data[i]["AttackCD"].ToString());
            float Scope = float.Parse(Data[i]["Scope"].ToString());
            int LevelUp1 = (int)Data[i]["LevelUp1"];
            int LevelUp2 = (int)Data[i]["LevelUp2"];
            int Dele1 = (int)Data[i]["Dele1"];
            int Dele2 = (int)Data[i]["Dele2"];
            int Dele3 = (int)Data[i]["Dele3"];
            if (!tower.ContainsKey(Name))
            {
                tower[Name] = new Tow(AttackCD, Scope, LevelUp1, LevelUp2, Dele1, Dele2, Dele3);
            }
        }
    }

    public GameObject GenerateMons(string Name)
    {
        switch (Name)
        {
            case "monster1":
                return mons1;
            case "monster2":
                return mons2;
            case "monster3":
                return mons3;
            case "monster4":
                return mons4;
            case "monster5":
                return mons5;
        }
        return null;
    }

    public void LostLevel()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":
                SceneManager.LoadScene("Level2"); break;
            case "Level2":
                SceneManager.LoadScene("Level3"); break;
            case "Level3":
                SceneManager.LoadScene("Level4"); break;
            case "Level4":
                SceneManager.LoadScene("Level5"); break;
            case "Level5":
                SceneManager.LoadScene("EndLevel"); break;
        }
    }

}
