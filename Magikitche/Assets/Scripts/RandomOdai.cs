using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomOdai : MonoBehaviour
{

    public Text text;
    private bool oshita;
    public int a;
    public int temp;

    // Start is called before the first frame update
    void Start()
    {
        oshita = false;
    }

    // Update is called once per frame
    void Update()
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
            }
        }
    }
}