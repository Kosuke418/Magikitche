using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour
{
    private Tile[,] AllTiles = new Tile[10,10];
    private List<Tile> EmptyTiles = new List<Tile>();
    private List<Tile[]> columns = new List<Tile[]>();

    private bool oshita;
    private int temp;
    public Text text;
    public Image ResultFood;


    public enum State{
        first,
        second,
        third,
        ready,
        choicefood,
        result,
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

        state = State.ready;
        oshita = false;
        ResultFood.enabled = false;
    }

    void Generate()
    {
        Tile[] LineOfTiles;
        LineOfTiles = columns[0];
        int start = 1;
        int end = 8;
        int count = 4;

        List<int> numbers = new List<int>();

        for(int i = start; i <= end; i++)
        {
            numbers.Add(i);
        }
        while(count-- > 0)
        {
            int index = Random.Range(0, numbers.Count);
            int randomNum = numbers[index];
            Debug.Log(randomNum);
            LineOfTiles[count].Number = randomNum;

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
                        AllTiles[1, FoodNum].GetComponent<Image>().sprite = AllTiles[0, 0].GetComponent<Image>().sprite;
                        if(AllTiles[0, 0].GetComponent<Image>().sprite!=null)
                             AllTiles[1, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }

                }
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[2, FoodNum].GetComponent<Image>().sprite == null)
                    {
                        AllTiles[2, FoodNum].GetComponent<Image>().sprite = AllTiles[0, 2].GetComponent<Image>().sprite;
                        if (AllTiles[0, 2].GetComponent<Image>().sprite != null)
                            AllTiles[2, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }
                }
                AllTiles[0, 0].GetComponent<Image>().sprite = null;
                AllTiles[0, 2].GetComponent<Image>().sprite = null;
                break;


            case 1:
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[2, FoodNum].GetComponent<Image>().sprite == null)
                    {
                        AllTiles[2, FoodNum].GetComponent<Image>().sprite = AllTiles[0, 1].GetComponent<Image>().sprite;
                        if (AllTiles[0, 1].GetComponent<Image>().sprite != null)
                            AllTiles[2, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }

                }
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[1, FoodNum].GetComponent<Image>().sprite == null)
                    {
                        AllTiles[1, FoodNum].GetComponent<Image>().sprite = AllTiles[0, 3].GetComponent<Image>().sprite;
                        if (AllTiles[0, 3].GetComponent<Image>().sprite != null)
                            AllTiles[1, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }
                }
                AllTiles[0, 1].GetComponent<Image>().sprite = null;
                AllTiles[0, 3].GetComponent<Image>().sprite = null;
                break;


            case 2:
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[1, FoodNum].GetComponent<Image>().sprite == null)
                    {
                        AllTiles[1, FoodNum].GetComponent<Image>().sprite = AllTiles[0, 2].GetComponent<Image>().sprite;
                        if (AllTiles[0, 2].GetComponent<Image>().sprite != null)
                            AllTiles[1, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }

                }
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[2, FoodNum].GetComponent<Image>().sprite == null)
                    {
                        AllTiles[2, FoodNum].GetComponent<Image>().sprite = AllTiles[0, 0].GetComponent<Image>().sprite;
                        if (AllTiles[0, 0].GetComponent<Image>().sprite != null)
                            AllTiles[2, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }
                }
                AllTiles[0, 0].GetComponent<Image>().sprite = null;
                AllTiles[0, 2].GetComponent<Image>().sprite = null;
                break;


            case 3:
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[2, FoodNum].GetComponent<Image>().sprite == null)
                    {
                        AllTiles[2, FoodNum].GetComponent<Image>().sprite = AllTiles[0, 3].GetComponent<Image>().sprite;
                        if (AllTiles[0, 3].GetComponent<Image>().sprite != null)
                            AllTiles[2, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }

                }
                for (int FoodNum = 0; FoodNum < 10; FoodNum++)
                {
                    if (AllTiles[1, FoodNum].GetComponent<Image>().sprite == null)
                    {
                        AllTiles[1, FoodNum].GetComponent<Image>().sprite = AllTiles[0, 1].GetComponent<Image>().sprite;
                        if (AllTiles[0, 1].GetComponent<Image>().sprite != null)
                            AllTiles[1, FoodNum].GetComponent<Image>().enabled = true;
                        break;
                    }
                }
                AllTiles[0, 1].GetComponent<Image>().sprite = null;
                AllTiles[0, 3].GetComponent<Image>().sprite = null;
                break;
        }
    }

    public void Randomset()
    {
        if (oshita)
        {
            temp = Random.Range(0, 4);
            text.text = Library.Instance.Categorys[temp].CategoryName;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            oshita = !oshita;
            if (oshita)
                Debug.Log("Start");
            else
            {
                Debug.Log("STOP");
                Debug.Log(temp);
                Debug.Log(Library.Instance.Categorys[temp].CategoryName);
                Debug.Log(Library.Instance.Categorys[temp].Foods[0].FoodName);
                state = State.choicefood;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log(state);
        if (state == State.gameover)
        {

        }
        else if (state == State.ready)
        {
            Randomset();
        }
        else if (state == State.choicefood)
        {
            if (AllTiles[0, 1].GetComponent<Image>().sprite == null && AllTiles[0, 0].GetComponent<Image>().sprite == null && AllTiles[0, 1].GetComponent<Image>().sprite == null && AllTiles[0, 2].GetComponent<Image>().sprite == null && AllTiles[0, 3].GetComponent<Image>().sprite == null)
            {
                if (AllTiles[1, 9].GetComponent<Image>().sprite == null)
                {
                    Generate();
                }
                else
                {
                    Debug.Log("プレイヤーの食材がいっぱいになったよ");
                    state = State.result;
                }
            }
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
        }else if (state == State.result)
        {
            Debug.Log("results");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResultFood.GetComponent<Image>().sprite = Library.Instance.Categorys[temp].Foods[0].FoodSprite;
                ResultFood.enabled = true;
            }
        }
    }
}
