using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{

    public int x;
    public int y;

    private Item _item;
    public Image icon;
    public Button button;

    public Item item
    {
        get => _item;
        set
        {
            if (_item == value)
                return;

            _item = value;
            icon.sprite = _item.sprite;
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
}
