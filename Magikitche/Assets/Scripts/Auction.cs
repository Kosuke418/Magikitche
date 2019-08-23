using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Auction : MonoBehaviour
{
    public int value, player;//playerは現在の先行プレイヤーと入札ターンのプレイヤーの判定に使う。
    public Text PlayerText, BidValueText, NowValueText, P1ScoreText, P2ScoreText;
    public Image picture1, picture2;
    public Image[] End1 =new Image[3];
    public Text[] End2 = new Text[2];
    public Button ButtonSet, FoldSet, EndButton;
    public Button[] Whitning = new Button[6];
    int Turn;
    float R, G, B;

    void Start()
    {
        //ここから結果表示画面の不可視化
        for (int n = 0; n < 3; n++)
        {
            R = End1[n].GetComponent<Image>().color.r;
            G = End1[n].GetComponent<Image>().color.g;
            B = End1[n].GetComponent<Image>().color.b;
            End1[n].GetComponent<Image>().color = new Color(R, G, B, 0.0f);
            Debug.Log(R + " " + G + " " + B);
        }
        for (int m = 0; m < 2; m++)
        {
            R = End2[m].GetComponent<Text>().color.r;
            G = End2[m].GetComponent<Text>().color.g;
            B = End2[m].GetComponent<Text>().color.b;
            End2[m].GetComponent<Text>().color = new Color(R, G, B, 0.0f);
        }
        //ここまで結果表示画面の不可視化

        if (MainGameManager.P1Score >= MainGameManager.P2Score)
        {
            MainGameManager.PrecedNum = 2;
        }
        else
        {
            MainGameManager.PrecedNum = 1;
        }
        Debug.Log(value + "スタート");
        value = 30;
        //int型のplayerはターンプレイヤーの数値を持つ（1Pなら1、2Pなら2）
        player = MainGameManager.PrecedNum; //playerは先行のプレイヤーであり、ターンプレイヤー
        this.PlayerText.text = "Player" + player.ToString() + "のターンです";
        P1ScoreText.text = (MainGameManager.P1Score).ToString();
        P2ScoreText.text = (MainGameManager.P2Score).ToString();
        //▼ここから食材画像の指定▼
        picture1.sprite = Library.Instance.Ingreds[MainGameManager.IngredNum1].IngredSprite;
        picture2.sprite = Library.Instance.Ingreds[MainGameManager.IngredNum2].IngredSprite;
        //▲ここまで食材画像の指定▼
        ButtonSet.Select();
        Turn = 0;
        FoldSet.interactable = false;
        EndButton.interactable = false;
    }
    public void Onclick(int BtonNum)
    {//消費点の変化を扱う
        if (BtonNum == 0)
        {
            value += 100;
        }
        else if (BtonNum == 1)
        {
            value += 10;
        }
        else if (BtonNum == 2)
        {
            value -= 100;
            if (value <= int.Parse(this.BidValueText.text))
            {
                this.NowValueText.text = this.BidValueText.text;
                value = int.Parse(this.NowValueText.text);
            }
        }
        else if (BtonNum == 3)
        {
            value -= 10;
            if (value <= int.Parse(this.BidValueText.text))
            {
                this.NowValueText.text = this.BidValueText.text;
                value = int.Parse(this.NowValueText.text);
            }
        }
        else if (BtonNum == 4)
        {
            if (Turn == 0)
            {
                if (player == 1)
                {
                    MainGameManager.P1Score -= int.Parse(this.BidValueText.text) - 10;
                    MainGameManager.PrecedNum = player;
                    End2[0].text = "Player"+player+"が"+ (int.Parse(this.BidValueText.text) - 10).ToString() + "点で落札しました";
                }
                else
                {
                    MainGameManager.P2Score -= int.Parse(this.BidValueText.text) - 10;
                    MainGameManager.PrecedNum = player;
                    End2[0].text = "Player" + player + "が" + (int.Parse(this.BidValueText.text) - 10).ToString() + "点で落札しました";
                }
            }
            else if (player == 1)
            {
                MainGameManager.P2Score -= (int.Parse(this.BidValueText.text) - 10);
                MainGameManager.PrecedNum = player + 1;
                End2[0].text = "Player" + (player + 1).ToString() + "が" + (int.Parse(this.BidValueText.text) - 10).ToString() + "点で落札しました";
            }
            else if (player == 2)
            {
                MainGameManager.P1Score -= (int.Parse(this.BidValueText.text) - 10);
                MainGameManager.PrecedNum = player - 1;
                End2[0].text = "Player" + (player - 1).ToString() + "が" + (int.Parse(this.BidValueText.text) - 10).ToString() + "点で落札しました";
            }
            //ここで返り値としてP1Score, P2Score（競り勝ったほうからマイナス済みの数値）とplayer（競り勝ったほう）を返す



            //▼ここに入札者名、入札額を表示するプログラム▼
            //ここから結果表示画面の可視化
            for (int n = 0; n < 3; n++)
            {
                R = End1[n].GetComponent<Image>().color.r;
                G = End1[n].GetComponent<Image>().color.g;
                B = End1[n].GetComponent<Image>().color.b;
                End1[n].GetComponent<Image>().color = new Color(R, G, B, 1.0f);
                Debug.Log(R + " " + G + " " + B);
            }
            for (int m = 0; m < 2; m++)
            {
                R = End2[m].GetComponent<Text>().color.r;
                G = End2[m].GetComponent<Text>().color.g;
                B = End2[m].GetComponent<Text>().color.b;
                End2[m].GetComponent<Text>().color = new Color(R, G, B, 1.0f);
            }
            for(int i=0; i < 6; i++)
            {
                Whitning[i].interactable = false;
            }
            EndButton.interactable = true;
            EndButton.Select();
            //ここまで結果表示画面の可視化
            //▲ここに入札者名、入札額を表示するプログラム▲
        }
        else if (BtonNum == 5)
        {
            if (int.Parse(this.BidValueText.text) <= value)
            {
                FoldSet.interactable = true;
                this.BidValueText.text = (value+10).ToString();
                Debug.Log("金額Up");
                Debug.Log("ボタン5:  valueは:"+value + "、 入札金額欄は:"+ BidValueText.text+ "、 入札金額欄は:"+NowValueText.text);
                if (player == 1)
                {
                    player = 2;
                }
                else if (player == 2)
                {
                    player = 1;
                }
                this.PlayerText.text = "Player" + player.ToString() + "のターンです";
                this.NowValueText.text = this.BidValueText.text;
                value = int.Parse(this.NowValueText.text);
                Turn++;
            }
            else
            {
                Debug.Log(value);
                Debug.Log("額が足りない");
                
                this.NowValueText.text = this.BidValueText.text;
                value = int.Parse(this.NowValueText.text);
            }
        }

        P1ScoreText.text = (MainGameManager.P1Score).ToString();
        P2ScoreText.text = (MainGameManager.P2Score).ToString();
        this.NowValueText.text = value.ToString();
        Debug.Log(value + "になりました");
    }
    public void EndClick()
    {
        MainGameManager.GenerateStop = true;
        MainGameManager.GameProgress = 1;
        SceneManager.LoadScene("Main");
    }
}