using UnityEngine;

/*
 * Class that holds data about each spot on the game board.
 */
public class Cell
{
    public enum CellType
    {
        None = 0,
        X = 1,
        O = 2,
    }

    public int XPos;
    public int ZPos;
    public CellType Type;
    public GameObject BoardSquare;

    public Cell(GameObject boardSquare, int xpos, int zpos)
    {
        Type = CellType.None;
        BoardSquare = boardSquare;
        XPos = xpos;
        ZPos = zpos;
    }
}
