using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MazePiece
{
    public Vector3 position;
    public bool[] directions;

    public MazePiece(Vector3 position, bool left, bool up, bool right, bool down)
    {
        directions = new bool[4] { left, up, right, down };
        this.position = position;
    }
}

public class MazeGenerator : MonoBehaviour
{
    public int mazeWidth = 10;
    public int mazeHeight = 10;

    public float mazePieceWidth = 5;
    public float mazePieceHeight = 5;

    private List<MazePiece> mazePieces = new List<MazePiece>();
    private List<MazePiece> visitedMazePieces2 = new List<MazePiece>();

    //Lists of indexes. They store the value of the index 
    private List<MazePiece> notVistiedMazePieces;
    private List<MazePiece> visitedMazePieces = new List<MazePiece>();

    public GameObject wallPrefab;

    private void Start()
    {
        InitializeEmptyMaze();
        BuildPaths();
        SpawnMazePieces();
    }
    void InitializeEmptyMaze()
    {
        for (int i = 0; i < mazeHeight; i++)
        {
            for (int j = 0; j < mazeWidth; j++)
            {
                mazePieces.Add(new MazePiece(new Vector3(j, 0, i), true, true, true, true));
            }
        }
        notVistiedMazePieces = mazePieces;
    }

    //just pass the maze piece to the function directly
    //return both maze pieces after checking direction

    void BuildPaths()
    {
        if (visitedMazePieces.Count == 0)
        {
            MazePiece randomPiece = notVistiedMazePieces[Random.Range(0, notVistiedMazePieces.Count)];
            visitedMazePieces.Add(randomPiece);
            notVistiedMazePieces.Remove(randomPiece);
        }
        while (notVistiedMazePieces.Count > 0)
        {
            //Pick a random room thats been visited -- 
            MazePiece randomPiece = notVistiedMazePieces[Random.Range(0, visitedMazePieces.Count)];
            ref MazePiece test = randomPiece;
            //Remove random infinity possibility by setting up a list of directions
            List<int> validDirections = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                validDirections.Add(i);
            }
            CheckDirections(randomPiece, validDirections);
        }
        visitedMazePieces.Clear();
    }

    private MazePiece CheckDirections(MazePiece currentPiece, List<int> validDirections)
    {

        if (validDirections.Count == 0)
        {
            if ((currentPiece.directions[0] ? 1 : 0)
                + (currentPiece.directions[1] ? 1 : 0)
                + (currentPiece.directions[2] ? 1 : 0)
                + (currentPiece.directions[3] ? 1 : 0) == 3)
            {
                //Detected dead ends. Item spawn code can go here?
            }
            visitedMazePieces.Remove(currentPiece); //Removing it because it can no longer be used
            return currentPiece;
        }
        else
        {
            //finds the index so we can find adjacent
            int idx = mazePieces.FindIndex(x => x == currentPiece);
            int direction = validDirections[Random.Range(0, validDirections.Count)];
            switch (direction)
            {
                //switch detects if idx is valid, then checks if its on the same z axis(same row) then makes sure the piece hasnt already been visited
                case 0: //left

                    if (idx - 1 >= 0)
                    {
                        MazePiece searchPiece = mazePieces[idx - 1];
                        if (searchPiece.position.z == currentPiece.position.z && notVistiedMazePieces.Contains(searchPiece))
                        {
                            currentPiece.directions[direction] = false;
                            searchPiece.directions[2] = false;
                            visitedMazePieces.Add(searchPiece);
                            notVistiedMazePieces.Remove(searchPiece);
                        }
                        else
                        {
                            validDirections.Remove(direction);
                            return CheckDirections(currentPiece, validDirections);
                        }
                    }
                    break;

                case 1: //up
                    if (idx + mazeWidth <= ((mazeWidth * mazeHeight) - 1))
                    {
                        MazePiece searchPiece = mazePieces[idx + mazeWidth];
                        if (notVistiedMazePieces.Contains(searchPiece))
                        {
                            currentPiece.directions[direction] = false;
                            searchPiece.directions[3] = false;
                            visitedMazePieces.Add(searchPiece);
                            notVistiedMazePieces.Remove(searchPiece);
                        }
                        else
                        {
                            validDirections.Remove(direction);
                            return CheckDirections(currentPiece, validDirections);
                        }
                    }
                    break;

                case 2: //right
                    if (idx + 1 <= ((mazeWidth * mazeHeight) - 1))
                    {
                        MazePiece searchPiece = mazePieces[idx + 1];
                        if ( searchPiece.position.z == currentPiece.position.z && notVistiedMazePieces.Contains(searchPiece))
                        {
                            currentPiece.directions[direction] = false;
                            searchPiece.directions[1] = false;
                            visitedMazePieces.Add(searchPiece);
                            notVistiedMazePieces.Remove(searchPiece);
                        }
                        else
                        {
                            validDirections.Remove(direction);
                            return CheckDirections(currentPiece, validDirections);
                        }
                    }
                    break;

                case 3: //down
                    if (idx - mazeWidth >= 0)
                    {
                        MazePiece searchPiece = mazePieces[idx - mazeWidth];
                        if (notVistiedMazePieces.Contains(searchPiece))
                        {
                            currentPiece.directions[direction] = false;
                            searchPiece.directions[1] = false;
                            visitedMazePieces.Add(searchPiece);
                            notVistiedMazePieces.Remove(searchPiece);
                        }
                        else
                        {
                            validDirections.Remove(direction);
                            return CheckDirections(currentPiece, validDirections);
                        }
                    }
                    break;

                default:
                    break;
            }
            return currentPiece;
        }
    }

    void SpawnMazePieces()
    {
        float widthOffset = mazePieceWidth / 2;
        float heightOffset = mazePieceHeight / 2;

        Quaternion wallRotation = new Quaternion();

        for (int i = 0; i < mazePieces.Count; i++)
        {
            MazePiece curr = new MazePiece(new Vector3(0, 0), false, false, false, false);

            //If it has a wall above it or to the side of it then don't place a wall
            //if (i + mazeWidth <= ((mazeWidth * mazeHeight) - 1) && mazePieces[i + mazeWidth].directions[3])
            //{
            //    mazePieces[i].directions[1] = false;
            //}
            //if (i - 1 >= 0 && mazePieces[i].position.z == mazePieces[i - 1].position.z && mazePieces[i - 1].directions[2])
            //{
            //    mazePieces[i].directions[0] = false;
            //}
            curr = mazePieces[i];
            curr.position = new Vector3(((curr.position.x + 1) * mazePieceWidth), 0, ((curr.position.z + 1) * mazePieceHeight));

            for (int directionIdx = 0; directionIdx < 4; directionIdx++)
            {
                if (mazePieces[i].directions[directionIdx] == true)
                {
                    switch (directionIdx)
                    {
                        case 0://left
                            wallRotation.eulerAngles = new Vector3(0, 0, 0);
                            Instantiate(wallPrefab, new Vector3(curr.position.x - widthOffset, curr.position.y, curr.position.z), wallRotation);
                            break;

                        case 1://up
                            wallRotation.eulerAngles = new Vector3(0, 90, 0);
                            Instantiate(wallPrefab, new Vector3(curr.position.x, curr.position.y, curr.position.z + heightOffset), wallRotation);
                            break;

                        case 2://right
                            wallRotation.eulerAngles = new Vector3(0, 0, 0);
                            Instantiate(wallPrefab, new Vector3(curr.position.x + widthOffset, curr.position.y, curr.position.z), wallRotation);
                            break;

                        case 3://down
                            wallRotation.eulerAngles = new Vector3(0, 90, 0);
                            Instantiate(wallPrefab, new Vector3(curr.position.x, curr.position.y, curr.position.z - heightOffset), wallRotation);
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}