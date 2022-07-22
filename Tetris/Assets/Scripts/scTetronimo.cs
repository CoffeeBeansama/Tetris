using UnityEngine;
using UnityEngine.Tilemaps; // Called for using Tilemaps


public enum scTetronimo
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z,

}
[System.Serializable] // Used to displaying Array in Editor
public struct Tetronimo_Data
{
    public scTetronimo tetronimo;
    public Tile tile;
    public Vector2Int[] cells {get; private set;} // Vector2 int for whole numbers
    public Vector2Int[,] wallKicks {get; private set;}

    public void Initialize()
    {
        this.cells = scData.Cells[this.tetronimo];
        this.wallKicks = scData.WallKicks[this.tetronimo];
    }



}