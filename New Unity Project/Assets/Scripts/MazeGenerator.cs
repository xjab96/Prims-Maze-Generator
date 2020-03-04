using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePiece
{
    public Vector2 position;
    public bool up, left, right, down;

    public MazePiece(Vector2 position, bool left, bool up, bool right, bool down) 
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
  
}