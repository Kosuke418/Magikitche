using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Auction : MonoBehaviour
{
    [SerializeField]
    Text p1ScoreText, p2ScoreText, p1BidValueText, p2BidValueText, auctionTimer;
    //各プレイヤーの得点、入札額、タイマーの変数
    [SerializeField]
    Image foodImage;
    [SerializeField]
    Image[] endImage = new Image[3];
    [SerializeField]
    Text[] endText = new Text[2];
    [SerializeField]
    AudioClip soundMoney1, soundMoney2;
    AudioSource audioMoney1, audioMoney2;
    private int player, kamenRiderKuronos, kamenRiderZeroOne, pcount;//playerは現在の先行プレイヤーと入札ターンのプレイヤーの判定に使う。
    int[] textCase = new int[2];
    private float R, G, B, kamenRider = 5f;

    void Start()
    {
        Debug.Log("スタート");

        //結果表示画面を不可視化
        for (int count = 0; count < 3; count++)
        {
            R = endImage[count].GetComponent<Image>().color.r;
            G = endImage[count].GetComponent<Image>().color.g;
            B = endImage[count].GetComponent<Image>().color.b;
            endImage[count].GetComponent<Image>().color = new Color(R, G, B, 0.0f);
            Debug.Log(R + " " + G + " " + B);
        }
        for (int count = 0; count < 2; count++)
        {
            R = endText[count].GetComponent<Text>().color.r;
            G = endText[count].GetComponent<Text>().color.g;
            B = endText[count].GetComponent<Text>().color.b;
            endText[count].GetComponent<Text>().color = new Color(R, G, B, 0.0f);
        }

        p1BidValueText.text = "0";
        p2BidValueText.text = "0";

        //プレイヤースコアテキストを設定
        p1ScoreText.text = (MainGameManager2.P1Score).ToString();
        p2ScoreText.text = (MainGameManager2.P2Score).ToString();

        //ここから食材画像を指定
        foodImage.sprite = Library.Instance.Ingreds[MainGameManager2.IngredNum].IngredSprite;

        //AudioSourceConmponentを取得
        audioMoney1 = GetComponent<AudioSource>();
        audioMoney2 = GetComponent<AudioSource>();
        audioMoney2.loop = true;

        pcount = 0;

    }

    private void Update()
    {
        ConSwitch();

        kamenRider -= Time.deltaTime;
        kamenRiderKuronos = (int)kamenRider;
        kamenRiderZeroOne = (int)((kamenRider - kamenRiderKuronos) * 100);
        if (kamenRider >= 0)
        {
            auctionTimer.text = kamenRiderKuronos.ToString() + "." + kamenRiderZeroOne;
        }
        if (pcount == 0)
        {
            if (kamenRider <= 0)
            {

                textCase[0] = int.Parse(p1BidValueText.text);
                textCase[1] = int.Parse(p2BidValueText.text);
                if (textCase[0] < textCase[1])
                {
                    MainGameManager2.P2Score -= int.Parse(p2BidValueText.text);
                    MainGameManager2.PrecedNum = 2;
                    EndAuction();
                }
                else if (textCase[0] > textCase[1])
                {
                    MainGameManager2.P1Score -= int.Parse(p1BidValueText.text);
                    MainGameManager2.PrecedNum = 1;
                    EndAuction();
                }
                else if (textCase[0] == textCase[1])
                {
                    kamenRider = 5;
                }
            }
        }
        else if (pcount == 1)
        {
            if (kamenRider <= 0) {
                MainGameManager2.GameProgress = 1;
                SceneManager.LoadScene("Main");

            }
        }
    }

    private void Onclick(int bNum, int bidPlayer)
    {
        textCase[0] = int.Parse(p1BidValueText.text);
        textCase[1] = int.Parse(p2BidValueText.text);
        if (bNum == 0)
        {
            Debug.Log("0の処理");
            if (bidPlayer == 1 && textCase[0] <= textCase[1])
            {
                Debug.Log("P1の処理0");
                p1BidValueText.text = (textCase[1] + 10).ToString();
                audioMoney1.PlayOneShot(soundMoney1);
            }
            else if (bidPlayer == 2 && textCase[0] >= textCase[1])
            {
                Debug.Log("P2の処理0");
                p2BidValueText.text = (textCase[0] + 10).ToString();
                audioMoney1.PlayOneShot(soundMoney1);
            }
        }
        else if (bNum == 1)
        {
            Debug.Log("1の処理");
            if (bidPlayer == 1 && textCase[0] <= textCase[1])
            {
                Debug.Log("P1の処理1");
                p1BidValueText.text = (textCase[1] + 100).ToString();
                audioMoney1.PlayOneShot(soundMoney1);
            }
            else if (bidPlayer == 2&& textCase[0] >= textCase[1])
            {
                if (textCase[1] >= textCase[0])
                {
                    Debug.Log("P2の処理1");
                    p2BidValueText.text = (textCase[0] + 100).ToString();
                    audioMoney1.PlayOneShot(soundMoney1);
                }
            }
            
        }

    }

    public void EndAuction()
    {
        //▼ここに入札者名、入札額を表示するプログラム▼
        //結果表示画面を可視化
        for (int n = 0; n < 3; n++)
        {
            R = endImage[n].GetComponent<Image>().color.r;
            G = endImage[n].GetComponent<Image>().color.g;
            B = endImage[n].GetComponent<Image>().color.b;
            endImage[n].GetComponent<Image>().color = new Color(R, G, B, 1.0f);
            Debug.Log(R + " " + G + " " + B);
        }
        for (int m = 0; m < 2; m++)
        {
            R = endText[m].GetComponent<Text>().color.r;
            G = endText[m].GetComponent<Text>().color.g;
            B = endText[m].GetComponent<Text>().color.b;
            endText[m].GetComponent<Text>().color = new Color(R, G, B, 1.0f);
        }
        if (MainGameManager2.PrecedNum == 1)
        {
            endText[0].text = "Player1が" + textCase[0].ToString() + "点で落札しました";
        }else if (MainGameManager2.PrecedNum == 2)
        {
            endText[0].text = "Player2が" + textCase[1].ToString() + "点で落札しました";
        }
            //▲ここに入札者名、入札額を表示するプログラム▲

        //SEを流すプログラム
        audioMoney2.PlayOneShot(soundMoney2);
        pcount = 1;
        //時間経過を表示を表示
        kamenRider = 3;
    }

    void ConSwitch()
    {
        if (Input.GetKeyDown("joystick 1 button 4") || Input.GetKeyDown(KeyCode.A))
        {
            Onclick(0, 1);
            Debug.Log("j1Button4(LB)");
        }
        if (Input.GetKeyDown("joystick 1 button 5") || Input.GetKeyDown(KeyCode.D))
        {
            Onclick(1, 1);
            Debug.Log("j1Button5(RB)");
        }

        if (Input.GetKeyDown("joystick 2 button 4") || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Onclick(0, 2);
            Debug.Log("j2Button4(LB)");
        }
        if (Input.GetKeyDown("joystick 2 button 5") || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Onclick(1, 2);
            Debug.Log("j2Button5(RB)");
        }
    }

}