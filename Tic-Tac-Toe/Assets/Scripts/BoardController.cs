using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BoardController
{
    private GameObject _boardContainer;
    private BoardData _boardData;
    private Cell[,] _cells;

    public BoardController(BoardData boardData)
    {
        _boardData = boardData;
    }

    public void SetupBoard()
    {
        _boardContainer = new GameObject("Board");

        _cells = new Cell[_boardData.BoardSize,_boardData.BoardSize];

        for(int x = 0; x<_cells.GetLength(0); x++)
        {
            for (int z = 0; z < _cells.GetLength(1); z++)
            {
                Vector3 position = new Vector3((x * _boardData.BoardSquareSeperation) + _boardData.BoardStartX, _boardData.BoardStartY, (z * _boardData.BoardSquareSeperation)+_boardData.BoardStartZ);
                GameObject boardSquare = GameObject.Instantiate(_boardData.BoardSquare, position, Quaternion.identity, _boardContainer.transform);
                _cells[x, z] = new Cell(boardSquare.GetComponent<CellInteractor>());
            }
        }
        
    }
}
