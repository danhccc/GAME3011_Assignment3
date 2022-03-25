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
}
