using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public List<Card> deck = new List<Card>();
    public List<Card> player_hand = new List<Card>();
    public List<Card> ai_hand = new List<Card>();
    public List<Card> discard_pile = new List<Card>();

    private float buttonPressDelay = 0.5f;

    private bool isAce11 = false;



    public Button Hit;
    public Button Stand;
    public Button DoubleDown;

    public TMPro.TextMeshProUGUI playerHandValue;
    public TMPro.TextMeshProUGUI aiHandValue;
    public TMPro.TextMeshProUGUI roundText;

    public int targetHandSize = 21;
    public int playerSpecialCards = 0;
    public int playerHandTotal;
    public int AIHandTotal;
    public int aiSpecialCards = 0;
    //These will reset every game
    public int playerGamesWon = 0;
    public int aiGamesWon = 0;

    public int round = 0;

    private void Awake()
    {
        // Check if there is already an instance of GameManager
        if (gm != null && gm != this)
        {
            // If there is, destroy this instance
            Destroy(gameObject);
        }
        else
        {
            // If there isn't, set this instance as the singleton instance
            gm = this;
            // Make this instance persistent across scenes
            DontDestroyOnLoad(gameObject);
            print("GameManager created");
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        buttonPressDelay -= Time.deltaTime;
    }

    void StartGame()
    {
        //Shuffle();
        DealAI();
    }
    
    void DealAI()
    {
        DealPlayerBegin();
    }

    void DealPlayerBegin()
    {
        isAce11 = false;
        buttonPressDelay = 0f;
        DrawCardPlayer();
        buttonPressDelay = 0f;
        DrawCardPlayer();
        DoubleDown.gameObject.SetActive(true);
    }

    public void DoubleDownPlayer()
    {
        DrawCardPlayer();
    }

    public void DrawCardPlayer()
    {
        DoubleDown.gameObject.SetActive(false);

        if (buttonPressDelay > 0)
        {
            return;
        }
        else
        {
            buttonPressDelay = 0.5f;
        }

        if (isAce11 == true && playerHandTotal < targetHandSize)
        {
            playerHandTotal -= 10;
            isAce11 = false;
        }


        Card card = deck[Random.Range(0, 51)];
        player_hand.Add(card);
        deck.Remove(card);
        instantiateCard(card);

    }

    void instantiateCard(Card card)
    {
        GameObject cardObject = Instantiate(card.gameObject, GameObject.Find("Canvas").transform);
        int listpos = player_hand.IndexOf(card);
        RectTransform canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        cardObject.transform.position = new Vector2(listpos * (canvasRect.rect.width / 10), -150);
        Card_data cardCurrentVal = card.data;

        if (cardCurrentVal.valueNotOnCard == 1 && playerHandTotal + 11 <= targetHandSize)
        {
            playerHandTotal += 11;
            isAce11 = true;
            print("ace value 11");
        }
        else if (cardCurrentVal.valueNotOnCard == 1 && playerHandTotal + 11 > targetHandSize)
        {
            playerHandTotal += 1;
            print("ace but too much for 11");
        }
        else
        {
            print("not ace");
            playerHandTotal += cardCurrentVal.valueNotOnCard;
        }


        playerHandValue.text = "Hand: " + playerHandTotal.ToString();
    }

    void DrawCardAI()
    {

    }

    void DealFaceCard()
    {

    }

    void Shuffle()
    {
        
    }

    public void AI_Turn()
    {
        Hit.gameObject.SetActive(false);
        Stand.gameObject.SetActive(false);
        DoubleDown.gameObject.SetActive(false);
    }

    void CheckIfBust()
    {
        if (playerHandTotal > targetHandSize)
        {
            print("Player Bust");
            aiGamesWon++;
            playerHandTotal = 0;
            AIHandTotal = 0;
            player_hand.Clear();
            ai_hand.Clear();
            discard_pile.Clear();
            StartGame();
        }
        else if (AIHandTotal > targetHandSize)
        {
            print("AI Bust");
            playerGamesWon++;
            playerHandTotal = 0;
            AIHandTotal = 0;
            player_hand.Clear();
            ai_hand.Clear();
            discard_pile.Clear();
            StartGame();
        }
    }

    public void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
