using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    public Text ResultText;

    // Start is called before the first frame update
    void Start()
    {
        if (MainGameManager2.P1Score > MainGameManager2.P2Score)
        {
            ResultText.text = "P1の勝ち！";
        }
        else if (MainGameManager2.P1Score < MainGameManager2.P2Score)
        {
            ResultText.text = "P2の勝ち！";
        }
        else
        {
            ResultText.text = "引き分け！！";
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 7"))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
