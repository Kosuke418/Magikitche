using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Ingred
{
    public string IngredName; 
    public Sprite IngredSprite;
}

[System.Serializable]
public class Food
{
    public string FoodName;
    public Sprite FoodSprite;
    public int FoodScore;
    public Answer[] Answers;
}

[System.Serializable]
public class Answer
{
    public int AnswerNumber;
    public string AnswerName;
}

public class Library : MonoBehaviour
{
    public static Library Instance;

    public Ingred[] Ingreds;
    public Food[] Foods;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
