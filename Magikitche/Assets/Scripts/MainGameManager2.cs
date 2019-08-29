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
    public static int[] Player1Food = new int[6];
    public static int[] Player2Food = new int[6];
    public Image[] madeFood1;
    public Image[] madeFood2;

    // 競売に使う変数
    public static int GameProgress;
    public static int PrecedNum;
    public static int IngredNum;
    public static int IngredNum2;

    // ターン
    public static int TurnCount = 0;

    // プレイヤーの点数
    public static int P1Score = 1000;
    public static int P2Score = 1000;
    public Text scoreText1;
    public Text scoreText2;

    public static bool GenerateStop;
    private int random;
    public static int[] ReachIngred1 = new int[10];
    public static int[] ReachIngred2 = new int[10];
    int[] ReachFood1 = new int[10];
    int[] ReachFood2 = new int[10];
    public Text TurnCountText;
    public Text P1ReachScore1;
    public Text P1ReachScore2;
    public Text P2ReachScore1;
    public Text P2ReachScore2;
    public static string P1ReachScore;
    public static string P2ReachScore;
    public Image Dialog;
    public Text DialogText;

    public Image IngredTileLeft;
    public Image IngredTileRight;
    float G0,G1;


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
        Check,
        CheckTile,
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
        else if (GameProgress == 2)
        {
            state = State.Generate;
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

        GenerateStop = true;
    }

    private void CheckTiles()
    {
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

    private void Player1MakeFood()
    {
        // プレイヤー１の料理作成
        int[] Ingred1;
        bool[] IngredThis1 = new bool[10];
        int m1;
        int count1;
        int temp1;
        int reachCount1 = 0;
        ReachIngred1 = new int[10];
        ReachFood1 = new int[10];
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

                Debug.Log(Library.Instance.Foods[i].FoodName + "を作った");
                // ReachIngred1 = new int[10];
                // ReachFood1 = new int[10];
                P1Score += Library.Instance.Foods[i].FoodScore;

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

                for (int FoodNum = 0; FoodNum < 6; FoodNum++)
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
                // Debug.Log("残り一個だよ！")
                Debug.Log("プレイヤー１リーチ" + Library.Instance.Foods[i].FoodName + "," + Library.Instance.Foods[i].Answers[temp1].AnswerName);
                ReachIngred1[reachCount1] = Library.Instance.Foods[i].Answers[temp1].AnswerNumber;
                ReachFood1[reachCount1] = i;
                reachCount1++;
            }
        }
    }

    private void Player2MakeFood()
    {
        // プレイヤー２の料理作成
        int[] Ingred2;
        bool[] IngredThis2 = new bool[10];
        int m2;
        int count2;
        int temp2;
        int reachCount2 = 0;
        ReachIngred2 = new int[10];
        ReachFood2 = new int[10];
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
                // ReachIngred2 = new int[10];
                // ReachFood2 = new int[10];
                P2Score += Library.Instance.Foods[i].FoodScore;

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

                for (int FoodNum = 0; FoodNum < 6; FoodNum++)
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
                // Debug.Log("プレーヤー２リーチ"+Library.Instance.Foods[i].FoodName + "," + Library.Instance.Foods[i].Answers[temp2].AnswerName);
                ReachIngred2[reachCount2] = Library.Instance.Foods[i].Answers[temp2].AnswerNumber;
                ReachFood2[reachCount2] = i;
                reachCount2++;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        //  Debug.Log(TurnCount);

        TurnCountText.text = "残り食材." + (31 - TurnCount);
        scoreText1.text = P1Score.ToString();
        scoreText2.text = P2Score.ToString();

        if (state == State.Ready)
        {
            Dialog.enabled = true;
            DialogText.enabled = true;
            DialogText.text = "「STARTで調理スタート！！」";
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 7"))
            {
                Dialog.enabled = false;
                DialogText.enabled = false;
                state = State.Generate;
            }

        }
        else if (state == State.Generate)
        {
            for (int i = 0; i < 10; i++)
            {
                Debug.Log(Library.Instance.Ingreds[ReachIngred1[i]].IngredName);
            }

            if (GameProgress == 2)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (Player1Ingred[i] != 0)
                        AllTiles[1, i].Number = Player1Ingred[i];
                    if (Player2Ingred[i] != 0)
                        AllTiles[2, i].Number = Player2Ingred[i];
                }
                for (int i = 0; i < 6; i++)
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
            }
            if (AllTiles[0, 0].Number == 0 && AllTiles[0, 1].Number == 0)
            {
                Generate(2);
                TurnCount++;
                totalTime = 5f;
                state = State.Choice;
            }
        }
        else if (state == State.Choice)
        {
            float level = 0.90f * Mathf.Abs(Mathf.Sin(Time.time * 3));
            for (int i = 0; i < 10; i++)
            {
                if (ReachIngred1[i] == AllTiles[0, 0].Number)
                {
                    IngredTileLeft.GetComponent<Image>().color = new Color(1f, level, level, 1f);
                    P1ReachScore1.enabled = true;
                    P1ReachScore1.text = "+" + Library.Instance.Foods[ReachFood1[i]].FoodScore.ToString();
                    break;
                }
                else
                {
                    P1ReachScore1.text = "";
                    IngredTileLeft.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                }
            }
            for (int i = 0; i < 10; i++)
            {
                G0 = IngredTileLeft.GetComponent<Image>().color.g;
                if (ReachIngred2[i] == AllTiles[0, 0].Number)
                {
                    if (G0 != 1f)
                    {
                        //Debug.Log(01);
                        IngredTileLeft.GetComponent<Image>().color = new Color(1f, level, 1f, 1f);
                    }
                    else
                    {
                        //Debug.Log(02);
                        IngredTileLeft.GetComponent<Image>().color = new Color(level, level, 1f, 1f);
                    }
                    P2ReachScore1.enabled = true;
                    P2ReachScore1.text = "+" + Library.Instance.Foods[ReachFood2[i]].FoodScore.ToString();
                    break;
                }
                else
                {
                    P2ReachScore1.text = "";
                }
            }
            for (int i = 0; i < 10; i++)
            {
                if (ReachIngred1[i] == AllTiles[0, 1].Number)
                {
                    IngredTileRight.GetComponent<Image>().color = new Color(1f, level, level, 1f);
                    P1ReachScore2.enabled = true;
                    P1ReachScore2.text = "+" + Library.Instance.Foods[ReachFood1[i]].FoodScore.ToString();
                    break;
                }
                else
                {
                    P1ReachScore2.text = "";
                    IngredTileRight.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                }
            }
            for (int i = 0; i < 10; i++)
            {
                G1 = IngredTileRight.GetComponent<Image>().color.g;
                if (ReachIngred2[i] == AllTiles[0, 1].Number)
                {

                    if (G1 != 1f)
                    {
                        //Debug.Log(11);
                        IngredTileRight.GetComponent<Image>().color = new Color(1f, level, 1f, 1f);
                    }
                    else
                    {
                        //Debug.Log(12);
                        IngredTileRight.GetComponent<Image>().color = new Color(level, level, 1f, 1f);
                    }
                    P2ReachScore2.enabled = true;
                    P2ReachScore2.text = "+" + Library.Instance.Foods[ReachFood2[i]].FoodScore.ToString();
                    break;
                }
                else
                {
                    P2ReachScore2.text = "";
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
                for (int i = 0; i < 6; i++)
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
                if (AllTiles[1, 9].Number != 0)
                {
                    AllTiles[1, 0].Number = 0;
                    Player1Ingred[0] = 0;
                }
                if (AllTiles[2, 9].Number != 0)
                {
                    AllTiles[2, 0].Number = 0;
                    Player2Ingred[0] = 0;
                }
                CheckTiles();
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
                P1ReachScore = P1ReachScore1.text;
                P2ReachScore = P2ReachScore1.text;
                IngredNum = AllTiles[0, 0].Number;
                IngredNum2 = AllTiles[0, 1].Number;
                SceneManager.LoadScene("AuctionScene");
            }
            else if (P1 == 1 && P2 == 1)
            {
                P1ReachScore = P1ReachScore2.text;
                P2ReachScore = P2ReachScore2.text;
                IngredNum = AllTiles[0, 1].Number;
                IngredNum2 = AllTiles[0, 0].Number;
                SceneManager.LoadScene("AuctionScene");
            }
            else if (P1 == 0 && P2 == 1)
            {
                if (AllTiles[1, 9].Number != 0)
                {
                    AllTiles[1, 0].Number = 0;
                    Player1Ingred[0] = 0;
                }
                if (AllTiles[2, 9].Number != 0)
                {
                    AllTiles[2, 0].Number = 0;
                    Player2Ingred[0] = 0;
                }
                CheckTiles();
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
                if (AllTiles[1, 9].Number != 0)
                {
                    AllTiles[1, 0].Number = 0;
                    Player1Ingred[0] = 0;
                }
                if (AllTiles[2, 9].Number != 0)
                {
                    AllTiles[2, 0].Number = 0;
                    Player2Ingred[0] = 0;
                }
                CheckTiles();
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

            Player1MakeFood();
            Player2MakeFood();

            state = State.Check;
        }
        else if (state == State.Check)
        {
            CheckTiles();
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
            Player1Food = new int[6];
            Player2Food = new int[6];
            ReachIngred1 = new int[10];
            ReachIngred2 = new int[10];
            ReachFood1 = new int[10];
            ReachFood2 = new int[10];

            timerText.enabled = false;

            Dialog.enabled = true;
            DialogText.enabled = true;
            DialogText.text = "「STARTで結果表示！！」";
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 7"))
            {
                Dialog.enabled = false;
                DialogText.enabled = false;
                SceneManager.LoadScene("Result");
            }
        }
    }
}