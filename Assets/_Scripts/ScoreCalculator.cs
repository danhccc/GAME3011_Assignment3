using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCalculator : MonoBehaviour
{
    public static ScoreCalculator Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;


    public int _score;
    public int Score 
    {
        get => _score;

        set
        {
            if (_score == value)
                return;

            _score = value;

            scoreText.SetText($"Score: {_score}");
        }
    }



    private void Awake()
    {
        Instance = this;
    }


}
