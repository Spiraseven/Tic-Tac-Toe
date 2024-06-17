using System;
using System.Collections;
using UnityEngine;

/*
 * Manages the flow of the game including turns and checking win conditions.
 */
public class GameManager : MonoBehaviour
{
    // What kind of game mode we are playing
    public enum Mode
    {
        SinglePlayer = 0,
        TwoPlayer = 1
    }

    // The possible states of the game
    public enum State
    {
        Standby = 0,
        Player1Turn = 1,
        Player2Turn = 2,
    }

    // An enum to represent the winner of the game
    public enum Winner
    {
        X = 0,
        O = 1,
        Draw = 2
    }

    // Global game states accessible by other classes
    public static State GameState = State.Standby;
    public static Mode GameMode = Mode.SinglePlayer;

    // Events other classes can react to in order to show information or visual effects
    public static event Action StartGame;
    public static event Action NextTurn;
    public static event Action<Winner> EndGame;

    // Class that controls the state of the game board and contains the information about each cell
    private BoardController _boardController;

    private bool _player1Starts = true;

    // Scriptable object used to configure the game and board (could be used to make a 4x4 instead of 3x3 game, for example)
    [SerializeField] private BoardData _BoardData;

    private void OnEnable()
    {
        InputManager.CellSelected += CellSelected;
    }

    private void OnDisable()
    {
        InputManager.CellSelected -= CellSelected;
    }

    // Initializes the game board
    void Start()
    {
        Camera.main.transform.position = _BoardData.CameraPosition;
        GameState = State.Standby;
        _player1Starts = true;
        _boardController = new BoardController(_BoardData);
        _boardController.SetupBoard();
    }

    // Initializes single player mode
    public void StartSinglePlayer()
    {
        GameMode = Mode.SinglePlayer;
        SetStartingState();
        _boardController?.ResetGameBoard();
    }

    // Initializes two player mode
    public void StartTwoPlayer()
    {
        GameMode = Mode.TwoPlayer;
        SetStartingState();
        _boardController?.ResetGameBoard();
    }

    // Sets initial state based on who should go first this round and starts the game
    private void SetStartingState()
    {
        if (_player1Starts)
        {
            GameState = State.Player1Turn;
        }
        else
        {
            GameState = State.Player2Turn;
        }

        if (GameState == State.Player2Turn && GameMode == Mode.SinglePlayer)
        {
            StartCoroutine(NPCTurn());
        }

        StartGame.Invoke();
    }

    // When a cell is selected by the player we try placing a new piece
    private void CellSelected(GameObject boardSquare)
    {
        if (GameState != State.Standby)
        {
            Cell cell = _boardController.GetCell(boardSquare);
            bool placed = _boardController.TryPlacePiece(cell, boardSquare.transform.position, GameState == State.Player1Turn);
            if (placed)
            {
                bool gameover = CheckWinCondition(cell);
                if (!gameover)
                {
                    UpdateNextTurnState();
                }
            }
        }
    }

    // Checks if game is won, is a draw, or should continue
    private bool CheckWinCondition(Cell cell)
    {
        bool won = _boardController.CheckWinCondition(cell);
        if (won)
        {
            if (GameState == State.Player1Turn)
            {
                GameOver(Winner.X);
                return true;
            }
            else
            {
                GameOver(Winner.O);
                return true;
            }
        }
        else if (_boardController.GetAvailableCellsCount() == 0)
        {
            GameOver(Winner.Draw);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Updates to the next turn and if it is an NPC turn starts the NPC coroutine
    private void UpdateNextTurnState()
    {
        if (GameState == State.Player1Turn)
        {
            GameState = State.Player2Turn;
        }
        else
        {
            GameState = State.Player1Turn;
        }

        NextTurn.Invoke();
        if (GameState == State.Player2Turn && GameMode == Mode.SinglePlayer)
        {
            StartCoroutine(NPCTurn());
        }
    }

    // Invokes any end game events and cleanup
    private void GameOver(Winner winner)
    {
        EndGame.Invoke(winner);
        GameState = State.Standby;

        // Changes who starts first next round
        _player1Starts = !_player1Starts;
    }

    // Runs NPC's turn by picking a random cell to place a piece on
    private IEnumerator NPCTurn()
    {
        // Artificial delay for user experience
        yield return new WaitForSeconds(0.75f);

        // Gets a random cell and places the piece
        Cell cell = _boardController.GetRandomAvailableCell();
        _boardController.TryPlacePiece(cell, cell.BoardSquare.transform.position, false);
        bool gameover = CheckWinCondition(cell);
        if (!gameover)
        {
            yield return new WaitForSeconds(0.75f);
            UpdateNextTurnState();
        }
    }
}
