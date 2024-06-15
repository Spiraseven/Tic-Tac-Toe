using UnityEngine;

public class Cell
{
    public enum CellType
    {
        None = 0,
        X = 1,
        O = 2,
    }

    public CellType Type;
    public CellInteractor Interactor;
    public GameObject Piece;

    public Cell(CellInteractor interactor)
    {
        Type = CellType.None;
        Interactor = interactor;
        Piece = null;
    }
}
