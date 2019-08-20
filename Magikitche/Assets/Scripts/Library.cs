﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Ingred
{
    public string IngredName; 
    public Image IngredImage;
}

[System.Serializable]
public class Food
{
    public string FoodName;
    public Image FoodImage;
}

[System.Serializable]
public class Category
{
    public string CategoryName;
    public Food[] Foods;
}

public class Library : MonoBehaviour
{
    public static Library Instance;

    public Ingred[] Ingreds;
    public Category[] Categorys;

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
