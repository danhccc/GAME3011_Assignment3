using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveLeftCounter : MonoBehaviour
{
    public static MoveLeftCounter Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;


    private int _moves;
    public int MoveLeft
    {
        get => _moves;

        set
        {
            if (_moves == value)
                return;

            _moves = value;

            scoreText.SetText($"Move: {_moves}");
        }

    }

    private void Awake()
    {
        Instance = this;
    }
}
