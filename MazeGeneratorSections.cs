using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneratorSections : MonoBehaviour
{
    public GameObject mazePiecePrefab;
    public Vector2 mazePieceSize = new Vector2(3, 3);

    public Vector2Int mazeDimensions = new Vector2Int(9, 9);
    public Vector2Int mazeSubGridDimensions = new Vector2Int(3, 3);
    private Vector2Int mazeFullDimensions;

    private List<List<MazePiece>> unvisitedGrids = new List<List<MazePiece>>();
    private List<List<MazePiece>> openGrids;
    private List<List<MazePiece>> closedGrids;

    private List<MazePiece> mazePieces = new List<MazePiece>();

    private class MazePiece
    {
        public Vector2 position;
        public bool shouldPlace = true;
        public MazePiece(Vector2 position)
        {
            this.position = position;
        }
    }
    void Start()
    {
        mazeFullDimensions = new Vector2Int(mazeDimensions.x * mazeSubGridDimensions.x, mazeDimensions.y * mazeSubGridDimensions.y);

        GenerateEmptyMaze();
        InstantiateMaze();
    }
    private void GenerateEmptyMaze()
    {
        for (int z = 0; z < mazeDimensions.y; z++)
        {
            for (int x = 0; x < mazeDimensions.x; x++)
            {
                unvisitedGrids.Add(new List<MazePiece>());
            }
        }

        //Generate initial maze grid
        for (int z = 1; z <= mazeFullDimensions.y; z++)
        {
            for (int x = 1; x <= mazeFullDimensions.x; x++)
            {
                mazePieces.Add(new MazePiece(new Vector2(x, z)));
                //Adds pieces to the correct grid
                unvisitedGrids[
                    (int)Mathf.Clamp((x - 1) / mazeSubGridDimensions.x, 0, Mathf.Infinity) +
                    ((int)Mathf.Clamp((z - 1) / mazeSubGridDimensions.y, 0, Mathf.Infinity) *
                    mazeDimensions.x)].Add(mazePieces[mazePieces.Count - 1]);
            }
        }
    }
    private void GeneratePaths()
    {
        //MazePiece randomPiece = notVisitedMazePieces[Random.Range(0, notVisitedMazePieces.Count)];
    }

    private void InstantiateMaze()
    {
        foreach(var grid in closedGrids)
        {
            foreach(var piece in grid)
            {
                if (piece.shouldPlace)
                        Instantiate(mazePiecePrefab, new Vector3(piece.position.x * mazePieceSize.x, 0, piece.position.y * mazePieceSize.y), new Quaternion());
            }
        }
    }
}
