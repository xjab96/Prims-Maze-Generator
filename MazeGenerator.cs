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

    private MazePiece CheckDirections(MazePiece chosenPiece, List<int> validDirections)
    {
        if (validDirections.Count == 0)
        {
            if ((chosenPiece.directions[0] ? 1 : 0) + 
                (chosenPiece.directions[1] ? 1 : 0) + 
                (chosenPiece.directions[2] ? 1 : 0) + 
                (chosenPiece.directions[3] ? 1 : 0) == 3)
            {
                //Detected dead ends. Item spawn code can go here?
            }
            closedList.Add(chosenPiece);
            openList.Remove(chosenPiece);
            return chosenPiece;
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
                        chosenPiece.directions[direction] = false;

                        var directionPiece = notVisitedMazePieces.Find(piece => piece.position == new Vector3(chosenPiece.position.x - 1, chosenPiece.position.y, chosenPiece.position.z));
                        openList.Add(directionPiece);
                        notVisitedMazePieces.Remove(directionPiece);
                        directionPiece.directions[2] = false;
                    }
                    else
                    {
                        validDirections.Remove(direction);
                        return CheckDirections(chosenPiece, validDirections);
                    }
                    break;

                case 1: //up
                    if ((chosenPiece.position.z < mazeHeight - 1) &&
                        notVisitedMazePieces.Exists(piece => piece.position == new Vector3(chosenPiece.position.x, chosenPiece.position.y, chosenPiece.position.z + 1)))
                    { 
                        chosenPiece.directions[direction] = false;

                        var directionPiece = notVisitedMazePieces.Find(piece => piece.position == new Vector3(chosenPiece.position.x, chosenPiece.position.y, chosenPiece.position.z + 1));
                        openList.Add(directionPiece);
                        notVisitedMazePieces.Remove(directionPiece);
                        directionPiece.directions[3] = false;
                    }
                    else
                    {
                        validDirections.Remove(direction);
                        return CheckDirections(chosenPiece, validDirections);
                    }
                    break;

                case 2: //right
                    if ((chosenPiece.position.x < mazeWidth - 1) &&
                        notVisitedMazePieces.Exists(piece => piece.position == new Vector3(chosenPiece.position.x + 1, chosenPiece.position.y, chosenPiece.position.z)))
                    {
                        chosenPiece.directions[direction] = false;

                        var directionPiece = notVisitedMazePieces.Find(piece => piece.position == new Vector3(chosenPiece.position.x + 1, chosenPiece.position.y, chosenPiece.position.z));
                        openList.Add(directionPiece);
                        notVisitedMazePieces.Remove(directionPiece);
                        directionPiece.directions[0] = false;
                    }
                    else
                    {
                        validDirections.Remove(direction);
                        return CheckDirections(chosenPiece, validDirections);
                    }
                    break;

                case 3: //down
                    if ((chosenPiece.position.x > 0) &&
                        notVisitedMazePieces.Exists(piece => piece.position == new Vector3(chosenPiece.position.x, chosenPiece.position.y, chosenPiece.position.z - 1)))
                    {
                        chosenPiece.directions[direction] = false;

                        var directionPiece = notVisitedMazePieces.Find(piece => piece.position == new Vector3(chosenPiece.position.x, chosenPiece.position.y, chosenPiece.position.z - 1));
                        openList.Add(directionPiece);
                        notVisitedMazePieces.Remove(directionPiece);
                        directionPiece.directions[1] = false;
                    }
                    else
                    {
                        validDirections.Remove(direction);
                        return CheckDirections(chosenPiece, validDirections);
                    }
                    break;
            }
            return chosenPiece;
        }
    }

    void SpawnMazePieces()
    {
        float widthOffset = mazePieceWidth / 2;
        float heightOffset = mazePieceHeight / 2;

        Quaternion wallRotation = new Quaternion();

        for (int i = 0; i < closedList.Count; i++)
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
            curr = closedList[i];
            curr.position = new Vector3(((curr.position.x + 1) * mazePieceWidth), 0, ((curr.position.z + 1) * mazePieceHeight));

            for (int directionIdx = 0; directionIdx < 4; directionIdx++)
            {
                if (closedList[i].directions[directionIdx] == true)
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