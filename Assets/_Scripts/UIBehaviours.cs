using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIBehaviours : MonoBehaviour
{
    [SerializeField] public GameObject ToggleMiniGame;
    [SerializeField] public float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        ToggleMiniGame.SetActive(true);
        Board.Instance.GameStarted = true;
    }

    public void resetGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnEasyModeButtonPressed()
    {
        ToggleMiniGame.SetActive(true);
        Board.Instance.GameStarted = true;
        Board.Instance._difficulty = Difficulty.EASY;
        Board.Instance.difficultyOffest = (int)Difficulty.EASY;
    }
    
    public void OnNormalModeButtonPressed()
    {
        ToggleMiniGame.SetActive(true);
        Board.Instance.GameStarted = true;
        Board.Instance._difficulty = Difficulty.NORMAL;
        Board.Instance.difficultyOffest = (int)Difficulty.NORMAL;
    }
    
    public void OnHardModeButtonPressed()
    {
        ToggleMiniGame.SetActive(true);
        Board.Instance.GameStarted = true;
        Board.Instance._difficulty = Difficulty.HARD;
        Board.Instance.difficultyOffest = (int)Difficulty.NORMAL;
    }
}
