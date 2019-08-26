﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameManager2 : MonoBehaviour
{

    private Tile[,] AllTiles = new Tile[10, 10];
    private List<Tile> EmptyTiles = new List<Tile>();
    private List<Tile[]> columns = new List<Tile[]>();
    private int P1 = 2;
    private int P2 = 2;
    int seconds;
    int sseconds;

    public Image Select10;
    public Image Select11;
    public Image Select20;
    public Image Select21;
    public Text timerText;
    public float totalTime;
    public static int IngredNum;
    public static int TurnCount = 1;
    public static bool GenerateStop;
    public static int P1Score = 100;
    public static int P2Score = 100;
    public Image[] MadeFood1;
    public Image[] MadeFood2;
    public Text ScoreText1;
    public Text ScoreText2;

    public enum State
    {
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
    }

    private IEnumerator DelayGenerate(int num, float waitTime)
    {
        GenerateStop = false;
        yield return new WaitForSeconds(waitTime);
        Generate(num);
        yield return new WaitForSeconds(waitTime);
        GenerateStop = true;
    }

    void Generate(int num)
    {
        int start = 1;
        int end = 18;

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

        Debug.Log(TurnCount);

        ScoreText1.text = P1Score.ToString();
        ScoreText2.text = P2Score.ToString();

        if (state == State.Generate)
        {
            if (AllTiles[0, 0].Number == 0 && AllTiles[0, 1].Number == 0)
            {
                StartCoroutine(DelayGenerate(2, 0f));
            }
            state = State.Choice;
        }
        else if (state == State.Choice)
        {
            totalTime -= Time.deltaTime;
            seconds = (int)totalTime;
            sseconds = (int)((totalTime - seconds) * 100);
            if (seconds >= 0 && sseconds >= 0)
            {
                timerText.text = seconds.ToString() + ":" + sseconds.ToString();
            }
            if (P1 == 0)
            {
                Select10.enabled = true;
                Select11.enabled = false;
            }
            else
            {
                Select11.enabled = true;
                Select10.enabled = false;
            }
            if (P2 == 0)
            {
                Select20.enabled = true;
                Select21.enabled = false;
            }
            else
            {
                Select21.enabled = true;
                Select20.enabled = false;
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
                if (Input.GetKeyDown(KeyCode.A))
                {
                    OnClickAct(0);
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    OnClickAct(1);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    OnClickAct(2);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    OnClickAct(3);
                }
            }
            if (totalTime <= 0)
            {
                state = State.MakeFood;
            }
        }
        else if (state == State.MakeFood)
        {
            if (P1 == 0 && P2 == 0)
            {
                IngredNum = AllTiles[0, 0].Number;
                SceneManager.LoadScene("AuctionScene");
            }
            else if (P1 == 1 && P2 == 1)
            {
                IngredNum = AllTiles[0, 1].Number;
                SceneManager.LoadScene("AuctionScene");
            }
            else if (P1 == 0 && P2 ==1)
            {
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[1, FoodNum].Number == 0)
                    {
                        AllTiles[1, FoodNum].Number = AllTiles[0, 0].Number;
                        AllTiles[0, 0].Number = 0;
                        break;
                    }
                }
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[2, FoodNum].Number == 0)
                    {
                        AllTiles[2, FoodNum].Number = AllTiles[0, 1].Number;
                        AllTiles[0, 1].Number = 0;
                        break;
                    }
                }
            }
            else if (P1 == 1 && P2 == 0)
            {
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[2, FoodNum].Number == 0)
                    {
                        AllTiles[2, FoodNum].Number = AllTiles[0, 0].Number;
                        AllTiles[0, 0].Number = 0;
                        break;
                    }
                }
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[1, FoodNum].Number == 0)
                    {
                        AllTiles[1, FoodNum].Number = AllTiles[0, 1].Number;
                        AllTiles[0, 1].Number = 0;
                        break;
                    }
                }
            }
            if (AllTiles[0, 0].Number == 0 && AllTiles[0, 1].Number == 0)
            {
                if (AllTiles[1, 9].Number == 0 && AllTiles[2, 9].Number == 0)
                {
                    StartCoroutine(DelayGenerate(2, 0f));
                    TurnCount++;
                    totalTime = 3;
                    state = State.Choice;
                }
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
            int[] Ingred1 = new int[10];
            bool[] IngredThis1 = new bool[10];
            int m1 = 0;
            bool CanMake1 = true;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    m1 = 0;
                    Ingred1 = new int[10];
                    for (int k = 0; k < 8; k++)
                    {
                        if (Library.Instance.Categorys[i].Foods[j].Answers[k].AnswerNumber != 0)
                        {
                            for (int l = 0; l < 10; l++)
                            {
                                //Debug.Log(Library.Instance.Categorys[i].CategoryName + "," + Library.Instance.Categorys[i].Foods[j].FoodName + "," + Library.Instance.Categorys[i].Foods[j].Answers[k].AnswerName + "," + l);
                                if (Library.Instance.Categorys[i].Foods[j].Answers[k].AnswerNumber == AllTiles[1, l].Number)
                                {
                                    //Debug.Log(Library.Instance.Categorys[i].Foods[j].FoodName + "の" + Library.Instance.Categorys[i].Foods[j].Answers[k].AnswerName + "があるよ");
                                    Ingred1[m1] = l;
                                    IngredThis1[k] = true;
                                    m1++;
                                    break;
                                }
                                else if (l == 9)
                                {
                                    //Debug.Log(Library.Instance.Categorys[i].Foods[j].FoodName + "の" + Library.Instance.Categorys[i].Foods[j].Answers[k].AnswerName + "がないよ");
                                    IngredThis1[k] = false;
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
                        }
                        Debug.Log(Library.Instance.Categorys[i].Foods[j].FoodName + "が作れる");
                        CanMake1 = true;
                        P1Score += 100;
                        for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                        {
                            if (MadeFood1[FoodNum].sprite == null)
                            {
                                MadeFood1[FoodNum].enabled = true;
                                MadeFood1[FoodNum].sprite = Library.Instance.Categorys[i].Foods[j].FoodSprite;
                                break;
                            }
                        }
                    }
                    else
                    {
                        CanMake1 = false;
                    }
                }
            }
            int[] Ingred2 = new int[10];
            bool[] IngredThis2 = new bool[10];
            int m2 = 0;
            bool CanMake2 = true;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    m2 = 0;
                    Ingred2 = new int[10];
                    for (int k = 0; k < 8; k++)
                    {
                        if (Library.Instance.Categorys[i].Foods[j].Answers[k].AnswerNumber != 0)
                        {
                            for (int l = 0; l < 10; l++)
                            {
                                //Debug.Log(Library.Instance.Categorys[i].CategoryName + "," + Library.Instance.Categorys[i].Foods[j].FoodName + "," + Library.Instance.Categorys[i].Foods[j].Answers[k].AnswerName + "," + l);
                                if (Library.Instance.Categorys[i].Foods[j].Answers[k].AnswerNumber == AllTiles[2, l].Number)
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
                        }
                        Debug.Log(Library.Instance.Categorys[i].Foods[j].FoodName + "が作れる");
                        CanMake2 = true;
                        P2Score += 100;
                        for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                        {
                            if (MadeFood2[FoodNum].sprite == null)
                            {
                                MadeFood2[FoodNum].enabled = true;
                                MadeFood2[FoodNum].sprite = Library.Instance.Categorys[i].Foods[j].FoodSprite;
                                break;
                            }
                        }
                    }
                    else
                    {
                        CanMake2 = false;
                    }
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
                AllTiles[2, 0].Number = 0;
                AllTiles[2, 1].Number = 0;
                AllTiles[2, 2].Number = 0;
                AllTiles[2, 3].Number = 0;
                AllTiles[2, 4].Number = 0;
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

        }
    }
}
