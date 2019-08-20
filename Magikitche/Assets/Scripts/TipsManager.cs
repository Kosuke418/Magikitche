using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsManager : MonoBehaviour
{
    public Image image;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClicAct()
    {
        Debug.Log("おしたよ");
        image.enabled = !image.enabled;
        text.enabled = !text.enabled;
    }
}
