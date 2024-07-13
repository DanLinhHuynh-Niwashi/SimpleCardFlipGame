using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class CardManager : MonoBehaviour
{
    // SINGLETON
    public static CardManager Instance { get; private set; }
    public Card cardPrefab;
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

    //POOLING
    List<Card> cardPool;
    void Start()
    {
        cardPool = new List<Card>();
        Card tmp;
        for (int i = 0; i < 40; i++)
        {
            tmp = Instantiate(cardPrefab);
            tmp.gameObject.SetActive(false);
            cardPool.Add(tmp);
        }
    }
    public Card GetPooledObject()
    {
        for (int i = 0; i < 40; i++)
        {
            if (!cardPool[i].gameObject.activeSelf)
            {
                return cardPool[i];
            }
        }
        return null;
    }

    //Reset toàn bộ pool về inactive, và ẩn pattern của thẻ đi
    public void Reset()
    {
        foreach (Card card in cardPool)
        {
            card.gameObject.SetActive(false);
            card.matchImg.enabled = true;
            card.transform.SetParent(null);
        }
        upCard.Clear();
    }


    // Update is called once per frame
    void Update()
    {
        countDownOpeningCard();
    }

    //Spawn thẻ
    public Sprite[] bgImage; //Background của thẻ, gồm 2 phần tử là mode đóng và lật
    public List<Sprite> matchImages; //pattern của thẻ, gồm 20 pattern có sẵn
    public List<Card> CreateCardList(int w, int h)
    {   //Tạo ra list gôm w*h/2 cặp thẻ, VD bàn kích thước 4 x 4 => 8 cặp thẻ

        //Random để chọn ra 8 pattern khác nhau từ list pattern
        List<int> randomList = new List<int>();

        while (randomList.Count < w * h / 2)
        {
            int a = Random.Range(0, 20);
            if (!randomList.Contains(a))
            {
                randomList.Add(a);
            }
        }

        List<Card> list = new List<Card>();
        int id = 0; //Chạy id riêng biệt cho các thẻ
        for (int i = 0; i < w * h / 2; i++) //tạo w*h/2 cặp thẻ
        {
            for (int k = 0; k < 2; k++) //Mỗi lần lặp tạo 1 cặp thẻ
            {
                Card card = GetPooledObject();
                card.gameObject.SetActive(true);
                card.id = id++;
                card.matchID = i; //Cặp thẻ giống nhau có cùng matchID
                card.bgImage = bgImage;
                card.bgImg.sprite = bgImage[1]; //Ban đầu thẻ lật
                card.matchImg.sprite = matchImages[randomList[i]]; //gán một pattern trong list đã chọn cho cặp thẻ
                card.isEnabled = true;
                card.isFlipped = true;
                list.Add(card);
                upCard.Add(card);
            }
        }
        
        countDown = 1;
        return list;
    }

    


    //QUẢN LÝ THẺ LẬT
    List<Card> upCard = new List<Card>(); 
    float countDown = 0;

    void flipAllUpCard()
    {
        foreach (Card card in upCard)
        {
            Debug.Log(card.isFlipped);
            card.flip();
        }
        upCard.Clear();
    }    
    //ĐẾM NGƯỢC VÀ LẬT LẠI THẺ KHI THẺ BỊ SAI
    void countDownOpeningCard()
    {
        if (countDown > 0)
        {
            countDown -= Time.deltaTime;
            Debug.Log(countDown);
            if (countDown <= 0)
            {
                countDown = 0;
                if (upCard.Count >= 2)
                {
                    flipAllUpCard();
                }
            }
        }
    }

    //LẬT THẺ VÀ KIỂM TRA MATCH
    public void flip(Card card)
    {
        if (countDown != 0) //Nếu đang đóng thẻ lật sai thì không được tương tác
        {
            return;
        }
        if (card.isFlipped) //NẾU THẺ ĐÃ ĐƯỢC LẬT: Xóa nó khỏi pair và úp thẻ lại
        {
            upCard.Remove(card);
            card.flip();
        }
        else //NẾU CHƯA: Thêm vào pair
        {
            upCard.Add(card);
            card.flip();
        }

        if (upCard.Count == 2) //NẾU ĐANG CÓ 2 THẺ LẬT TRÊN BÀN: So sánh có match hay không, nếu không thì countdown 0.5s rồi đóng lại.
        {
            if (upCard[0].matchID == upCard[1].matchID)
            {
                upCard[0].isEnabled = false;
                upCard[1].isEnabled = false;
                Debug.Log("Match!");
                upCard.Clear();
                GameManager.Instance.numOfPair--; //Giảm số cặp thẻ còn lại của game đang chạy
            }
            else
            {
                countDown = 0.5f;
                Debug.Log("No Match!");
            }
        }
    }
}
