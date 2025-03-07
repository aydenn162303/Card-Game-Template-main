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
    public GameObject canvas;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI TopLeft;
    public TextMeshProUGUI TopRight;
    public TextMeshProUGUI BottomLeft;
    public TextMeshProUGUI BottomRight;
    public Image spriteImage;
    public string valueOnCard = "none";
    public int valueNotOnCard = 9; //this will not stay 9
    private GameObject Dealer;
    public bool hidden = false; // add stuff to hide if it is true
    public float yValue = 0;
    public bool waituntilnothidden = false;
        

    // Start is called before the first frame update
    void Start()
    {
        Dealer = GameObject.FindWithTag("GameManager");
        if (data != null)
        {
            LoadCardData(data);
        }
        canvas = GameObject.FindWithTag("Canvas");

    }

        //listpos = Dealer.GetComponent<GameManager>().player_hand.IndexOf(this.gameObject.GetComponent<Card>());
        //gameObject.transform.position = new Vector2(listpos * 100, + 5);
   
    void LoadCardData(Card_data data)
    {
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
    }


    // Update is called once per frame
    private bool isMoving = false;
    
    void Update()
    {
        if (this.gameObject.transform.position.y < 8 && !isMoving)
        {
            StartCoroutine(MoveCard());
        }

        if (hidden)
        {
            this.transform.SetParent(null);
        }

        if (Dealer.GetComponent<GameManager>().AICanDraw == true)
        {
            hidden = false;
        }

        if (!hidden)
        {
            this.transform.SetParent(canvas.transform);  
        }

    }
    
    IEnumerator MoveCard()
    {
        isMoving = true;

        while (this.gameObject.transform.position.y < 8)
        {
            float x = this.gameObject.transform.position.x;
            float y = this.gameObject.transform.position.y;
            this.gameObject.transform.position = new Vector2(x, y + 3f);
            yield return new WaitForSeconds(0.01f);
        }
        isMoving = false;
    }


    IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }


}
