using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // SINGLETON
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    //SẮP XẾP THẺ LẬT
    public RectTransform cardField; 
    public GridLayoutGroup cardLayout;
    public void CreateTable(int w = 4, int h = 4)
    {
        //Lấy list thẻ
        List<Card> listCard = CardManager.Instance.CreateCardList (w, h);

        //Set số cột của cardLayout để sắp xếp thẻ
        cardLayout.constraintCount = w;
        //Set lại kích thước mỗi thẻ cho cân đối với kích thước màn hình
        cardLayout.cellSize = new Vector2((cardField.rect.width - 20*w)/w, (cardField.rect.height - 30 * h) / h);

        //Tổng số cặp thẻ
        numOfPair = w * h / 2;

        while (listCard.Count != 0)
        {
            int index = Random.Range(0, listCard.Count);
            listCard[index].transform.SetParent(cardField);
            listCard.Remove(listCard[index]);

        }
    }

    [SerializeField]
    Slider slider;
    [SerializeField]
    public TextMeshProUGUI timeText;

    float time = 0;
    float maxTime = 61;
    public int numOfPair;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }
    private void EndGame()
    {
        UIManager.Instance.end();
        this.gameObject.SetActive(false);

    }    
    public void StartGame()
    {
        this.gameObject.SetActive(true);
        CardManager.Instance.Reset();
        time = maxTime;
        CreateTable();
        
    }
    private void Update()
    {
        time -= Time.deltaTime;
        int min = (int)time / 60;
        int sec = (int)time - min*60;
        timeText.text = min.ToString("00") + ":" + sec.ToString("00");
        slider.value = time / maxTime;

        //Khi không còn cặp thẻ úp nào hoặc hết thời gian thì endgame
        if (numOfPair == 0)
            EndGame();
        if (time <= 0)
            EndGame();

              
    }
}
