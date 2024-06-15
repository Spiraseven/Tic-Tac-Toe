using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private BoardController _boardController;

    [SerializeField] private BoardData _BoardData;

    void Start()
    {
        _boardController = new BoardController(_BoardData);
        _boardController.SetupBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
