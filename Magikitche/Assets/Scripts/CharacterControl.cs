using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{
    public Image P1Chara;
    public Image P2Chara;
    float level;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        level = Mathf.Abs(Mathf.Sin(Time.time*10));
        P1Chara.transform.eulerAngles = new Vector3(0, 180, -level);
        P2Chara.transform.eulerAngles = new Vector3(0, 0, level);
    }
}
