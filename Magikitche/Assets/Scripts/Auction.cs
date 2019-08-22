using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Auction : MonoBehaviour
{
    public int value, player;//playerは現在の先行プレイヤーと入札ターンのプレイヤーの判定に使う。
    public Text BidValueText;
    public Text NowValueText;
    public Text PlayerText;
    public Image picture1, picture2;
    void Start()
    {
        Debug.Log(value + "スタート");
        value = 0;

      //int型のplayerはターンプレイヤーの数値を持つ（1Pなら1、2Pなら2）
      player = MainGameManager.PrecedNum; //playerは先行のプレイヤーであり、ターンプレイヤー


        Debug.Log("Player" + player + "が先行権獲得に近いです。");
        this.PlayerText.text = "Player" + player.ToString() + "のターンです";

      //▼ここから食材画像の指定▼
      picture1.sprite = Library.Instance.Ingreds[MainGameManager.IngredNum1].IngredSprite;
      picture2.sprite = Library.Instance.Ingreds[MainGameManager.IngredNum2].IngredSprite;
      //▲ここまで食材画像の指定▼

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
            if (int.Parse(this.NowValueText.text) <= int.Parse(this.BidValueText.text))
            {
                this.NowValueText.text = this.BidValueText.text;
                value = int.Parse(this.NowValueText.text);
            }
        }
        else if (BtonNum == 3)
        {
            value -= 10;
            if (int.Parse(this.NowValueText.text) <= int.Parse(this.BidValueText.text))
            {
                this.NowValueText.text = this.BidValueText.text;
                value = int.Parse(this.NowValueText.text);
            }
        }
        else if (BtonNum == 4)
        {
            Debug.Log("￥" + this.BidValueText.text + "で落札");
            Debug.Log("決定");
          if (player == 1)
          {
              MainGameManager.PrecedNum = player + 1;
              MainGameManager.P2Score -= value;
          }
          else if (player == 2)
          {
              MainGameManager.PrecedNum = player - 1;
              MainGameManager.P1Score -= value;
          }

            //ここで返り値としてP1Score, P2Score（競り勝ったほうからマイナス済みの数値）とplayer（競り勝ったほう）を返す

            MainGameManager.GameProgress = 1;
            SceneManager.LoadScene("Main");
        }
        else if (BtonNum == 5)
        {
            if (int.Parse(this.BidValueText.text) < value)
            {
                this.BidValueText.text = value.ToString();
                Debug.Log("金額Up");

                //▼仮のプレイヤー
                if (player == 1)
                {
                    player = 2;
                }
                else if (player == 2)
                {
                    player = 1;
                }
                this.PlayerText.text = "Player" + player.ToString() + "のターンです";
                //▲仮のプレイヤー
            }
            else
            {
                Debug.Log(value);
                Debug.Log("額が足りない");
                this.NowValueText.text = this.BidValueText.text;
                value = int.Parse(this.NowValueText.text);
            }
        }
        this.NowValueText.text = value.ToString();
        Debug.Log(value + "になりました");
    }
}