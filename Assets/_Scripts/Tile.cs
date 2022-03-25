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

    public Item Item
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

    /*   Ask the Neighbour tiles to return all the same type of tiles   */
    // Should go from small to big
    public Tile Left => x > 0 ? Board.Instance.Tiles[x - 1, y] : null;
    public Tile Top => y > 0 ? Board.Instance.Tiles[x, y - 1] : null;
    public Tile Right => x < Board.Instance.Width - 1 ? Board.Instance.Tiles[x + 1, y] : null;
    public Tile bottom => y < Board.Instance.Height - 1 ? Board.Instance.Tiles[x, y + 1] : null;

    // This will be call MANY times
    public Tile[] AllLinkedTiles => new[]
    {
        Left, Top, Right, bottom,
    };

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(() => Board.Instance.Select(this));
    }

    public List<Tile> GetConnectedTiles(List<Tile> exclude = null)
    {
        // exclude the tiles that already been check to prevent problems
        var result = new List<Tile> { this, };

        if (exclude == null)
            exclude = new List<Tile> { this, };
        else 
            exclude.Add(this);

        foreach(var linkedTile in AllLinkedTiles)
        {
            if (linkedTile == null || exclude.Contains(linkedTile) || linkedTile.Item != Item) 
                continue;

            result.AddRange(linkedTile.GetConnectedTiles(exclude));
        }
        
        return result;
    }

}
