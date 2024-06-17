using System;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Manages user input through Unity's new Input System. 
 * Will trigger events when the user selects a spot to place a piece on the board.
 */
public class InputManager : MonoBehaviour
{
    private int _boardLayerMask;

    public static event Action<GameObject> CellSelected;

    private void Awake()
    {
        _boardLayerMask = LayerMask.GetMask("Board");
    }

    public void OnMouseBoardSelect()
    {
        // Don't accept input during NPC turn or when on Standby
        if ((GameManager.GameMode == GameManager.Mode.SinglePlayer && GameManager.GameState == GameManager.State.Player2Turn)
            || GameManager.GameState == GameManager.State.Standby)
        {
            return;
        }

        // Cast a ray to select one of the board squares and trigger an event on hit
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _boardLayerMask))
        {
            if (hit.collider.CompareTag("BoardSquare"))
            {
                CellSelected?.Invoke(hit.collider.gameObject);
            }
        }
    }
}
