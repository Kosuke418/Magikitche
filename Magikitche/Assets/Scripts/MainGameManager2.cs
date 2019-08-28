using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameManager2 : MonoBehaviour
{
    // 各種タイル
    private Tile[,] AllTiles = new Tile[10, 10];
    private List<Tile> EmptyTiles = new List<Tile>();

    // プレイヤーのセレクトボタンの位置
    private int P1 = 2;
    private int P2 = 2;

    // タイマー
    int seconds;
    int sseconds;
    public Text timerText;
    public float totalTime;

    // セレクトボタン
    public Image select10;
    public Image select11;
    public Image select20;
    public Image select21;

    // プレイヤーの所持食材と料理
    public static int[] Player1Ingred = new int[10];
    public static int[] Player2Ingred = new int[10];
    public static int[] Player1Food = new int[4];
    public static int[] Player2Food = new int[4];
    public Image[] madeFood1;
    public Image[] madeFood2;

    // 競売に使う変数
    public static int GameProgress;
    public static int PrecedNum;
    public static int IngredNum;
    public static int IngredNum2;

    // ターン
    public static int TurnCount = 1;

    // プレイヤーの点数
    public static int P1Score = 100;
    public static int P2Score = 100;
    public Text scoreText1;
    public Text scoreText2;

    public static bool GenerateStop;
    private int random;
    int[] ReachIngred1 = new int[10];
    int[] ReachIngred2 = new int[10];
    bool ReachKaburi;
    int ReachKaburiIngred;

    // オーディオデータ
    public AudioClip StartVoice;
    public AudioClip select;
    public AudioClip Make1Food;
    public AudioClip Make2Food;
    public AudioClip PutFood;
    public AudioSource seSource;
    public AudioSource bgmSource;

    // ステータス
    public enum State
    {
        Ready,
        Generate,
        Choice,
        MakeFood,
        CheckTile,
        Check,
        Result,
        ScoreView
    }

    private State state;

    // Start is called before the first frame update
    void Start()
    {
        Tile[] AllTilesOneDim = GameObject.FindObjectsOfType<Tile>();
        foreach (Tile t in AllTilesOneDim)
        {
            t.Number = 0;
            AllTiles[t.IndCols, t.IndRows] = t;
            EmptyTiles.Add(t);
        }

        P1 = 0;
        P2 = 1;

        if (GameProgress == 1)
        {
            state = State.MakeFood;
        }
        else
        {
            state = State.Ready;
        }


    }

    // 食料生成を遅らせる関数
    private IEnumerator DelayGenerate(int num, float waitTime)
    {
        GenerateStop = false;
        yield return new WaitForSeconds(waitTime);
        Generate(num);
        yield return new WaitForSeconds(waitTime);
        GenerateStop = true;
    }

    // 食材生成の関数
    void Generate(int num)
    {
        int start = 1;
        int end = 19;

        List<int> numbers = new List<int>();

        for (int i = start; i <= end; i++)
        {
            numbers.Add(i);
        }
        while (num-- > 0)
        {
            int index = Random.Range(0, numbers.Count);
            int randomNum = numbers[index];
            AllTiles[0, num].Number = randomNum;
            numbers.RemoveAt(index);
        }
    }

    // セレクトボタン移動の関数
    public void OnClickAct(int number)
    {
        switch (number)
        {
            case 0:
                P1 = 0;
                break;
            case 1:
                P1 = 1;
                break;
            case 2:
                P2 = 0;
                break;
            case 3:
                P2 = 1;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
       //  Debug.Log(TurnCount);

        scoreText1.text = P1Score.ToString();
        scoreText2.text = P2Score.ToString();

        if (state == State.Ready)
        {
                state = State.Generate;

        }
        else if (state == State.Generate)
        {
            if (AllTiles[0, 0].Number == 0 && AllTiles[0, 1].Number == 0)
            {
                StartCoroutine(DelayGenerate(2, 0f));
                TurnCount++;
                totalTime = 3f;
                state = State.Choice;
            }
        }
        else if (state == State.Choice)
        {
            for(int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (ReachIngred1[i] == ReachIngred1[j] && ReachIngred1[i] != 0)
                    {
                        ReachKaburi = true;
                        ReachKaburiIngred = ReachIngred1[i];
                        break;
                    }
                    else
                    {
                        ReachKaburi = false;
                    }
                }
            }
            if (!ReachKaburi)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (ReachIngred1[i] == AllTiles[0, 0].Number)
                    {
                        AllTiles[0, 0].GetComponent<Image>().color = Color.red;
                        break;
                    }
                    else
                    {
                        AllTiles[0, 0].GetComponent<Image>().color = Color.white;
                    }
                    if (ReachIngred2[i] == AllTiles[0, 0].Number)
                    {
                        AllTiles[0, 0].GetComponent<Image>().color = Color.blue;
                        break;
                    }
                    else
                    {
                        AllTiles[0, 0].GetComponent<Image>().color = Color.white;
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    if (ReachIngred1[i] == AllTiles[0, 1].Number)
                    {
                        AllTiles[0, 1].GetComponent<Image>().color = Color.red;
                        break;
                    }
                    else
                    {
                        AllTiles[0, 1].GetComponent<Image>().color = Color.white;
                    }
                    if (ReachIngred2[i] == AllTiles[0, 1].Number)
                    {
                        AllTiles[0, 1].GetComponent<Image>().color = Color.blue;
                        break;
                    }
                    else
                    {
                        AllTiles[0, 1].GetComponent<Image>().color = Color.white;
                    }
                }
            }
            else
            {
                if (ReachKaburiIngred == AllTiles[0, 0].Number)
                {
                    AllTiles[0, 0].GetComponent<Image>().color = Color.cyan;
                }
                if (ReachKaburiIngred == AllTiles[0, 1].Number)
                {
                    AllTiles[0, 1].GetComponent<Image>().color = Color.cyan;
                }
            }
            totalTime -= Time.deltaTime;
            seconds = (int)totalTime;
            sseconds = (int)((totalTime - seconds) * 100);
            if (seconds >= 0 && sseconds >= 0)
            {
                timerText.text = seconds.ToString() + "." + sseconds.ToString();
            }
            if (P1 == 0)
            {
                select10.enabled = true;
                select11.enabled = false;
            }
            else
            {
                select11.enabled = true;
                select10.enabled = false;
            }
            if (P2 == 0)
            {
                select20.enabled = true;
                select21.enabled = false;
            }
            else
            {
                select21.enabled = true;
                select20.enabled = false;
            }
            for (int i = 0; i < 9; i++)
            {
                if (AllTiles[1, i].Number == 0)
                {
                    AllTiles[1, i].Number = AllTiles[1, i + 1].Number;
                    AllTiles[1, i + 1].Number = 0;
                }
                if (AllTiles[2, i].Number == 0)
                {
                    AllTiles[2, i].Number = AllTiles[2, i + 1].Number;
                    AllTiles[2, i + 1].Number = 0;
                }
            }
            if (GenerateStop == true)
            {
                if (Input.GetKeyDown("joystick 1 button 4") || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown("joystick 1 button 2"))
                {
                    seSource.clip = select;
                    seSource.Play();
                    OnClickAct(0);
                }
                else if (Input.GetKeyDown("joystick 1 button 5") || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown("joystick 1 button 1"))
                {
                    seSource.clip = select;
                    seSource.Play();
                    OnClickAct(1);
                }
                else if (Input.GetKeyDown("joystick 2 button 4") || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("joystick 2 button 2"))
                {
                    seSource.clip = select;
                    seSource.Play();
                    OnClickAct(2);
                }
                else if (Input.GetKeyDown("joystick 2 button 5") || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("joystick 2 button 1"))
                {
                    seSource.clip = select;
                    seSource.Play();
                    OnClickAct(3);
                }
            }
            if (totalTime <= 0)
            {
                state = State.MakeFood;
            }
            GameProgress = 0;
        }
        else if (state == State.MakeFood)
        {
            if (GameProgress == 1)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (Player1Ingred[i] != 0)
                        AllTiles[1, i].Number = Player1Ingred[i];
                    if (Player2Ingred[i] != 0)
                        AllTiles[2, i].Number = Player2Ingred[i];
                }
                for (int i = 0; i < 4; i++)
                {
                    if (Player1Food[i] != 0)
                    {
                        madeFood1[i].enabled = true;
                        madeFood1[i].sprite = Library.Instance.Foods[Player1Food[i]].FoodSprite;
                    }
                    if (Player2Food[i] != 0)
                    {
                        madeFood2[i].enabled = true;
                        madeFood2[i].sprite = Library.Instance.Foods[Player2Food[i]].FoodSprite;
                    }
                }
                if (PrecedNum == 1)
                {
                    for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                    {
                        if (AllTiles[1, FoodNum].Number == 0 && Player1Ingred[FoodNum] == 0)
                        {
                            AllTiles[1, FoodNum].Number = IngredNum;
                            Player1Ingred[FoodNum] = IngredNum;
                            IngredNum = 0;

                            random = Random.Range(0, 5);

                            seSource.clip = PutFood;
                            seSource.Play();

                            break;
                        }
                    }
                    for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                    {
                        if (AllTiles[2, FoodNum].Number == 0 && Player2Ingred[FoodNum] == 0)
                        {
                            AllTiles[2, FoodNum].Number = IngredNum2;
                            Player2Ingred[FoodNum] = IngredNum2;
                            IngredNum2 = 0;
                            break;
                        }
                    }
                }
                else
                {
                    for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                    {
                        if (AllTiles[1, FoodNum].Number == 0 && Player1Ingred[FoodNum] == 0)
                        {
                            AllTiles[1, FoodNum].Number = IngredNum2;
                            Player1Ingred[FoodNum] = IngredNum2;
                            IngredNum2 = 0;


                            seSource.clip = PutFood;
                            seSource.Play();

                            break;
                        }
                    }
                    for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                    {
                        if (AllTiles[2, FoodNum].Number == 0 && Player2Ingred[FoodNum] == 0)
                        {
                            AllTiles[2, FoodNum].Number = IngredNum;
                            Player2Ingred[FoodNum] = IngredNum;
                            IngredNum = 0;
                            break;
                        }
                    }
                }
            }
            else if (P1 == 0 && P2 == 0)
            {
                IngredNum = AllTiles[0, 0].Number;
                IngredNum2 = AllTiles[0, 1].Number;
                SceneManager.LoadScene("AuctionScene");
            }
            else if (P1 == 1 && P2 == 1)
            {
                IngredNum = AllTiles[0, 1].Number;
                IngredNum2 = AllTiles[0, 0].Number;
                SceneManager.LoadScene("AuctionScene");
            }
            else if (P1 == 0 && P2 == 1)
            {
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[1, FoodNum].Number == 0 && Player1Ingred[FoodNum] == 0)
                    {
                        AllTiles[1, FoodNum].Number = AllTiles[0, 0].Number;
                        Player1Ingred[FoodNum] = AllTiles[0, 0].Number;
                        AllTiles[0, 0].Number = 0;

                        seSource.clip = PutFood;
                        seSource.Play();

                        break;
                    }
                }
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[2, FoodNum].Number == 0 && Player2Ingred[FoodNum] == 0)
                    {
                        AllTiles[2, FoodNum].Number = AllTiles[0, 1].Number;
                        Player2Ingred[FoodNum] = AllTiles[0, 1].Number;
                        AllTiles[0, 1].Number = 0;
                        break;
                    }
                }
            }
            else if (P1 == 1 && P2 == 0)
            {
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[2, FoodNum].Number == 0 && Player2Ingred[FoodNum] == 0)
                    {
                        AllTiles[2, FoodNum].Number = AllTiles[0, 0].Number;
                        Player2Ingred[FoodNum] = AllTiles[0, 0].Number;
                        AllTiles[0, 0].Number = 0;

                        seSource.clip = PutFood;
                        seSource.Play();

                        break;
                    }
                }
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[1, FoodNum].Number == 0 && Player1Ingred[FoodNum] == 0)
                    {
                        AllTiles[1, FoodNum].Number = AllTiles[0, 1].Number;
                        Player1Ingred[FoodNum] = AllTiles[0, 1].Number;
                        AllTiles[0, 1].Number = 0;
                        break;
                    }
                }
            }

            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (AllTiles[1, i].Number == 0)
                    {
                        Player1Ingred[i] = Player1Ingred[i + 1];
                        Player1Ingred[i + 1] = 0;
                        AllTiles[1, i].Number = AllTiles[1, i + 1].Number;
                        AllTiles[1, i + 1].Number = 0;
                    }
                    if (AllTiles[2, i].Number == 0)
                    {
                        Player2Ingred[i] = Player2Ingred[i + 1];
                        Player2Ingred[i + 1] = 0;
                        AllTiles[2, i].Number = AllTiles[2, i + 1].Number;
                        AllTiles[2, i + 1].Number = 0;
                    }
                }
            }

            // プレイヤー１の料理作成
            int[] Ingred1;
            bool[] IngredThis1 = new bool[10];
            int m1;
            int count1;
            int temp1;
            int reachCount1 = 0;
            ReachIngred1 = new int[10];
            for (int i = 1; i < 14; i++)
            {
                m1 = 0;
                Ingred1 = new int[10];
                count1 = 0;
                temp1 = 0;
                for (int k = 0; k < 8; k++)
                {
                    if (Library.Instance.Foods[i].Answers[k].AnswerNumber != 0)
                    {
                        for (int l = 0; l < 10; l++)
                        {
                            // Debug.Log(Library.Instance.Categorys[i].CategoryName + "," + Library.Instance.Categorys[i].Foods[j].FoodName + "," + Library.Instance.Categorys[i].Foods[j].Answers[k].AnswerName + "," + l);
                            if (Library.Instance.Foods[i].Answers[k].AnswerNumber == AllTiles[1, l].Number)
                            {
                               // Debug.Log(Library.Instance.Foods[i].FoodName + "の" + Library.Instance.Foods[i].Answers[k].AnswerNumber + "があるよ");
                                Ingred1[m1] = l;
                                IngredThis1[k] = true;
                                m1++;
                                break;
                            }
                            else if (l == 9)
                            {
                               // Debug.Log(Library.Instance.Foods[i].FoodName + "の" + Library.Instance.Foods[i].Answers[k].AnswerNumber + "がないよ");
                                IngredThis1[k] = false;
                                count1++;
                                temp1 = k;
                            }
                        }
                    }
                    else
                    {
                        IngredThis1[k] = true;
                    }
                }
                if (IngredThis1[0] && IngredThis1[1] && IngredThis1[2] && IngredThis1[3] && IngredThis1[4] && IngredThis1[5] && IngredThis1[6] && IngredThis1[7])
                {
                    for (int n = 0; n < 8; n++)
                    {
                        AllTiles[1, Ingred1[n]].Number = 0;
                        Player1Ingred[Ingred1[n]] = 0;
                    }

                    Debug.Log(Library.Instance.Foods[i].FoodName + "が作れる");
                    ReachIngred1 = new int[10];
                    P1Score += 100;

                    random = Random.Range(0, 5);
                    if (random == 0)
                    {
                        seSource.clip = Make1Food;
                        seSource.Play();
                    }
                    else
                    {
                        seSource.clip = Make2Food;
                        seSource.Play();
                    }

                    for (int FoodNum = 0; FoodNum < 4; FoodNum++)
                    {
                        if (madeFood1[FoodNum].sprite == null && Player1Food[FoodNum] == 0)
                        {
                            madeFood1[FoodNum].enabled = true;
                            madeFood1[FoodNum].sprite = Library.Instance.Foods[i].FoodSprite;
                            Player1Food[FoodNum] = i;
                            break;
                        }
                    }
                }
                else if (count1 == 1)
                {
                    Debug.Log("残り一個だよ！");
                    Debug.Log(Library.Instance.Foods[i].FoodName + "," + Library.Instance.Foods[i].Answers[temp1].AnswerName);
                    ReachIngred1[reachCount1] = Library.Instance.Foods[i].Answers[temp1].AnswerNumber;
                    reachCount1++;
                }
            }

            // プレイヤー２の料理作成
            int[] Ingred2;
            bool[] IngredThis2 = new bool[10];
            int m2;
            int count2;
            int temp2;
            int reachCount2 = 0;
            ReachIngred2 = new int[10];
            for (int i = 1; i < 14; i++)
            {
                temp2 = 0;
                count2 = 0;
                m2 = 0;
                Ingred2 = new int[10];
                for (int k = 0; k < 8; k++)
                {
                    if (Library.Instance.Foods[i].Answers[k].AnswerNumber != 0)
                    {
                        for (int l = 0; l < 10; l++)
                        {
                            //Debug.Log(Library.Instance.Categorys[i].CategoryName + "," + Library.Instance.Categorys[i].Foods[j].FoodName + "," + Library.Instance.Categorys[i].Foods[j].Answers[k].AnswerName + "," + l);
                            if (Library.Instance.Foods[i].Answers[k].AnswerNumber == AllTiles[2, l].Number)
                            {
                                //Debug.Log(Library.Instance.Categorys[i].Foods[j].FoodName + "の" + Library.Instance.Categorys[i].Foods[j].Answers[k].AnswerName + "があるよ");
                                Ingred2[m2] = l;
                                IngredThis2[k] = true;
                                m2++;
                                break;
                            }
                            else if (l == 9)
                            {
                                //Debug.Log(Library.Instance.Categorys[i].Foods[j].FoodName + "の" + Library.Instance.Categorys[i].Foods[j].Answers[k].AnswerName + "がないよ");
                                IngredThis2[k] = false;
                                count2++;
                                temp2 = k;
                            }
                        }
                    }
                    else
                    {
                        IngredThis2[k] = true;
                    }
                }
                if (IngredThis2[0] && IngredThis2[1] && IngredThis2[2] && IngredThis2[3] && IngredThis2[4] && IngredThis2[5] && IngredThis2[6] && IngredThis2[7])
                {
                    for (int n = 0; n < 8; n++)
                    {
                        AllTiles[2, Ingred2[n]].Number = 0;
                        Player2Ingred[Ingred2[n]] = 0;
                    }

                    // Debug.Log(Library.Instance.Categorys[i].Foods[j].FoodName + "が作れる");
                    ReachIngred2 = new int[10];
                    P2Score += 100;

                    random = Random.Range(0, 5);
                    if (random == 0)
                    {
                        seSource.clip = Make1Food;
                        seSource.Play();
                    }
                    else
                    {
                        seSource.clip = Make2Food;
                        seSource.Play();
                    }

                    for (int FoodNum = 0; FoodNum < 4; FoodNum++)
                    {
                        if (madeFood2[FoodNum].sprite == null && Player2Food[FoodNum] == 0)
                        {
                            madeFood2[FoodNum].enabled = true;
                            madeFood2[FoodNum].sprite = Library.Instance.Foods[i].FoodSprite;
                            Player2Food[FoodNum] = i;
                            break;
                        }
                    }
                }
                else if (count2 == 1)
                {
                    // Debug.Log("残り一個だよ！");
                    // Debug.Log(Library.Instance.Foods[i].FoodName + "," + Library.Instance.Foods[i].Answers[temp1].AnswerName);
                    ReachIngred2[reachCount2] = Library.Instance.Foods[i].Answers[temp2].AnswerNumber;
                    reachCount2++;
                }

            }
            state = State.Check;
        }
        else if (state == State.Check)
        {
            if (AllTiles[1, 0].Number != 0 &&
                AllTiles[1, 1].Number != 0 &&
                AllTiles[1, 2].Number != 0 &&
                AllTiles[1, 3].Number != 0 &&
                AllTiles[1, 4].Number != 0 &&
                AllTiles[1, 5].Number != 0 &&
                AllTiles[1, 6].Number != 0 &&
                AllTiles[1, 7].Number != 0 &&
                AllTiles[1, 8].Number != 0 &&
                AllTiles[1, 9].Number != 0)
            {
                ReachIngred1 = new int[10];
                state = State.MakeFood;
                AllTiles[1, 0].Number = 0;
                AllTiles[1, 1].Number = 0;
                AllTiles[1, 2].Number = 0;
                AllTiles[1, 3].Number = 0;
                AllTiles[1, 4].Number = 0;
            }
            if (AllTiles[2, 0].Number != 0 &&
                AllTiles[2, 1].Number != 0 &&
                AllTiles[2, 2].Number != 0 &&
                AllTiles[2, 3].Number != 0 &&
                AllTiles[2, 4].Number != 0 &&
                AllTiles[2, 5].Number != 0 &&
                AllTiles[2, 6].Number != 0 &&
                AllTiles[2, 7].Number != 0 &&
                AllTiles[2, 8].Number != 0 &&
                AllTiles[2, 9].Number != 0)
            {
                ReachIngred2 = new int[10];
                state = State.MakeFood;
                AllTiles[2, 0].Number = 0;
                AllTiles[2, 1].Number = 0;
                AllTiles[2, 2].Number = 0;
                AllTiles[2, 3].Number = 0;
                AllTiles[2, 4].Number = 0;
            }
            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (AllTiles[1, i].Number == 0)
                    {
                        Player1Ingred[i] = Player1Ingred[i + 1];
                        Player1Ingred[i + 1] = 0;
                        AllTiles[1, i].Number = AllTiles[1, i + 1].Number;
                        AllTiles[1, i + 1].Number = 0;
                    }
                    if (AllTiles[2, i].Number == 0)
                    {
                        Player2Ingred[i] = Player2Ingred[i + 1];
                        Player2Ingred[i + 1] = 0;
                        AllTiles[2, i].Number = AllTiles[2, i + 1].Number;
                        AllTiles[2, i + 1].Number = 0;
                    }
                }
            }
            state = State.CheckTile;
        }
        else if (state == State.CheckTile)
        {
            if (TurnCount == 30)
            {
                Debug.Log("ターン終了だよ！");
                state = State.Result;
            }
            else
            {
                state = State.Generate;
            }
        }
        else if (state == State.Result)
        {
            TurnCount = 0;
            Player1Ingred = new int[10];
            Player2Ingred = new int[10];
            Player1Food = new int[4];
            Player2Food = new int[4];

            timerText.enabled = false;
            SceneManager.LoadScene("Main");
        }
    }
}
