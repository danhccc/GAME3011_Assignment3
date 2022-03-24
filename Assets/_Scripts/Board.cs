using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;

public class Board : MonoBehaviour
{
    // Singleton
    public static Board Instance { get; private set; }

    public Row[] rows;
    public Tile[,] Tiles{get; private set;}

    public int Width => Tiles.GetLength(/*Dimension:*/0);
    public int Height => Tiles.GetLength(/*Dimension:*/1);

    // List of first seleced item and second selected item
    private readonly List<Tile> _selection = new List<Tile>();

    private const float TweenDuration = 0.25f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Get the maximum number of rows in the board
        Tiles = new Tile[rows.Max(Row => Row.tiles.Length), rows.Length]; // Max width and height

        // Base on rows in scene, setup the board
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = rows[y].tiles[x];

                tile.x = x; 
                tile.y = y;

                tile.Item = ItemDataBase.Items[Random.Range(0, ItemDataBase.Items.Length)];

                Tiles[x, y] = tile;
            }
        }


    }


    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.A)) 
            return;

        foreach (var connectedTile in Tiles[0, 0].GetConnectedTiles())
        {
            connectedTile.icon.transform.DOScale(1.25f, TweenDuration).Play();
            print("?");
        }
            
        
    }

    public async void Select(Tile tile)
    {
        if (!_selection.Contains(tile)) 
            _selection.Add(tile);

        if (_selection.Count < 2) 
            return;

        Debug.Log($"Selected tiles at {_selection[0].x}, {_selection[0].y}) and ({_selection[1].x}, {_selection[1].y})");

        await Swap(_selection[0], _selection[1]);

        _selection.Clear();
    }

    public async Task Swap(Tile tile1, Tile tile2)
    {
        var icon_1 = tile1.icon;
        var icon_2 = tile2.icon;

        var iconTransform_1 = icon_1.transform;
        var iconTransform_2 = icon_2.transform;

        var sequence = DOTween.Sequence();

        // Play the swaping animation(icons), no value changed at this point
        sequence.Join(iconTransform_1.DOMove(iconTransform_2.position, TweenDuration))
                .Join(iconTransform_2.DOMove(iconTransform_1.position, TweenDuration));

        // Wait for sequence to finished before continue (prevent swaping more times to break the game)
        await sequence.Play().AsyncWaitForCompletion();

        // After animation finished, swap their parent tile
        iconTransform_1.SetParent(tile2.transform);
        iconTransform_2.SetParent(tile1.transform);

        // Switch icons
        tile1.icon = icon_2;
        tile2.icon = icon_1;

        // Change items
        var tile1Item = tile1.Item;
        tile1.Item = tile2.Item;
        tile2.Item = tile1Item;
    }

    private void CanPop()
    {
        
    }

    private void Pop()
    {

    }

}
