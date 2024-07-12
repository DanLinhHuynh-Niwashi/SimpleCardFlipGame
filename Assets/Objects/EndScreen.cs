using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    TextMeshProUGUI victorText;
    [SerializeField]
    TextMeshProUGUI timeText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setResult()
    {
        if (GameManager.Instance.numOfPair ==0)
        {
            victorText.text = "VICTORY";
            timeText.text = "Remaining time: " + GameManager.Instance.timeText.text;
        }
        else
        {
            victorText.text = "FAILED";
            timeText.text = "";
        }
    }
}
