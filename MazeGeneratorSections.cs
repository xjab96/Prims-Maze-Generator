using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneratorSections : MonoBehaviour
{
    public GameObject mazePiecePrefab;
    public Vector2 mazePieceSize = new Vector2(3, 3);

    public Vector2Int mazeDimensions = new Vector2Int(9, 9);
    public Vector2Int mazeSubGridDimensions = new Vector2Int(3, 3);
    private Vector2Int mazeGridDimensions;

    private List<MazePiece>[,] mazeGrid;
    private List<MazePiece>[,] openGrids;
    private List<MazePiece>[,] closedGrids;

    private List<List<MazePiece>> gridPieces = new List<List<MazePiece>>();
    private List<List<MazePiece>> openGridPieces;
    private List<List<MazePiece>> closedGridPieces;

    private List<MazePiece> subgridPieces = new List<MazePiece>();
    private List<MazePiece> openList = new List<MazePiece>();
    private List<MazePiece> closedList = new List<MazePiece>();

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
        mazeGridDimensions = new Vector2Int(mazeDimensions.x * mazeSubGridDimensions.x, mazeDimensions.y * mazeSubGridDimensions.y);
        mazeGrid = new List<MazePiece>[mazeDimensions.x, mazeDimensions.y];
        openGrids = mazeGrid;
        closedGrids = mazeGrid;

        GenerateEmptyMaze();
        InstantiateMaze();
    }
    private void GenerateEmptyMaze()
    {
        for (int z = 0; z < mazeDimensions.y; z++)
        {
            for (int x = 0; x < mazeDimensions.x; x++)
            {
                //mazeGrid[x, z] = new List<MazePiece>();
                gridPieces.Add(new List<MazePiece>());

            }
        }
        Debug.Log(gridPieces.Count);

        //Generate initial maze grid
        for (int z = 1; z <= mazeGridDimensions.y; z++)
        {
            for (int x = 1; x <= mazeGridDimensions.x; x++)
            {
                subgridPieces.Add(new MazePiece(new Vector2(x, z)));

                int testX = (int)Mathf.Clamp((x - 1) / mazeSubGridDimensions.x, 0, Mathf.Infinity);
                int testZ = (int)Mathf.Clamp((z - 1) / mazeSubGridDimensions.y, 0, Mathf.Infinity);


                gridPieces[testX + (testZ * mazeDimensions.x)].Add(subgridPieces[subgridPieces.Count - 1]);
                Debug.Log(testX + (testZ * mazeDimensions.x));
                //mazeGrid[(int)Mathf.Clamp((x - 1) / mazeSubGridDimensions.x, 0, Mathf.Infinity), (int)Mathf.Clamp((z - 1) / mazeSubGridDimensions.y, 0, Mathf.Infinity)].Add(subgridPieces[subgridPieces.Count - 1]);
            }
        }
    }
    private void GeneratePaths()
    {
        //MazePiece randomPiece = notVisitedMazePieces[Random.Range(0, notVisitedMazePieces.Count)];
    }

    private void InstantiateMaze()
    {
        foreach(var test in gridPieces)
        {
            foreach(var subPiece in test)
            {
                if (subPiece.shouldPlace)
                        Instantiate(mazePiecePrefab, new Vector3(subPiece.position.x * mazePieceSize.x, 0, subPiece.position.y * mazePieceSize.y), new Quaternion());
            }
        }


        //for (int z = 0; z < mazeGrid.GetLength(1); z++)
        //{
        //    for (int x = 0; x < mazeGrid.GetLength(0); x++)
        //    {
        //        for (int i = 0; i < mazeGrid[x, z].Count; i++)
        //        {
        //            if (mazeGrid[x, z][i].shouldPlace)
        //                Instantiate(mazePiecePrefab, new Vector3(mazeGrid[x, z][i].position.x * mazePieceSize.x, 0, mazeGrid[x, z][i].position.y * mazePieceSize.y), new Quaternion());
        //        }
        //    }
        //}
    }
}
