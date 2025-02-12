using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Card_data data;

    public string card_name;
    public string description;
    //health is number
    public Sprite sprite;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI TopLeft;
    public TextMeshProUGUI TopRight;
    public TextMeshProUGUI BottomLeft;
    public TextMeshProUGUI BottomRight;
    public Image spriteImage;
    public string valueOnCard = "none";
    public int valueNotOnCard = 9; //this will not stay 9

    private int listpos = 0;
    private GameObject Dealer;
        

    // Start is called before the first frame update
    void Start()
    {
        Dealer = GameObject.FindWithTag("GameManager");
        card_name = data.card_name;
        description = data.description;
        valueOnCard = data.valueOnCard;
        valueNotOnCard = data.valueNotOnCard;
        sprite = data.sprite;
        nameText.text = card_name;
        descriptionText.text = description;
        TopLeft.text = valueOnCard;
        TopRight.text = valueOnCard;
        BottomLeft.text = valueOnCard;
        BottomRight.text = valueOnCard;
        spriteImage.sprite = sprite;

        //listpos = Dealer.GetComponent<GameManager>().player_hand.IndexOf(this.gameObject.GetComponent<Card>());
        //gameObject.transform.position = new Vector2(listpos * 100, + 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
