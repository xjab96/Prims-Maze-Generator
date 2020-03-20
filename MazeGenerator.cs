using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MazePiece
{
    public Vector3 position;
    public bool left, up, right, down;
    public MazePiece(Vector3 position, bool left, bool up, bool right, bool down)
    {
        this.left = left;
        this.up = up;
        this.right = right;
        this.down = down;
        this.position = position;
    }
}

public class MazeGenerator : MonoBehaviour
{
    public int mazeWidth = 10;
    public int mazeHeight = 10;

    public float mazePieceWidth = 5;
    public float mazePieceHeight = 5;

    private List<MazePiece> notVisitedMazePieces = new List<MazePiece>();
    private List<MazePiece> openList = new List<MazePiece>();
    private List<MazePiece> closedList = new List<MazePiece>();

    public GameObject wallPrefab;

    private void Start()
    {
        InitializeEmptyMaze();
        BuildPaths();
        SpawnMazePieces();
    }
    void InitializeEmptyMaze()
    {
        for (int z = 0; z < mazeHeight; z++)
        {
            for (int x = 0; x < mazeWidth; x++)
            {
                notVisitedMazePieces.Add(new MazePiece(new Vector3(x, 0, z), true, true, true, true));
            }
        }
    }

    void BuildPaths()
    {
        if (openList.Count == 0)
        {
            var randomPiece = notVisitedMazePieces[Random.Range(0, notVisitedMazePieces.Count)];
            openList.Add(randomPiece);
            notVisitedMazePieces.Remove(randomPiece);
        }
        while (openList.Count > 0)
        {
            //Pick a random room thats been visited -- 
            MazePiece randomPiece = openList[Random.Range(0, openList.Count)];

            //Remove random infinity possibility by setting up a list of directions
            List<int> validDirections = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                validDirections.Add(i);
            }
            CheckDirections(randomPiece, validDirections);
        }
    }

    private void CheckDirections(MazePiece chosenPiece, List<int> validDirections)
    {
        if (validDirections.Count == 0)
        {
            if ((chosenPiece.left ? 1 : 0) + 
                (chosenPiece.up ? 1 : 0) + 
                (chosenPiece.right ? 1 : 0) + 
                (chosenPiece.down ? 1 : 0) == 3)
            {
                //Detected dead ends. Item spawn code can go here?
                //Need to add a way to see if pieces either side of it are also dead ends otherwise the maze can look problematic
            }
            closedList.Add(chosenPiece);
            openList.Remove(chosenPiece);
        }
        else
        {
            //finds the index so we can find adjacent
            int direction = validDirections[Random.Range(0, validDirections.Count)];
            switch (direction)
            {
                case 0: //left
                    if (chosenPiece.position.x > 0 &&
                        notVisitedMazePieces.Exists(piece => piece.position == new Vector3(chosenPiece.position.x - 1, chosenPiece.position.y, chosenPiece.position.z)))
                    {
                        chosenPiece.left = false;

                        var directionPiece = notVisitedMazePieces.Find(piece => piece.position == new Vector3(chosenPiece.position.x - 1, chosenPiece.position.y, chosenPiece.position.z));
                        openList.Add(directionPiece);
                        notVisitedMazePieces.Remove(directionPiece);
                        directionPiece.right = false;
                    }
                    else
                    {
                        validDirections.Remove(direction);
                        CheckDirections(chosenPiece, validDirections);
                    }
                    break;

                case 1: //up
                    if ((chosenPiece.position.z < mazeHeight - 1) &&
                        notVisitedMazePieces.Exists(piece => piece.position == new Vector3(chosenPiece.position.x, chosenPiece.position.y, chosenPiece.position.z + 1)))
                    { 
                        chosenPiece.up = false;

                        var directionPiece = notVisitedMazePieces.Find(piece => piece.position == new Vector3(chosenPiece.position.x, chosenPiece.position.y, chosenPiece.position.z + 1));
                        openList.Add(directionPiece);
                        notVisitedMazePieces.Remove(directionPiece);
                        directionPiece.down = false;
                    }
                    else
                    {
                        validDirections.Remove(direction);
                        CheckDirections(chosenPiece, validDirections);
                    }
                    break;

                case 2: //right
                    if ((chosenPiece.position.x < mazeWidth - 1) &&
                        notVisitedMazePieces.Exists(piece => piece.position == new Vector3(chosenPiece.position.x + 1, chosenPiece.position.y, chosenPiece.position.z)))
                    {
                        chosenPiece.right = false;

                        var directionPiece = notVisitedMazePieces.Find(piece => piece.position == new Vector3(chosenPiece.position.x + 1, chosenPiece.position.y, chosenPiece.position.z));
                        openList.Add(directionPiece);
                        notVisitedMazePieces.Remove(directionPiece);
                        directionPiece.left = false;
                    }
                    else
                    {
                        validDirections.Remove(direction);
                        CheckDirections(chosenPiece, validDirections);
                    }
                    break;

                case 3: //down
                    if ((chosenPiece.position.x > 0) &&
                        notVisitedMazePieces.Exists(piece => piece.position == new Vector3(chosenPiece.position.x, chosenPiece.position.y, chosenPiece.position.z - 1)))
                    {
                        chosenPiece.down = false;

                        var directionPiece = notVisitedMazePieces.Find(piece => piece.position == new Vector3(chosenPiece.position.x, chosenPiece.position.y, chosenPiece.position.z - 1));
                        openList.Add(directionPiece);
                        notVisitedMazePieces.Remove(directionPiece);
                        directionPiece.up = false;
                    }
                    else
                    {
                        validDirections.Remove(direction);
                        CheckDirections(chosenPiece, validDirections);
                    }
                    break;
            }
        }
    }

    void SpawnMazePieces()
    {
        float widthOffset = mazePieceWidth / 2;
        float heightOffset = mazePieceHeight / 2;
        Quaternion wallRotation = new Quaternion();

        for (int i = 0; i < closedList.Count; i++)
        {
            MazePiece currentPiece = closedList[i];

            //If it has a wall above it or to the side of it then don't place a wall
            if ((currentPiece.position.z < mazeHeight - 1) && currentPiece.up == true)
            {
                if (closedList.Exists(piece => piece.position == new Vector3(currentPiece.position.x, currentPiece.position.y, currentPiece.position.z + 1)))
                {
                    closedList.Find(piece => piece.position == new Vector3(currentPiece.position.x, currentPiece.position.y, currentPiece.position.z + 1)).down = false;
                }
            }
            if (currentPiece.position.x > 0 && currentPiece.left == true)
            {
                if (closedList.Exists(piece => piece.position == new Vector3(currentPiece.position.x - 1, currentPiece.position.y, currentPiece.position.z)))
                {
                    closedList.Find(piece => piece.position == new Vector3(currentPiece.position.x - 1, currentPiece.position.y, currentPiece.position.z)).right = false;
                }
              
            }

            //Allows the piece to be offset correctly
            currentPiece.position = new Vector3(((currentPiece.position.x + 1) * mazePieceWidth), 0, ((currentPiece.position.z + 1) * mazePieceHeight));

            bool[] directions = new bool[4] { currentPiece.left, currentPiece.up, currentPiece.right, currentPiece.down };

            for (int directionIdx = 0; directionIdx < 4; directionIdx++)
            {
                if (directions[directionIdx] == true)
                {
                    switch (directionIdx)
                    {
                        case 0://left
                            wallRotation.eulerAngles = new Vector3(0, 0, 0);
                            Instantiate(wallPrefab, new Vector3(currentPiece.position.x - widthOffset, currentPiece.position.y, currentPiece.position.z), wallRotation);
                            break;

                        case 1://up
                            wallRotation.eulerAngles = new Vector3(0, 90, 0);
                            Instantiate(wallPrefab, new Vector3(currentPiece.position.x, currentPiece.position.y, currentPiece.position.z + heightOffset), wallRotation);
                            break;

                        case 2://right
                            wallRotation.eulerAngles = new Vector3(0, 0, 0);
                            Instantiate(wallPrefab, new Vector3(currentPiece.position.x + widthOffset, currentPiece.position.y, currentPiece.position.z), wallRotation);
                            break;

                        case 3://down
                            wallRotation.eulerAngles = new Vector3(0, 90, 0);
                            Instantiate(wallPrefab, new Vector3(currentPiece.position.x, currentPiece.position.y, currentPiece.position.z - heightOffset), wallRotation);
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}