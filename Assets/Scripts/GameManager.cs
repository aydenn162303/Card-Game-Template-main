using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public List<Card> deck = new List<Card>();
    public List<Card> player_hand = new List<Card>();
    public List<Card> ai_hand = new List<Card>();
    public List<Card> discard_pile = new List<Card>();

    public int targetHandSize = 21;
    public int playerSpecialCards = 0;
    public int aiSpecialCards = 0;
    //These will reset every game
    public int playerGamesWon = 0;
    public int aiGamesWon = 0;



    private void Awake()
    {
        if (gm != null && gm != this)
        {
            Destroy(gameObject);
        }
        else
        {
            gm = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Deal()
    {
        

    }

    void Shuffle()
    {

    }

    void AI_Turn()
    {

    }



    
}
