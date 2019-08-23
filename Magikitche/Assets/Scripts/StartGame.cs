using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Text Press;
    private float level;

    // Start is called before the first frame update
    void Start()
    {
        MainGameManager.P1Score = 100;
        MainGameManager.P2Score = 100;
        MainGameManager.GameCount = 0;
        MainGameManager.IngredNum1=0;
        MainGameManager.IngredNum2=0;
        MainGameManager.GameProgress=0;
        MainGameManager.PrecedNum=0;
        MainGameManager.Player1Ingred = new int[10];
        MainGameManager.Player2Ingred = new int[10];
    }

    // Update is called once per frame
    void Update()
    {
        level = Mathf.Abs(Mathf.Sin(Time.time * 10));
        Press.color = new Color(0f, 0f, 0f, level);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(StartGameDelay(0.5f));
        }
    }

    private IEnumerator StartGameDelay(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("Main");
    }
}