﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsManager : MonoBehaviour
{
    public Image TipsPanel;
    public Text TipsText;
    public GameObject MainGameManager;

    // Start is called before the first frame update
    void Start()
    {
        TipsPanel.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClicAct()
    {
        if (MainGameManager.GetComponent<MainGameManager>().state == global::MainGameManager.State.choicefood)
        {
            Debug.Log("おしたよ");
            TipsPanel.enabled = !TipsPanel.enabled;
            TipsText.enabled = !TipsText.enabled;
            Debug.Log(MainGameManager.GetComponent<MainGameManager>().OdaiText.text);
            if(MainGameManager.GetComponent<MainGameManager>().OdaiText.text=="おかず")
                TipsText.text = "おかずだよ";
            if (MainGameManager.GetComponent<MainGameManager>().OdaiText.text == "麺類")
                TipsText.text = "めんるいだよ";
            if (MainGameManager.GetComponent<MainGameManager>().OdaiText.text == "汁物")
                TipsText.text = "しるものだよ";
            if (MainGameManager.GetComponent<MainGameManager>().OdaiText.text == "ごはんもの")
                TipsText.text = "ごはんものだよ";
        }
    }
}
