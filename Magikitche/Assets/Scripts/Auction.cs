using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Auction : MonoBehaviour
{
    [SerializeField]
    Text p1ScoreText, p2ScoreText, p1BidValueText, p2BidValueText, auctionTimer, lbText, rbText;
    //各プレイヤーの得点、入札額、タイマーの変数
    [SerializeField]
    GameObject P1Sprite, P2Sprite, Curtain1, Curtain2, BronzCoin, GoldCoin;
    [SerializeField]
    Image foodImage;
    [SerializeField]
    Image[] endImage = new Image[2];
    [SerializeField]
    Text[] endText = new Text[1];
    [SerializeField]
    AudioClip soundMoney1, soundMoney2, audioWood;
    AudioSource audioMoney1;
    int player, kamenRiderKuronos, kamenRiderZeroOne, pcount, coinCount, charaCount, bidCount, p1BidCount, p2BidCount;//playerは現在の先行プレイヤーと入札ターンのプレイヤーの判定に使う。
    int[] textCase = new int[2];
    float R, G, B, kamenRider, charaTransform, curtainTransform, x, y, z, p1TenCount, p2TenCount;

    void Start()
    {
        Debug.Log("スタート");
        
        //結果表示画面を不可視化
        for (int count = 0; count < 2; count++)
        {
            R = endImage[count].GetComponent<Image>().color.r;
            G = endImage[count].GetComponent<Image>().color.g;
            B = endImage[count].GetComponent<Image>().color.b;
            endImage[count].GetComponent<Image>().color = new Color(R, G, B, 0.0f);
            Debug.Log(R + " " + G + " " + B);
        }
        R = endText[0].GetComponent<Text>().color.r;
        G = endText[0].GetComponent<Text>().color.g;
        B = endText[0].GetComponent<Text>().color.b;
        endText[0].GetComponent<Text>().color = new Color(R, G, B, 0.0f);

        p1BidValueText.text = "0";
        p2BidValueText.text = "0";

        //プレイヤースコアテキストを設定
        p1ScoreText.text = (MainGameManager2.P1Score).ToString();
        p2ScoreText.text = (MainGameManager2.P2Score).ToString();

        //操作テキストを設定
        lbText.text = "+10";
        rbText.text = "+100";

        //ここから食材画像を指定
        foodImage.sprite = Library.Instance.Ingreds[MainGameManager2.IngredNum].IngredSprite;

        
        //AudioSourceConmponentを取得
        audioMoney1 = GetComponent<AudioSource>();
        /*
        audioMoney2 = GetComponent<AudioSource>();
        audioMoney2.loop = (true);
        */

        //初期化一覧
        x = 50;
        y = -40;
        z = 100;
        p1BidCount = 0;
        p2BidCount = 0;
        p1TenCount = 0;
        p2TenCount = 0;
        charaCount = 1;
        coinCount = 0;
        bidCount = 0;
        pcount = 0;
        kamenRider = 5;
        curtainTransform = 150;
    }

    private void Update()
    {
        CharaMove();

        kamenRider -= Time.deltaTime;
        kamenRiderKuronos = (int)kamenRider;
        kamenRiderZeroOne = (int)((kamenRider - kamenRiderKuronos) * 100);
        auctionTimer.text = kamenRiderKuronos.ToString() + ":" + kamenRiderZeroOne;
        if (pcount == 0)
        {
            ConSwitch();
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
                    kamenRider = 3;
                }
            }
        }
        else if (pcount == 1)
        {
            curtainTransform -= 1.05f;
            Curtain1.transform.position = new Vector3(-curtainTransform, 0, 100);
            Curtain2.transform.position = new Vector3(curtainTransform, 0, 100);
            if (coinCount % 3 == 0)
            {
                audioMoney1.PlayOneShot(soundMoney2);
                coinCount+=1;
            }
            if (kamenRider <= 0) {
                MainGameManager2.GameProgress = 1;
                SceneManager.LoadScene("Main");
            }
        }
    }

    private void BidCheck(int bNum, int bidPlayer)
    {
        textCase[0] = int.Parse(p1BidValueText.text);
        textCase[1] = int.Parse(p2BidValueText.text);
        if (bNum == 0)
        {
            if (bidPlayer == 1 && textCase[0] <= textCase[1])
            {
                p1BidValueText.text = (textCase[1] + 10).ToString();
                audioMoney1.PlayOneShot(soundMoney1);
                CoinCreate(bNum, bidPlayer);
            }
            else if (bidPlayer == 2 && textCase[0] >= textCase[1])
            {
                p2BidValueText.text = (textCase[0] + 10).ToString();
                audioMoney1.PlayOneShot(soundMoney1);
                CoinCreate(bNum, bidPlayer);
            }
        }
        else if (bNum == 1)
        {
            if (bidPlayer == 1 && textCase[0] <= textCase[1])
            {
                p1BidValueText.text = (textCase[1] + 100).ToString();
                audioMoney1.PlayOneShot(soundMoney1);
                CoinCreate(bNum, bidPlayer);
            }
            else if (bidPlayer == 2&& textCase[0] >= textCase[1])
            {
                p2BidValueText.text = (textCase[0] + 100).ToString();
                audioMoney1.PlayOneShot(soundMoney1);
                CoinCreate(bNum, bidPlayer);
            }   
        }

        //時間追加のプログラム:最大でも3s追加は3回, 2s追加は2回, 1s追加は1回
        if (kamenRider <= 3 && bidCount <= 2)
        {
            kamenRider = 3;
            bidCount++;
        }else if(kamenRider <= 2 && bidCount <= 5)
        {
            kamenRider = 2;
            bidCount++;
        }else if(kamenRider <= 1 && bidCount <= 6)
        {
            kamenRider = 1;
            bidCount++;
        }
    }

    public void EndAuction()
    {
        //▼ここに入札者名、入札額を表示するプログラム▼
        //結果表示画面を可視化
        for (int n = 0; n < 2; n++)
        {
            R = endImage[n].GetComponent<Image>().color.r;
            G = endImage[n].GetComponent<Image>().color.g;
            B = endImage[n].GetComponent<Image>().color.b;
            endImage[n].GetComponent<Image>().color = new Color(R, G, B, 1.0f);
            Debug.Log(R + " " + G + " " + B);
        }
        R = endText[0].GetComponent<Text>().color.r;
        G = endText[0].GetComponent<Text>().color.g;
        B = endText[0].GetComponent<Text>().color.b;
        endText[0].GetComponent<Text>().color = new Color(R, G, B, 1.0f);

        if (MainGameManager2.PrecedNum == 1)
        {
            endText[0].text = "Player1が\n" + textCase[0].ToString() + "点で\n落札しました";
        }else if (MainGameManager2.PrecedNum == 2)
        {
            endText[0].text = "Player2が\n" + textCase[1].ToString() + "点で\n落札しました";
        }
        //▲ここに入札者名、入札額を表示するプログラム▲

        //操作テキストの更新
        lbText.text = "";
        rbText.text = "";

        //SEを流すプログラム
        audioMoney1.PlayOneShot(audioWood);

        pcount = 1;

        //時間経過を表示を表示
        kamenRider = 2.5f;
    }

    void CharaMove()
    {
       // charaTransform = Mathf.Abs(Mathf.Sin(Time.time * 10));
        if (charaCount > 0)
        {
            charaTransform += 0.1f;
            P1Sprite.transform.eulerAngles = new Vector3(0, 0, -charaTransform);
            P2Sprite.transform.eulerAngles = new Vector3(0, 0, charaTransform);
            if (charaTransform >= 5)
            {
                charaCount=-1;
            }
        }else if (charaCount < 0)
        {
            charaTransform -= 0.1f;
            P1Sprite.transform.eulerAngles = new Vector3(0, 0, -charaTransform);
            P2Sprite.transform.eulerAngles = new Vector3(0, 0, charaTransform);
            if (charaTransform <= -10)
            {
                charaCount = 1;
            }
        }
    }
    //▲キャラ動作▲

    void ConSwitch()
    {
        if (Input.GetKeyDown("joystick 1 button 4") || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown("joystick 1 button 2"))
        {
            BidCheck(0, 1);
            Debug.Log("j1Button4(LB)");
        }
        if (Input.GetKeyDown("joystick 1 button 5") || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown("joystick 1 button 1"))
        {
            BidCheck(1, 1);
            Debug.Log("j1Button5(RB)");
        }

        if (Input.GetKeyDown("joystick 2 button 4") || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("joystick 2 button 2"))
        {
            BidCheck(0, 2);
            Debug.Log("j2Button4(LB)");
        }
        if (Input.GetKeyDown("joystick 2 button 5") || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("joystick 2 button 1"))
        {
            BidCheck(1, 2);
            Debug.Log("j2Button5(RB)");
        }
    }

    void CoinCreate(int bidOR, int playerNumber)
    {
        if (bidOR == 0)
        {
            if (playerNumber == 1)
            {
                Instantiate(BronzCoin, new Vector3(-(x-(4 * p1TenCount)), y+(3*p1BidCount), z), Quaternion.identity);
                p1BidCount++;
            }
            else if (playerNumber == 2)
            {
                Instantiate(BronzCoin, new Vector3(x - (4 * p2TenCount), y+ (3 * p2BidCount), z), Quaternion.identity);
                p2BidCount++;
            }
        }else if (bidOR == 1)
        {
            if (playerNumber == 1)
            {
                Instantiate(GoldCoin, new Vector3(-(x - (4 * p1TenCount)), y+ (3 * p1BidCount), z), Quaternion.identity);
                p1BidCount++;
            }
            else if (playerNumber == 2)
            {
                Instantiate(GoldCoin, new Vector3(x - (4 * p2TenCount), y + (3 * p2BidCount), z), Quaternion.identity);
                p2BidCount++;
            }
        }
        if (p1BidCount == 9)
        {
            p1BidCount = 0;
            p1TenCount++;
        }
        if (p2BidCount == 9)
        {
            p2BidCount = 0;
            p2TenCount++;
        }
    }

}