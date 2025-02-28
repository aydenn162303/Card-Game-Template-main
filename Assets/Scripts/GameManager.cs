using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;
using System;


//IMPORTANT: THERE MIGHT BE ERRORS WITH AI DRAWING ACES BETWEEN AI HIDDEN HAND TOTAL AND NORMAL HAND TOTAL, MAYBE NOT BUT MAKE SURE.

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public List<Card> deck = new List<Card>();
    public List<Card> special = new List<Card>();
    public List<Card> player_hand = new List<Card>();
    public List<Card> ai_hand = new List<Card>();
    public List<Card> discard_pile = new List<Card>();

    private float buttonPressDelay = 0.5f;

    private bool isAce11 = false;
    private bool isAce11AI = false;

    public Button Hit;
    public Button Stand;
    public Button DoubleDown;

    public TMPro.TextMeshProUGUI playerHandValue;
    public TMPro.TextMeshProUGUI aiHandValue;
    public TMPro.TextMeshProUGUI roundText;

    public Image plcrown1;
    public Image plcrown2;
    public Image plcrown3;

    public Image aicrown1;
    public Image aicrown2;
    public Image aicrown3;

    public Image coverCardAI;

    public int targetHandSize = 21;
    public int playerSpecialCards = 0;
    public int playerHandTotal = 0;
    public int AIHandTotal = 0;
    public int AIHandTotalHidden = 0; //both are added, this is just for hidden cards to store their values.
    public int AddedAIHandTotal = 0;
    public int aiSpecialCards = 0;
    //These will reset every game
    public int playerGamesWon = 0;
    public int aiGamesWon = 0;

    public int round = 0;
    public bool hiddenCard = false; //sets true once just to check if hidden bool should be changed in a card to make it appear offscreen until the ai's turn.
    public bool AICanDraw = false;
    

    private void Awake()
    {
        GameObject dealerObj = GameObject.Find("DealerOld");
        if (dealerObj != null)
        {
            gm = dealerObj.GetComponent<GameManager>();
            TransferVariablesFromExistingDealer(gm);
            Destroy(dealerObj);
            // Make this instance persistent across scenes
            DontDestroyOnLoad(gameObject);
            //print("Old Dealer deleted");
        }
        else
        {
            // If there isn't, set this instance as the singleton instance
            gm = this;
            // Make this instance persistent across scenes
            DontDestroyOnLoad(gameObject);
            //print("GameManager created");
        }

    }

    void TransferVariablesFromExistingDealer(GameManager existingDealer)
    {
        this.targetHandSize = existingDealer.targetHandSize;
        this.playerSpecialCards = existingDealer.playerSpecialCards;
        this.round = existingDealer.round;
        this.playerGamesWon = existingDealer.playerGamesWon;
        this.aiGamesWon = existingDealer.aiGamesWon;
    }

    // Start is called before the first frame update
    private void Start()
    {
        StartRound();
        AICanDraw = false;
    }

    // Update is called once per frame
    void Update()
    {
        buttonPressDelay -= Time.deltaTime;
        AddedAIHandTotal = AIHandTotal + AIHandTotalHidden;
    }

    void StartRound()
    {
        round++;

        if (playerGamesWon > 0) plcrown1.gameObject.SetActive(true); else plcrown1.gameObject.SetActive(false);
        if (playerGamesWon > 1) plcrown2.gameObject.SetActive(true); else plcrown2.gameObject.SetActive(false);
        if (playerGamesWon > 2) plcrown3.gameObject.SetActive(true); else plcrown3.gameObject.SetActive(false);
        if (aiGamesWon > 0) aicrown1.gameObject.SetActive(true); else aicrown1.gameObject.SetActive(false);
        if (aiGamesWon > 1) aicrown2.gameObject.SetActive(true); else aicrown2.gameObject.SetActive(false);
        if (aiGamesWon > 2) aicrown3.gameObject.SetActive(true); else aicrown3.gameObject.SetActive(false);

        coverCardAI.gameObject.SetActive(false);
        Hit.gameObject.SetActive(false);
        DoubleDown.gameObject.SetActive(false);
        Stand.gameObject.SetActive(false);
        playerHandValue.gameObject.SetActive(false);
        aiHandValue.gameObject.SetActive(false);
        roundText.gameObject.SetActive(true);
        roundText.text = "Round: " + round.ToString();

        StartCoroutine(StartGameCoroutine());
    }

    IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(2);
        //print("waiting");

        coverCardAI.gameObject.SetActive(true);
        coverCardAI.transform.SetAsLastSibling();
        Hit.gameObject.SetActive(true);
        Stand.gameObject.SetActive(true);
        DoubleDown.gameObject.SetActive(true);
        playerHandValue.gameObject.SetActive(true);
        aiHandValue.gameObject.SetActive(true);
        roundText.gameObject.SetActive(false);
        playerHandValue.gameObject.SetActive(true);
        aiHandValue.gameObject.SetActive(true);
        roundText.gameObject.SetActive(false);

        plcrown1.gameObject.SetActive(false);
        plcrown2.gameObject.SetActive(false);
        plcrown3.gameObject.SetActive(false);
        aicrown1.gameObject.SetActive(false);
        aicrown2.gameObject.SetActive(false);
        aicrown3.gameObject.SetActive(false);
        DealAI();
    }
    
    void DealAI()
    {
        DealAIBegin();
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

    void DealAIBegin()
    {
        isAce11AI = false;
        hiddenCard = true;
        DrawCardAI();
        hiddenCard = false;
        DrawCardAI();
    }

    public void DoubleDownPlayer()
    {
        DrawCardPlayer();
    }

    public void DrawCardAI()
    {
        print("DrawCardAI called");
        if (isAce11AI == true && AIHandTotal + AIHandTotalHidden > targetHandSize)
        {
            AIHandTotal -= 10;
            isAce11AI = false;
        }


        Card card = deck[Random.Range(0, deck.Count)];
        ai_hand.Add(card);
        deck.Remove(card);
        if (hiddenCard == true)
        {
            DealHiddenCardAI(card);
            hiddenCard = false;
        }
        else
        {
            InstantiateCardAI(card);
        }
        if (AICanDraw) { StartCoroutine(AITurn()); }
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

        if (isAce11 == true && playerHandTotal > targetHandSize)
        {
            playerHandTotal -= 10;
            isAce11 = false;
        }

        Random.InitState(System.DateTime.Now.Millisecond);
        Card card = deck[Random.Range(0, deck.Count)];
        player_hand.Add(card);
        deck.Remove(card);
        instantiateCard(card);

    }

    void instantiateCard(Card card)
    {
        
        if (player_hand.Count <= 6)
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

            }
            else if (cardCurrentVal.valueNotOnCard == 1 && playerHandTotal + 11 > targetHandSize)
            {
                playerHandTotal += 1;

            }
            else
            {

                playerHandTotal += cardCurrentVal.valueNotOnCard;
            }


            playerHandValue.text = "Hand: " + playerHandTotal.ToString();

        }
        else
        {
            print("Player Hand Full");
        }

        CheckIfBust(); //DOUBLE CHECK TO SEE IF ACE IS WORTH 1 AND NO BUST!!  


    }

    void InstantiateCardAI(Card card)
    {
        print("InstantiateCardAI called");
        GameObject cardObject = Instantiate(card.gameObject, GameObject.Find("Canvas").transform);
        int listpos = ai_hand.IndexOf(card);
        RectTransform canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        cardObject.transform.position = new Vector2(945 + ((listpos * (canvasRect.rect.width / 10)) * -1), canvasRect.rect.height - 350);
        Card_data cardCurrentVal = card.data;

        if (cardCurrentVal.valueNotOnCard == 1 && AIHandTotal + AIHandTotalHidden + 11 <= targetHandSize)
        {
            AIHandTotal += 11;
            isAce11AI = true;
            //print("AI    ace value 11");
        }
        else if (cardCurrentVal.valueNotOnCard == 1 && AIHandTotal + AIHandTotalHidden + 11 > targetHandSize)
        {
            AIHandTotal += 1;
            //print("AI   ace but too much for 11");
        }
        else
        {
            //print("AI   not ace");
            AIHandTotal += cardCurrentVal.valueNotOnCard;
        }

        aiHandValue.text = "Hand: " + AIHandTotal.ToString() + " + ?";

    }

    void DealHiddenCardAI(Card card) //allows drawing exclusively from here as an alternate gamemode (so that the player cannot see what is drawn as a challenge)
    {
        GameObject cardObject = Instantiate(card.gameObject, GameObject.Find("Canvas").transform);
        int listpos = ai_hand.IndexOf(card);
        RectTransform canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        cardObject.transform.position = new Vector2(945 + ((listpos * (canvasRect.rect.width / 10)) * -1), canvasRect.rect.height - 350);
        Card_data cardCurrentVal = card.data;

        if (cardCurrentVal.valueNotOnCard == 1 && AIHandTotal + AIHandTotalHidden + 11 <= targetHandSize)
        {
            AIHandTotalHidden += 11;
            isAce11AI = true;
            //print("HIDDEN ACE VALUE 11");
        }
        else if (cardCurrentVal.valueNotOnCard == 1 && AIHandTotal + AIHandTotalHidden + 11 > targetHandSize)
        {
            AIHandTotalHidden += 1;
            //print("HIDDEN ACE VALUE 1");
        }
        else
        {
            //print("HIDDEN NON ACE");
            AIHandTotalHidden += cardCurrentVal.valueNotOnCard;
        }
        
 
    }


    void DealFaceCard()
    {

    }

    void Shuffle()
    {
        
    }

    public void StandButton()
    {
        StartCoroutine(AITurn());
    }

    public IEnumerator AITurn()
    {
        print("AI Turn begin");
        Hit.gameObject.SetActive(false);
        Stand.gameObject.SetActive(false);
        DoubleDown.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);

        //What the Ai chooses to do is here, for now it will just draw until it hits 16 or more.
        AddedAIHandTotal = AIHandTotal + AIHandTotalHidden; //make sure this value is correct
        if (AIHandTotal + AIHandTotalHidden <= 16 || AddedAIHandTotal < playerHandTotal && playerHandTotal <= targetHandSize)
        {
            AICanDraw = true;
            DrawCardAI();
            print("AI Draw");
        }
        else
        {
            AICanDraw = false;
            StartCoroutine(RoundEnd());
            print("AI Stand");
        }

    }

    IEnumerator RoundEnd()
    {
        Stand.gameObject.SetActive(false);
        Hit.gameObject.SetActive(false);
        DoubleDown.gameObject.SetActive(false);
        playerHandValue.gameObject.SetActive(false);
        aiHandValue.gameObject.SetActive(false);
        roundText.gameObject.SetActive(true);

        CheckIfBust();

        print("Round End");
        //double check this works right
        if (playerHandTotal > AddedAIHandTotal && playerHandTotal <= targetHandSize || AddedAIHandTotal > targetHandSize)
        {
            roundText.text = "Player Wins!";
            playerGamesWon++;
        }
        else if (playerHandTotal < AddedAIHandTotal && AddedAIHandTotal <= targetHandSize || playerHandTotal > targetHandSize)
        {
            roundText.text = "AI Wins!";
            aiGamesWon++;
        }
        else if (playerHandTotal == AddedAIHandTotal)
        {
            roundText.text = "Draw!";
        }
        else
        {
            roundText.gameObject.SetActive(true);
            roundText.text = "Unknown Error!";
        }

       // foreach (Card card in player_hand)
        //{
            //print(card.data.value.ToString());
        //}

        yield return StartCoroutine(WaitForSeconds(2));
        playerHandTotal = 0;
        AIHandTotal = 0;
        AIHandTotalHidden = 0;
        ai_hand.Clear();
        player_hand.Clear();
        discard_pile.Clear();
        LoadNewScene("!Menu");
        LoadNewScene("InGame");
    }

    void CheckIfBust()
    {
        if (playerHandTotal > targetHandSize)
        {
            print("Player Bust");
            StartCoroutine(HandleBust("Player"));
        }

        if (AIHandTotal > targetHandSize)
        {
            print("AI Bust");
            StartCoroutine(HandleBust("AI"));
            //stuff
        }
    }

    IEnumerator HandleBust(string who_busted)
        {
            roundText.gameObject.SetActive(true);

            if (who_busted == "Player") { roundText.text = "Bust!"; } else { roundText.text = "AI Bust!"; }

            Hit.gameObject.SetActive(false);
            Stand.gameObject.SetActive(false);
            DoubleDown.gameObject.SetActive(false);
            yield return new WaitForSeconds(2);

            if (who_busted == "Player") { aiGamesWon++; } else { playerGamesWon++; }

            playerHandTotal = 0;
            AIHandTotal = 0;
            AIHandTotalHidden = 0;
            ai_hand.Clear();
            player_hand.Clear();
            discard_pile.Clear();
            LoadNewScene("!Menu");
            LoadNewScene("InGame");
        }

    public void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        this.gameObject.name = "DealerOld";
    }
    public IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
