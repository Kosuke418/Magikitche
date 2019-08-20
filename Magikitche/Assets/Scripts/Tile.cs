using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{

    public int IndCols;
    public int IndRows;

    public int Number
    {
        get
        {
            return number;
        }
        set
        {
            number = value;
            if (number == 0)
            {
                SetEmpty();
            }
            else
            {
                ApplyStyle(number);
                SetVisible();
            }
        }
    }

    private int number;

    private Text TileText;
    private Image TileImage;


    private void Awake()
    {
        TileImage = this.GetComponent<Image>();
    }

    void ApplyStyleFromHolder(int index)
    {
        TileImage = Library.Instance.Ingreds[index].IngredImage;
    }

    void ApplyStyle(int num)
    {
        switch (num)
        {
            case 1:
                ApplyStyleFromHolder(1);
                break;
            default:
                Debug.LogError("アプリスタイルのnumberを確認して");
                break;
        }
    }

    private void SetVisible()
    {
        TileImage.enabled = true; 
    }

    private void SetEmpty()
    {
        TileImage.enabled = false;
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
