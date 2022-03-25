using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    public bool GameStarted;
    public float timer;
    public int moveLeft;

    [Header("References")]
    [SerializeField] public TextMeshProUGUI timer_Text;
    [SerializeField] public TextMeshProUGUI move_Text;
    [SerializeField] public GameObject gameoverPanel;
    [SerializeField] public TextMeshProUGUI gameoverScore;

    // Singleton
    public static Board Instance { get; private set; }

    public Row[] rows;
    public Tile[,] Tiles{get; private set;}

    public int Width => Tiles.GetLength(/*Dimension:*/0);
    public int Height => Tiles.GetLength(/*Dimension:*/1);

    // List of first seleced item and second selected item
    private readonly List<Tile> _selection = new List<Tile>();

    private const float TweenDuration = 0.25f;

    [SerializeField] private AudioClip popSound;
    [SerializeField] private AudioSource audioSource;


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
        //if (!Input.GetKeyDown(KeyCode.A)) 
        //    return;

        //foreach (var connectedTile in Tiles[0, 0].GetConnectedTiles())
        //{
        //    connectedTile.icon.transform.DOScale(1.25f, TweenDuration).Play();
        //}

        if (GameStarted)
        {
            runTimer();
            if (timer <= 0 || moveLeft <= 0)
            {
                Gameover();
            }
            showMoves();
        }
    }
    
    public async void Select(Tile tile)
    {
        if (!_selection.Contains(tile))
        {
            if (_selection.Count > 0)   // If player already select one tile
            {
                if (Array.IndexOf(_selection[0].AllLinkedTiles, tile) != -1)    // Check if the second tile is tnear the first tile
                {
                    _selection.Add(tile);
                }
                else print("Invalid tile! Please choose a near by tile.");
            }
            else
            {
                _selection.Add(tile);
            }
        }

        if (_selection.Count < 2) 
            return;

        Debug.Log($"Selected tiles at {_selection[0].x}, {_selection[0].y}) and ({_selection[1].x}, {_selection[1].y})");

        await Swap(_selection[0], _selection[1]);

        // If we can pop after swapping, keep the pop; otherwise swap back
        if (CanPop())
        {
            Pop();
            moveLeft -= 1;
        }
            
        else
        {
            await Swap(_selection[0], _selection[1]);
            moveLeft -= 1;
        }

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

    // Define the minimum and maximum amount of tiles that needs to pop
    private bool CanPop()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (Tiles[x, y].GetConnectedTiles().Skip(1).Count() >= 2)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private async void Pop()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = Tiles[x, y];

                var connectedTiles = tile.GetConnectedTiles();

                // After skipping the first one, if more than 2 tiles connected, continue:
                if (connectedTiles.Skip(1).Count() < 2)
                    continue;

                var deflateSequence = DOTween.Sequence();

                foreach (var _connectedTile in connectedTiles)
                {
                    deflateSequence.Join(_connectedTile.icon.transform.DOScale(Vector3.zero, TweenDuration));
                }

                // Matching interactive
                audioSource.PlayOneShot(popSound);
                ScoreCalculator.Instance.Score += tile.Item.value * connectedTiles.Count;


                await deflateSequence.Play().AsyncWaitForCompletion();


                var inflateSequence = DOTween.Sequence();

                foreach (var _connectedTile in connectedTiles)
                {
                    _connectedTile.Item = ItemDataBase.Items[Random.Range(0, ItemDataBase.Items.Length)];

                    inflateSequence.Join(_connectedTile.icon.transform.DOScale(Vector3.one, TweenDuration));
                }

                await inflateSequence.Play().AsyncWaitForCompletion();

                x = y = 0;
            }
        }
    }

    public void runTimer()
    {
        timer -= Time.deltaTime;
        timer_Text.text = "Timer: " + timer.ToString("F0");
    }

    public void showMoves()
    {
        move_Text.text = "Move: " + moveLeft.ToString();
    }

    public void Gameover()
    {
        GameStarted = false;
        Time.timeScale = 0;

        gameoverPanel.SetActive(true);
        gameoverScore.text = ScoreCalculator.Instance._score.ToString();
    }
}
