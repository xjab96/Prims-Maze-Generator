using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneratorSections : MonoBehaviour
{
    public GameObject mazePiecePrefab;
    public Vector2Int mazeDimensions = new Vector2Int(10, 10);
    public Vector2Int mazeSubGridDimensions = new Vector2Int(3, 3);
    private Vector2Int mazeGridDimensions;

    private List<MazePiece>[,] mazeGrid;
    private List<MazePiece> notVisitedMazePieces = new List<MazePiece>();
    private List<MazePiece> openList;
    private List<MazePiece> closedList;

    private class MazePiece
    {
        public Vector2 position;
        public bool shouldPlace = true;
        public MazePiece(Vector2 position)
        {
            this.position = position;
        }
    }
    /// <summary>
    /// Create a new empty maze in the same way as before but use bools. Then create a new maze in each of those squares
    /// Create the whole maze then seperate into the squares
    /// 
    /// 
    /// 
    /// 
    /// </summary>

    void Start()
    {
        mazeGridDimensions = new Vector2Int(mazeDimensions.x * mazeSubGridDimensions.x, mazeDimensions.y * mazeSubGridDimensions.y);
        mazeGrid = new List<MazePiece>[mazeDimensions.x, mazeDimensions.y];
        
        GenerateEmptyMaze();
    }
    private void GenerateEmptyMaze()
    {
        int pieceX = -1;
        int pieceZ = -1;
        //Generate initial maze grid
        for (int z = 0; z < mazeGridDimensions.y - 1; z++)
        {
            for (int x = 0; x < mazeGridDimensions.x - 1; x++)
            {
                if(pieceX != (x + 1) / mazeSubGridDimensions.x || pieceZ != (z + 1) / mazeSubGridDimensions.y)
                {
                    Debug.Log("hit");
                    pieceX = (x + 1) / mazeSubGridDimensions.x;
                    pieceZ = (z + 1) / mazeSubGridDimensions.y;
                    mazeGrid[pieceX, pieceZ] = new List<MazePiece>();
                }

                notVisitedMazePieces.Add(new MazePiece(new Vector2(x, z)));
                mazeGrid[pieceX, pieceZ].Add(notVisitedMazePieces[notVisitedMazePieces.Count - 1]);

                Debug.Log(mazeGrid[pieceX, pieceZ][mazeGrid[pieceX, pieceZ].Count - 1].position);
            }
        }


    }
}
