using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    private Tile[,] AllTiles = new Tile[10,10];
    private List<Tile> EmptyTiles = new List<Tile>();
    private List<Tile[]> columns = new List<Tile[]>();

    private bool oshita;
    public int temp;
    private int temp2;
    private int t;
    private int count = 0;
    private int[] temp1 = new int[100];
    private bool GenerateStop;
    public static int P1Score = 100;
    public static int P2Score = 100;
    public static int GameCount = 0;
    public static int IngredNum1;
    public static int IngredNum2;
    public static int GameProgress;
    public static int PrecedNum;
    public static int[] Player1Ingred = new int[10];
    public static int[] Player2Ingred = new int[10];

    public Text[] PlusScoreText;
    public Text[] MinusScoreText;
    public Image DialogPanel;
    public Text DialogText;
    public Text OdaiText;
    public Text ScoreText1;
    public Text ScoreText2;
    public Image ResultFood;
    public Image[] Food;
    public Image ResultPanel;
    public Image[] Result;
    public Sprite Maru;
    public Sprite Batu;
    public Image STile;
    public Image ShitaTile;
    public Text STileText;
    public Text ShitaTileText;
    public Text WText;
    public Text UeTileText;

    public enum State {
        third,
        firstready,
        secondready,
        choicefood,
        secondchoicefood,
        result,
        score,
        resultPlayer,
        tugi,
        gameover,
        hint
    }

    public State state;

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

        columns.Add(new Tile[] { AllTiles[0, 0], AllTiles[0, 1], AllTiles[0, 2], AllTiles[0, 3] });
        if (GameCount == 0 && GameProgress != 1)
            state = State.firstready;
        else if ((GameCount == 1 || GameCount == 2)&& GameProgress != 1)
            state = State.tugi;
        else if (GameProgress == 1)
            state = State.secondchoicefood;

        oshita = false;
        ResultFood.enabled = false;
        for(int i = 1; i < 9; i++)
        {
            Food[i-1].enabled = false;
        }
        ScoreText1.text = P1Score.ToString();
        ScoreText2.text = P2Score.ToString();
    }

    private IEnumerator DelayGenerate(int num, float waitTime)
    {
        GenerateStop = false;
        yield return new WaitForSeconds(waitTime);
        Generate(num);
        yield return new WaitForSeconds(waitTime);
        GenerateStop = true;
    }

    private IEnumerator DelayDialog(float waitTime)
    {
        DialogText.text = "GM「正解の料理は" + Library.Instance.Categorys[temp].Foods[temp2].FoodName + "でした！」";
        yield return new WaitForSeconds(waitTime);
        DialogText.text = "GM「Aボタンで採点します！」";
    }

    void Generate(int num)
    {
        Tile[] LineOfTiles;
        LineOfTiles = columns[0];
        int start = 1;
        int end = 18;

        List<int> numbers = new List<int>();

        for(int i = start; i <= end; i++)
        {
            numbers.Add(i);
        }
        while(num-- > 0)
        {
            int index = Random.Range(0, numbers.Count);
            int randomNum = numbers[index];
            AllTiles[0, num].Number = randomNum;
            numbers.RemoveAt(index);
        }
    }

    public void OnClickAct(int number)
    {
        switch (number)
        {
            case 0:
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[1, FoodNum].GetComponent<Image>().sprite == null)
                    {
                        AllTiles[1, FoodNum].Number = AllTiles[0, 0].Number;
                        AllTiles[0, 0].Number = 0;
                        if (AllTiles[0, 0].GetComponent<Image>().sprite!=null)
                             AllTiles[1, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }
                }
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[2, FoodNum].GetComponent<Image>().sprite == null)
                    {
                        AllTiles[2, FoodNum].Number = AllTiles[0, 2].Number;
                        AllTiles[0, 2].Number = 0;
                        if (AllTiles[0, 2].GetComponent<Image>().sprite != null)
                            AllTiles[2, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }
                }
                AllTiles[0, 0].GetComponent<Image>().sprite = null;
                AllTiles[0, 2].GetComponent<Image>().sprite = null;
                AllTiles[0, 0].Number = 0;
                AllTiles[0, 2].Number = 0;
                break;


            case 1:
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[2, FoodNum].GetComponent<Image>().sprite == null)
                    {
                        AllTiles[2, FoodNum].Number = AllTiles[0, 1].Number;
                        AllTiles[0, 1].Number = 0;
                        if (AllTiles[0, 1].GetComponent<Image>().sprite != null)
                            AllTiles[2, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }

                }
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[1, FoodNum].GetComponent<Image>().sprite == null)
                    {
                        AllTiles[1, FoodNum].Number = AllTiles[0, 3].Number;
                        AllTiles[0, 3].Number = 0;
                        if (AllTiles[0, 3].GetComponent<Image>().sprite != null)
                            AllTiles[1, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }
                }
                AllTiles[0, 1].GetComponent<Image>().sprite = null;
                AllTiles[0, 3].GetComponent<Image>().sprite = null;
                AllTiles[0, 1].Number = 0;
                AllTiles[0, 3].Number = 0;
                break;


            case 2:
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[1, FoodNum].GetComponent<Image>().sprite == null)
                    {
                        AllTiles[1, FoodNum].Number = AllTiles[0, 2].Number;
                        AllTiles[0, 2].Number = 0;
                        if (AllTiles[0, 2].GetComponent<Image>().sprite != null)
                            AllTiles[1, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }

                }
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[2, FoodNum].GetComponent<Image>().sprite == null)
                    {
                        AllTiles[2, FoodNum].Number = AllTiles[0, 0].Number;
                        AllTiles[0, 0].Number = 0;
                        if (AllTiles[0, 0].GetComponent<Image>().sprite != null)
                            AllTiles[2, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }
                }
                AllTiles[0, 0].GetComponent<Image>().sprite = null;
                AllTiles[0, 2].GetComponent<Image>().sprite = null;
                AllTiles[0, 0].Number = 0;
                AllTiles[0, 2].Number = 0;
                break;


            case 3:
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[2, FoodNum].GetComponent<Image>().sprite == null)
                    {
                        AllTiles[2, FoodNum].Number = AllTiles[0, 3].Number;
                        AllTiles[0, 3].Number = 0;
                        if (AllTiles[0, 3].GetComponent<Image>().sprite != null)
                            AllTiles[2, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }

                }
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[1, FoodNum].GetComponent<Image>().sprite == null)
                    {
                        AllTiles[1, FoodNum].Number = AllTiles[0, 1].Number;
                        AllTiles[0, 1].Number = 0;
                        if (AllTiles[0, 1].GetComponent<Image>().sprite != null)
                            AllTiles[1, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }
                }
                AllTiles[0, 1].GetComponent<Image>().sprite = null;
                AllTiles[0, 3].GetComponent<Image>().sprite = null;
                AllTiles[0, 1].Number = 0;
                AllTiles[0, 3].Number = 0;
                break;

            case 4:
                for (int FoodNum = 0; FoodNum < 5; FoodNum++)
                {
                    if (AllTiles[1, FoodNum].GetComponent<Image>().sprite == null && Player1Ingred[FoodNum] == 0)
                    {
                        AllTiles[1, FoodNum].Number = AllTiles[0, 0].Number;
                        Player1Ingred[FoodNum] = AllTiles[0, 0].Number;
                        AllTiles[0, 0].Number = 0;
                        if (AllTiles[0, 0].GetComponent<Image>().sprite != null)
                            AllTiles[1, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }

                }
                for (int FoodNum = 0; FoodNum < 5; FoodNum++)
                {
                    if (AllTiles[2, FoodNum].GetComponent<Image>().sprite == null && Player2Ingred[FoodNum] == 0)
                    {
                        AllTiles[2, FoodNum].Number = AllTiles[0, 1].Number;
                        Player2Ingred[FoodNum] = AllTiles[0, 1].Number;
                        AllTiles[0, 1].Number = 0;
                        if (AllTiles[0, 1].GetComponent<Image>().sprite != null)
                            AllTiles[2, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }
                }
                AllTiles[0, 0].GetComponent<Image>().sprite = null;
                AllTiles[0, 1].GetComponent<Image>().sprite = null;
                AllTiles[0, 0].Number = 0;
                AllTiles[0, 1].Number = 0;
                break;


            case 5:
                for (int FoodNum = 0; FoodNum < 5; FoodNum++)
                {
                    if (AllTiles[2, FoodNum].GetComponent<Image>().sprite == null && Player2Ingred[FoodNum] == 0)
                    {
                        AllTiles[2, FoodNum].Number = AllTiles[0, 1].Number;
                        Player2Ingred[FoodNum] = AllTiles[0, 1].Number;
                        AllTiles[0, 1].Number = 0;
                        if (AllTiles[0, 1].GetComponent<Image>().sprite != null)
                            AllTiles[2, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }

                }
                for (int FoodNum = 0; FoodNum < 5; FoodNum++)
                {
                    if (AllTiles[1, FoodNum].GetComponent<Image>().sprite == null && Player1Ingred[FoodNum] == 0)
                    {
                        AllTiles[1, FoodNum].Number = AllTiles[0, 0].Number;
                        Player1Ingred[FoodNum] = AllTiles[0, 0].Number;
                        AllTiles[0, 0].Number = 0;
                        if (AllTiles[0, 0].GetComponent<Image>().sprite != null)
                            AllTiles[1, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }
                }
                AllTiles[0, 1].GetComponent<Image>().sprite = null;
                AllTiles[0, 0].GetComponent<Image>().sprite = null;
                AllTiles[0, 1].Number = 0;
                AllTiles[0, 0].Number = 0;
                break;
        }
    }

    public void Randomset()
    {
        if (oshita)
        {
            temp = Random.Range(0, 4);
            OdaiText.text = Library.Instance.Categorys[temp].CategoryName;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            oshita = !oshita;
            if (oshita)
                Debug.Log("Start");
            else
            {
				if (GameCount == 0)
					state = State.choicefood;
                else
                    state = State.secondchoicefood;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 24; i++)
        {
            if (i >= 0 && i < 4)
            {
                if (AllTiles[0, i].Number != 0)
                {
                    PlusScoreText[i].enabled = true;
                    MinusScoreText[i].enabled = true;
                    PlusScoreText[i].text = Library.Instance.Ingreds[AllTiles[0, i].Number].IngredPlusScore.ToString();
                    MinusScoreText[i].text = Library.Instance.Ingreds[AllTiles[0, i].Number].IngredMinusScore.ToString();
                }
                else
                {
                    PlusScoreText[i].enabled = false;
                    MinusScoreText[i].enabled = false;
                }
            }
            if (i >= 4 && i < 14)
            {
                if (AllTiles[1, i-4].Number != 0)
                {
                    PlusScoreText[i].enabled = true;
                    MinusScoreText[i].enabled = true;
                    PlusScoreText[i].text = Library.Instance.Ingreds[AllTiles[1, i-4].Number].IngredPlusScore.ToString();
                    MinusScoreText[i].text = Library.Instance.Ingreds[AllTiles[1, i-4].Number].IngredMinusScore.ToString();
                }
                else
                {
                    PlusScoreText[i].enabled = false;
                    MinusScoreText[i].enabled = false;
                }
            }
            if (i >= 14 && i < 24)
            {
                if (AllTiles[2, i-14].Number != 0)
                {
                    PlusScoreText[i].enabled = true;
                    MinusScoreText[i].enabled = true;
                    PlusScoreText[i].text = Library.Instance.Ingreds[AllTiles[2, i-14].Number].IngredPlusScore.ToString();
                    MinusScoreText[i].text = Library.Instance.Ingreds[AllTiles[2, i-14].Number].IngredMinusScore.ToString();
                }
                else
                {
                    PlusScoreText[i].enabled = false;
                    MinusScoreText[i].enabled = false;
                }
            }
        }
        if (GameCount == 3)
        {
            DialogPanel.enabled = true;
            DialogText.enabled = true;
            DialogText.text = "GM「Spaceボタンを押すと結果発表にいくよ！」";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DialogPanel.enabled = false;
                DialogText.enabled = false;
                SceneManager.LoadScene("Result");
            }
        }
        else if (state == State.tugi)
        {

            STileText.enabled = false;
            ShitaTileText.enabled = false;
            STile.enabled = false;
            ShitaTile.enabled = false;
            WText.enabled = false;
            UeTileText.enabled = false;
            if (GameCount == 2)
            {
                Player1Ingred = new int[10];
                Player2Ingred = new int[10];
            }

            state = State.secondready;

        }
        else if (state == State.secondready)
        {
            Randomset();

        }
        else if (state == State.firstready)
        {
            Randomset();
        }
        else if (state == State.secondchoicefood)
        {
            if (AllTiles[0, 0].GetComponent<Image>().sprite == null && AllTiles[0, 1].GetComponent<Image>().sprite == null && GameProgress == 0)
            {
                if (AllTiles[1, 4].GetComponent<Image>().sprite == null && AllTiles[2, 4].GetComponent<Image>().sprite == null && GameProgress == 0)
                {
                    WText.enabled = false;
                    UeTileText.enabled = false;
                    StartCoroutine(DelayGenerate(2, 0.5f));
                    GameProgress = 0;
                }
                else
                {
                    Debug.Log("プレイヤーの食材がいっぱいになったよ");
                    state = State.result;
                }
            }
            else if (GameProgress == 0)
            {
                IngredNum1 = AllTiles[0, 0].Number;
                IngredNum2 = AllTiles[0, 1].Number;
                DialogPanel.enabled = true;
                DialogText.enabled = true;
                if (P1Score >= P2Score)
                {
                    DialogText.text = "GM「↑ボタンを押すとオークション！↓ボタンを押すとPlayer" + 1 + "が先行で食材が選べるよ！";
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        SceneManager.LoadScene("AuctionScene");
                    }
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        DialogPanel.enabled = true;
                        DialogText.enabled = false;
                        GameProgress = 1;
                        if (P1Score >= P2Score)
                        {
                            PrecedNum = 1;
                        }
                        else
                        {
                            PrecedNum = 2;
                        }
                    }
                }
                else
                {
                    DialogText.text = "GM「Wボタンを押すとオークション！Sボタンを押すとPlayer" + 2 + "が先行で食材が選べるよ！";
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        SceneManager.LoadScene("AuctionScene");
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        DialogPanel.enabled = true;
                        DialogText.enabled = false;
                        GameProgress = 1;
                        if (P1Score >= P2Score)
                        {
                            PrecedNum = 1;
                        }
                        else
                        {
                            PrecedNum = 2;
                        }
                    }
                }
            }
            if (GameProgress == 1)
            {
                AllTiles[0, 0].Number = IngredNum1;
                AllTiles[0, 1].Number = IngredNum2;
                STileText.enabled = false;
                ShitaTileText.enabled = false;
                STile.enabled = false;
                ShitaTile.enabled = false;
                WText.enabled = true;
                UeTileText.enabled = true;
                if (PrecedNum == 1)
                {
                    WText.text = "A";
                    UeTileText.text = "D";
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        OnClickAct(4);
                        GameProgress = 0;
                    }
                    else if (Input.GetKeyDown(KeyCode.D))
                    {
                        OnClickAct(5);
                        GameProgress = 0;
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        if (Player1Ingred[i] != 0)
                            AllTiles[1, i].Number = Player1Ingred[i];
                        if (Player2Ingred[i] != 0)
                            AllTiles[2, i].Number = Player2Ingred[i];
                    }
                }
                else if (PrecedNum == 2)
                {
                    WText.text = "←";
                    UeTileText.text = "→";
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        OnClickAct(4);
                        GameProgress = 0;
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        OnClickAct(5);
                        GameProgress = 0;
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        if (Player1Ingred[i] != 0)
                            AllTiles[1, i].Number = Player1Ingred[i];
                        if (Player2Ingred[i] != 0)
                            AllTiles[2, i].Number = Player2Ingred[i];
                    }
                }
            }
            
        }
        else if (state == State.choicefood)
        {
            if (AllTiles[0, 1].GetComponent<Image>().sprite == null && AllTiles[0, 0].GetComponent<Image>().sprite == null && AllTiles[0, 1].GetComponent<Image>().sprite == null && AllTiles[0, 2].GetComponent<Image>().sprite == null && AllTiles[0, 3].GetComponent<Image>().sprite == null)
            {
                if (AllTiles[1, 9].GetComponent<Image>().sprite == null&& AllTiles[2, 9].GetComponent<Image>().sprite == null)
                {
                    StartCoroutine(DelayGenerate(4,0.5f));
                }
                else
                {
                    Debug.Log("プレイヤーの食材がいっぱいになったよ");
                    state = State.result;
                }
            }
            if (GenerateStop == true)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    OnClickAct(1);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    OnClickAct(3);
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    OnClickAct(0);
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    OnClickAct(2);
                }
            }
        }
        else if (state == State.result)
        {
            // Debug.Log("results");
            if (count == 0)
            {
                DialogPanel.enabled = true;
                DialogText.enabled = true;
                DialogText.text = "GM「Spaceボタンを押すと正解の料理を発表！」";
            }else if (count == 1)
            {
                DialogPanel.enabled = true;
                DialogText.enabled = true;
                StartCoroutine(DelayDialog(1f));
            }
            if (Input.GetKeyDown(KeyCode.Space) && count == 0)
            {
                DialogPanel.enabled = false;
                DialogText.enabled = false;
                temp2 = Random.Range(0, 3);
                ResultFood.GetComponent<Image>().sprite = Library.Instance.Categorys[temp].Foods[temp2].FoodSprite;
                ResultPanel.enabled = true;
                ResultFood.enabled = true;
                count++;
                for (int j = 1; j < 9; j++)
                {
                    temp1[j] = Library.Instance.Categorys[temp].Foods[temp2].Answers[j - 1].AnswerNumber;
                    if (Library.Instance.Ingreds[temp1[j]].IngredSprite != null)
                    {
                        Food[j - 1].enabled = true;
                        Food[j - 1].GetComponent<Image>().sprite = Library.Instance.Ingreds[temp1[j]].IngredSprite;
                    }
                    else
                    {
                        Food[j - 1].enabled = false;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.A) && count == 1)
            {
                DialogPanel.enabled = false;
                DialogText.enabled = false;
                ResultPanel.enabled = false;
                ResultFood.enabled = false;
                for (int i = 0; i < 8; i++)
                {
                    Food[i].enabled = false;
                }
                state = State.resultPlayer;
            }
        }
        else if (state == State.resultPlayer)
        {
            for (int h = 1; h <= 2; h++)
            {
                for (int g = 0; g <= 9; g++)
                {
                    for (int j = 1; j < 9; j++)
                    {
                        if (AllTiles[h, g].Number == temp1[j] && AllTiles[h, g].Number !=0)
                        {
                            Result[(h * 10 + g) - 10].sprite = Maru;
                            Result[(h * 10 + g) - 10].enabled = true;
                            if (h == 1)
                                P1Score += Library.Instance.Ingreds[temp1[j]].IngredPlusScore;
                            if (h == 2)
                                P2Score += Library.Instance.Ingreds[temp1[j]].IngredPlusScore;
                            break;
                        }
                        else
                        {
                        }
                    }
                }
            }
            for (int j = 0; j < 20; j++)
            {
                if (Result[j].sprite == null)
                {
                    Result[j].sprite = Batu;
                    if (j >= 0 && j < 10)
                        P1Score += Library.Instance.Ingreds[AllTiles[1,j%10].Number].IngredMinusScore;
                    if (j >= 10 && j < 20)
                        P2Score += Library.Instance.Ingreds[AllTiles[2,j%10].Number].IngredMinusScore;

                    if (GameCount == 1 || GameCount == 2)
                    {
                        Result[0].enabled = true;
                        Result[1].enabled = true;
                        Result[2].enabled = true;
                        Result[3].enabled = true;
                        Result[4].enabled = true;
                        Result[5].enabled = false;
                        Result[6].enabled = false;
                        Result[7].enabled = false;
                        Result[8].enabled = false;
                        Result[9].enabled = false;
                        Result[10].enabled = true;
                        Result[11].enabled = true;
                        Result[12].enabled = true;
                        Result[13].enabled = true;
                        Result[14].enabled = true;
                        Result[15].enabled = false;
                        Result[16].enabled = false;
                        Result[17].enabled = false;
                        Result[18].enabled = false;
                        Result[19].enabled = false;
                    }
                    else
                        Result[j].enabled = true;
                }
            }
            state = State.score;
        }
        else if (state == State.score)
        {
            ScoreText1.text = P1Score.ToString();
            ScoreText2.text = P2Score.ToString();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Main");
                GameCount++;
            }

        }
    }
}