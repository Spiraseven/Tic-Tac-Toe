using UnityEngine;

/*
 * Data to setup a board with. Can be used to test boards of different sizes and styles.
 */

[CreateAssetMenu(fileName = "BoardData", menuName = "ScriptableObjects/BoardData", order = 1)]
public class BoardData : ScriptableObject
{
    public int BoardSize = 3;
    public float BoardStartX = -1.3f;
    public float BoardStartY = 0.05f;
    public float BoardStartZ = -1.3f;
    public float BoardSquareSeperation = 1.3f;
    public GameObject BoardSquare;
    public GameObject BoardX;
    public GameObject BoardO;
}
