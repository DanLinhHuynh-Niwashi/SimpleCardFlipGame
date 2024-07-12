using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    EndScreen endGame;

    public static UIManager Instance { get; private set; }

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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void end()
    {
        endGame.gameObject.SetActive(true);
        endGame.setResult();

    }    
    public void restartClick()
    {
        endGame.gameObject.SetActive(false);
        GameManager.Instance.StartGame();
    }    
}
