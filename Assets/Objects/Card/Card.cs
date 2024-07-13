using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    // Start is called before the first frame update
    public int id { get; set; }
    public int matchID { get; set; }
    [SerializeField]
    public Image matchImg;
    public Image bgImg;
    public bool isEnabled { get;  set; }
    public Sprite[] bgImage { get; set; }

    public bool isFlipped = true;
    void Start()
    {
        bgImg = this.GetComponent<Image>();
        matchImg.preserveAspect = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Flip (visually)
    public void flip()
    {
        if (isFlipped)
        {
            this.matchImg.enabled = false;
            if (bgImg != null)
            {
                bgImg.sprite = bgImage[0];
            }
        }
        else
        {
            this.matchImg.enabled = true;
            if (bgImg != null)
            {
                bgImg.sprite = bgImage[1];
            }
        }
        isFlipped = !isFlipped;
    }    
    // On Click
    public void onClick()
    {
        if (isEnabled)
        {
            CardManager.Instance.flip(this);
        }
    }    
}
