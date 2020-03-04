using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePiece
{
    public Vector3 position;
    public bool up, left, right, down;

    public MazePiece(Vector3 position, bool left, bool up, bool right, bool down) 
    {
        this.up = up;
        this.left = left;
        this.down = down;
        this.right = right;
        this.position = position;
    }

    public void RemoveWall(int direction)
    {
        switch(direction)
        {
            case 0:
                left = false;
                break;

            case 1:
                up = false;
                break;

            case 2:
                right = false;
                break;

            case 3:
                down = false;
                break;

            default:
                //Debug.Log("no wall was removed");
                break;
        }
    }

    public string ToString()
    {
        return "The piece is placed at " + position.ToString() +
            "\nLeft connection is " + left.ToString() +
            "\nTop connection is " + up.ToString() +
            "\nRight connection is " + right.ToString() +
            "\nDown connection is " + down.ToString();
    }
}

public class MazeGenerator : MonoBehaviour
{
    public int mazeWidth = 10;
    public int mazeHeight = 10;

    public int mazePieceWidth = 5;
    public int mazePieceHeight = 5;

    private List<MazePiece> mazePieces = new List<MazePiece>();

    GameObject WallPrefab;

    private void Start()
    {
        InitializeMaze();
    }
    void InitializeMaze()
    {
        int k = 0;
        for(int i = 0; i < mazeHeight; i++)
        {
            for(int j = 0; j < mazeWidth; j++)
            {
                mazePieces.Add(new MazePiece(new Vector3(j, 0, i), true, true, true, true));
                //If it has a wall above it or to the side of it then don't place a wall
                if (k - mazeWidth < 0)
                {
                    mazePieces[k].up = false;
                }
                if(mazePieces[k].position.x != 0)
                {
                    mazePieces[k].left = false;
                }
                k++;
            }
        }
        //for (int i = 0; i < mazePieces.Count; i++)
        //{
        //    Debug.Log("indx = " + i + " pos = " + mazePieces[i].position);
        //}
        SpawnMazePieces();
    }

    void SpawnMazePieces()
    {
        for(int i = 0; i < mazePieces.Count; i++)
        {
            MazePiece curr = new MazePiece(new Vector3(0,0), false, false, false, false);
            curr = mazePieces[i];
            curr.position = new Vector3(((curr.position.x + 1) * mazePieceWidth), 0, ((curr.position.z + 1) * mazePieceHeight));
        }
    }

    bool CheckDirections()
    {
        //check if index is will be null
        //check if
        return false;
    }

}