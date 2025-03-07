using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chip : MonoBehaviour
{
    public Chip_data data;
    
    public int value;
    public Sprite sprite;
    public Image spriteImage;
    private GameObject Dealer;
    
    // Start is called before the first frame update
    void Start()
    {
        Dealer = GameObject.FindWithTag("GameManager");
        if (data != null)
        {
            LoadChipData(data);
        }
    }
    
    void LoadChipData(Chip_data data)
    {
        value = data.chip_value;
        sprite = data.sprite;
        spriteImage.sprite = sprite;
    }
}
