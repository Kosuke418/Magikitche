using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour
{
    private Tile[,] AllTiles = new Tile[10,10];
    private List<Tile> EmptyTiles = new List<Tile>();
    private List<Tile[]> columns = new List<Tile[]>();


    public enum status{
        first,
        second,
        third,
        ready,
        choicefood,
        gameover,
        hint
    }

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Generate();
        }
    }
}
