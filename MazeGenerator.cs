﻿using System.Collections;
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

    //Lists of indexes. They store the value of the index 
    private List<int> unusedMazePieces = new List<int>();
    private List<int> visitedMazePieces = new List<int>();

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
                unusedMazePieces.Add((mazePieces.Count - 1)); //makes the list increment by 1
            }
        }
    }

    void BuildPaths()
    {
        if (visitedMazePieces.Count == 0)
        {
            int randomIdx = Random.Range(0, unusedMazePieces.Count);
            visitedMazePieces.Add(unusedMazePieces[randomIdx]);
            unusedMazePieces.RemoveAt(randomIdx);
        }
        while (unusedMazePieces.Count > 0)
        {
            //Pick a random room thats been visited -- 
            int randomIdx = visitedMazePieces[Random.Range(0, visitedMazePieces.Count)];
            List<int> validDirections = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                validDirections.Add(i);
            }
            CheckDirections(randomIdx, validDirections);
        }
        Debug.Log("beaten the awful while loop");
    }

    private int CheckDirections(int idx, List<int> validDirections)
    {

        if (validDirections.Count == 0)
        {
            if ((mazePieces[idx].directions[0] ? 1 : 0) + (mazePieces[idx].directions[1] ? 1 : 0) + (mazePieces[idx].directions[2] ? 1 : 0) + (mazePieces[idx].directions[3] ? 1 : 0) == 3)
            {
                //Detected dead ends. Item spawn code can go here?
            }
            visitedMazePieces.Remove(idx); //Removing it because it can no longer be used
            return -1;
        }
        else
        {
            foreach (int o in validDirections)
            {
                Debug.Log(o);
            }
            int direction = validDirections[Random.Range(0, validDirections.Count)];
            switch (direction)
            {
                //switch detects if idx is valid, then checks if its on the same z axis(same row) then makes sure the piece hasnt already been visited
                case 0: //left
                    if (idx - 1 >= 0 && mazePieces[idx - 1].position.z == mazePieces[idx].position.z && unusedMazePieces.Contains(idx - 1))
                    {
                        mazePieces[idx].directions[direction] = false;
                        mazePieces[idx - 1].directions[2] = false;
                        visitedMazePieces.Add(idx - 1);
                        unusedMazePieces.Remove(idx - 1);
                    }
                    else
                    {
                        validDirections.Remove(direction);
                        return CheckDirections(idx, validDirections);
                    }
                    break;

                case 1: //up
                    if (idx - mazeWidth >= 0 && mazePieces[idx - mazeWidth].position.x == mazePieces[idx].position.x && unusedMazePieces.Contains(idx - mazeWidth))
                    {
                        mazePieces[idx].directions[direction] = false;
                        mazePieces[idx - mazeWidth].directions[3] = false;
                        visitedMazePieces.Add(idx - mazeWidth);
                        unusedMazePieces.Remove(idx - mazeWidth);
                    }
                    else
                    {
                        validDirections.Remove(direction);
                        return CheckDirections(idx, validDirections);
                    }
                    break;

                case 2: //right
                    if (idx + 1 <= (mazeWidth * mazeHeight) - 1 && mazePieces[idx + 1].position.z == mazePieces[idx].position.z && unusedMazePieces.Contains(idx + 1))
                    {
                        mazePieces[idx].directions[direction] = false;
                        mazePieces[idx + 1].directions[1] = false;
                        visitedMazePieces.Add(idx + 1);
                        unusedMazePieces.Remove(idx + 1);
                    }
                    else
                    {
                        validDirections.Remove(direction);
                        return CheckDirections(idx, validDirections);
                    }
                    break;

                case 3: //down
                    if (idx + mazeWidth <= (mazeWidth * mazeHeight) - 1 && mazePieces[idx + mazeWidth].position.x == mazePieces[idx].position.x && unusedMazePieces.Contains(idx + mazeWidth))
                    {
                        mazePieces[idx].directions[direction] = false;
                        mazePieces[idx + mazeWidth].directions[1] = false;
                        visitedMazePieces.Add(idx + mazeWidth);
                        unusedMazePieces.Remove(idx + mazeWidth);
                    }
                    else
                    {
                        validDirections.Remove(direction);
                        return CheckDirections(idx, validDirections);
                    }
                    break;

                default:
                    break;
            }
            return direction;
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
            if (i - mazeWidth > 0)
            {
                mazePieces[i].directions[1] = false;
            }
            if (mazePieces[i].position.x != 0)
            {
                mazePieces[i].directions[0] = false;
            }

            curr = mazePieces[i];
            curr.position = new Vector3(((curr.position.x + 1) * mazePieceWidth), 0, ((curr.position.z + 1) * -mazePieceHeight));

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