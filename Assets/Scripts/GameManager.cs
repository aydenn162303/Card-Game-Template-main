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

    private float buttonPressDelay = 0.3f;



    public Button Hit;
    public Button Stand;
    public Button DoubleDown;

    public TMPro.TextMeshProUGUI playerHandValue;

    public int targetHandSize = 21;
    public int playerSpecialCards = 0;
    public int aiSpecialCards = 0;
    //These will reset every game
    public int playerGamesWon = 0;
    public int aiGamesWon = 0;

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
    void Start()
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
        Shuffle();
        DealAI();
    }
    
    void DealAI()
    {
        DealPlayer();
    }

    void DealPlayer()
    {
        DrawCardPlayer();
        DrawCardPlayer();
    }

    public void DrawCardPlayer()
    {
        if (buttonPressDelay > 0)
        {
            return;
        }
        else
        {
            buttonPressDelay = 0.3f;
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
        cardObject.transform.position = new Vector2(listpos * 100, 5);
//add to total
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

    void AI_Turn()
    {

    }

    public void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
