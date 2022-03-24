using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour
{
    // Singleton
    public static Board Instance { get; private set; }

    public Row[] rows;
    public Tile[,] Tiles{get; private set;}

    public int Width => Tiles.GetLength(/*Dimension:*/0);
    public int Height => Tiles.GetLength(/*Dimension:*/1);


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
                Tiles[x, y] = rows[y].tiles[x];
            }
        }


    }




}
