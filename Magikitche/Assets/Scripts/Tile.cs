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
            //Debug.Log("number:" + number);
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
        TileImage.sprite = Library.Instance.Ingreds[index].IngredSprite;
    }

    void ApplyStyle(int num)
    {
        switch (num)
        {
            case 1:
                ApplyStyleFromHolder(1);
                break;
            case 2:
                ApplyStyleFromHolder(2);
                break;
            case 3:
                ApplyStyleFromHolder(3);
                break;
            case 4:
                ApplyStyleFromHolder(4);
                break;
            case 5:
                ApplyStyleFromHolder(5);
                break;
            case 6:
                ApplyStyleFromHolder(6);
                break;
            case 7:
                ApplyStyleFromHolder(7);
                break;
            case 8:
                ApplyStyleFromHolder(8);
                break;
            case 9:
                ApplyStyleFromHolder(9);
                break;
            case 10:
                ApplyStyleFromHolder(10);
                break;
            case 11:
                ApplyStyleFromHolder(11);
                break;
            case 12:
                ApplyStyleFromHolder(12);
                break;
            case 13:
                ApplyStyleFromHolder(13);
                break;
            case 14:
                ApplyStyleFromHolder(14);
                break;
            case 15:
                ApplyStyleFromHolder(15);
                break;
            case 16:
                ApplyStyleFromHolder(16);
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
