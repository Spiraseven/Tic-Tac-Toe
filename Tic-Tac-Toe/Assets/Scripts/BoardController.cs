using System.Collections.Generic;
using UnityEngine;

/*
 * Holds the current state of the board and functions to manipulate the game board.
 */
public class BoardController
{
    // Board square and piece parents
    private GameObject _boardContainer;
    private GameObject _pieceContainer;

    // The data that describes the size and style of the board
    private BoardData _boardData;
    
    // The organized list of cells based on position
    private Cell[,] _cells;

    // All currently available cells (used by AI/NPC to pick a random cell)
    private List<Cell> _availableCells;

    // We raycast against a board gameobject so this is a quick way to lookup what cell we hit
    private Dictionary<GameObject, Cell> _cellLookup;

    public BoardController(BoardData boardData)
    {
        _boardData = boardData;
    }

    // Initializes the board with the correct number of grid squares
    public void SetupBoard()
    {
        _boardContainer = new GameObject("Board");
        _pieceContainer = new GameObject("PieceContainer");

        _cells = new Cell[_boardData.BoardSize, _boardData.BoardSize];
        _cellLookup = new Dictionary<GameObject, Cell>();
        _availableCells = new List<Cell>();

        // Sets up a 2d grid of cells
        for (int x = 0; x < _cells.GetLength(0); x++)
        {
            for (int z = 0; z < _cells.GetLength(1); z++)
            {
                Vector3 position = new Vector3((x * _boardData.BoardSquareSeperation) + _boardData.BoardStartX, _boardData.BoardStartY, (z * _boardData.BoardSquareSeperation) + _boardData.BoardStartZ);
                GameObject boardSquare = GameObject.Instantiate(_boardData.BoardSquare, position, Quaternion.identity, _boardContainer.transform);
                _cells[x, z] = new Cell(boardSquare, x, z);
                _cellLookup.Add(boardSquare, _cells[x, z]);
            }
        }

        ResetAvailableCells();
    }

    // Clears and resets cells
    private void ResetAvailableCells()
    {
        _availableCells.Clear();
        for (int x = 0; x < _cells.GetLength(0); x++)
        {
            for (int z = 0; z < _cells.GetLength(1); z++)
            {
                _availableCells.Add(_cells[x, z]);
                _cells[x, z].Type = Cell.CellType.None;
            }
        }
    }

    // Resets the board for the next round
    public void ResetGameBoard()
    {
        ResetAvailableCells();
        ClearPieces();
    }

    // Destroys all of the piece gameobjects to clear the board
    private void ClearPieces()
    {
        foreach (Transform child in _pieceContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public Cell GetRandomAvailableCell()
    {
        return _availableCells[Random.Range(0, _availableCells.Count)];
    }

    public int GetAvailableCellsCount()
    {
        return _availableCells.Count;
    }

    public Cell GetCell(GameObject boardSquare)
    {
        return _cellLookup[boardSquare];
    }

    // Places pieces on the board and removes them from the available list
    public bool TryPlacePiece(Cell cell, Vector3 position, bool player1Turn)
    {
        if (cell != null)
        {
            if (cell.Type == Cell.CellType.None)
            {
                if (player1Turn)
                {
                    cell.Type = Cell.CellType.X;
                    position.y += 0.75f;
                    GameObject pieceX = GameObject.Instantiate(_boardData.XPiece, position, Quaternion.Euler(new Vector3(Random.Range(0, 15), 45, Random.Range(0, 15))), _pieceContainer.transform);
                }
                else
                {
                    cell.Type = Cell.CellType.O;
                    position.y += 0.75f;
                    GameObject pieceX = GameObject.Instantiate(_boardData.OPiece, position, Quaternion.Euler(new Vector3(Random.Range(0, 15), 45, Random.Range(0, 15))), _pieceContainer.transform);
                }
                _availableCells.Remove(cell);
                return true;
            }
        }
        return false;
    }

    // Checks for a win condition against the recently placed piece.
    // Win Condition: All pieces are the same in a row, column, or diagonal based on the board size (works for 3x3, 4x4, etc)
    public bool CheckWinCondition(Cell cell)
    {
        int xpos = cell.XPos;
        int zpos = cell.ZPos;

        // Check column of current cell
        for (int x = 0; x < _cells.GetLength(0); x++)
        {
            // If one is not matching we can exit early
            if (_cells[x, zpos].Type != cell.Type) { break; }
            // If we make it to the end of the loop we have a win condition
            else if (x == _cells.GetLength(0) - 1) { return true; }
        }

        // Check row of current cell 
        for (int z = 0; z < _cells.GetLength(1); z++)
        {
            if (_cells[xpos, z].Type != cell.Type) { break; }
            else if (z == _cells.GetLength(1) - 1) { return true; }
        }

        // Check equal diagonal
        for (int i = 0; i < _cells.GetLength(1); i++)
        {
            if (_cells[i, i].Type != cell.Type) { break; }
            else if (i == _cells.GetLength(1) - 1) { return true; }
        }

        // Check reverse diagonal
        for (int i = 0; i < _cells.GetLength(1); i++)
        {
            if (_cells[i, _cells.GetLength(1) - 1 - i].Type != cell.Type) { break; }
            else if (i == _cells.GetLength(1) - 1) { return true; }
        }

        return false;
    }
}
