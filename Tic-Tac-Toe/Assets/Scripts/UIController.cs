using TMPro;
using UnityEngine;

/*
 * Manages the UI flow based on events from the GameManager.
 */
public class UIController : MonoBehaviour
{

    // UI Elements
    [SerializeField] private GameObject StartPanel;
    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private TextMeshProUGUI TurnText;

    // Constant strings used to display specific messages
    private const string _titleString = "Tic-Tac-Toe";
    private const string _xWinString = "X Wins!..Play again?";
    private const string _oWinString = "O Wins!..Play again?";
    private const string _drawString = "Draw!..Play again?";
    private const string _xTurnString = "X's Turn";
    private const string _oTurnString = "O's Turn";
    private const string _npcTurnString = "NPC's Turn (O)";

    private void OnEnable()
    {
        GameManager.StartGame += StartGame;
        GameManager.EndGame += EndGame;
        GameManager.NextTurn += NextTurn;
    }
    private void OnDisable()
    {
        GameManager.StartGame -= StartGame;
        GameManager.EndGame -= EndGame;
        GameManager.NextTurn -= NextTurn;
    }

    private void Start()
    {
        TitleText.SetText(_titleString);
    }

    // Sets the next turn text
    private void NextTurn()
    {
        if (GameManager.GameState == GameManager.State.Player1Turn)
        {
            TurnText.SetText(_xTurnString);
        }
        else
        {
            if (GameManager.GameMode == GameManager.Mode.TwoPlayer)
            {
                TurnText.SetText(_oTurnString);
            }
            else
            {
                TurnText.SetText(_npcTurnString);
            }
        }
        TurnText.gameObject.SetActive(true);
    }

    // Based on the winner shows the correct message and re-enables the start screen
    private void EndGame(GameManager.Winner winner)
    {
        TurnText.gameObject.SetActive(false);

        switch (winner)
        {
            case GameManager.Winner.X:
                TitleText.SetText(_xWinString);
                break;
            case GameManager.Winner.O:
                TitleText.SetText(_oWinString);
                break;
            case GameManager.Winner.Draw:
                TitleText.SetText(_drawString);
                break;
        }

        StartPanel.SetActive(true);
    }

    // When the game starts the start panel is turned off and the next turn text is shown
    private void StartGame()
    {
        StartPanel.SetActive(false);
        NextTurn();
    }

}
