using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePiece
{
    public Vector3 position;
    public bool[] directions;

    public MazePiece(Vector3 position, bool left, bool up, bool right, bool down)
    {
        directions = new bool[4]{ left, up, right, down };
        this.position = position;
    }

}

public class MazeGenerator : MonoBehaviour
{
    public int mazeWidth = 10;
    public int mazeHeight = 10;

    public int mazePieceWidth = 5;
    public int mazePieceHeight = 5;

    private List<MazePiece> mazePieces = new List<MazePiece>();

    public GameObject wallPrefab;

    private void Start()
    {
        InitializeMaze();
    }
    void InitializeMaze()
    {
        int k = 0;
        Debug.Log(mazeHeight);
        for(int i = 0; i < mazeHeight; i++)
        {
            for(int j = 0; j < mazeWidth; j++)
            {
                mazePieces.Add(new MazePiece(new Vector3(j, 0, i), true, true, true, true));
                //If it has a wall above it or to the side of it then don't place a wall
                //if (k - mazeWidth > 0)
                //{
                //    mazePieces[k].directions[1] = false;
                //}
                //if(mazePieces[k].position.x != 0)
                //{
                //    mazePieces[k].directions[0] = false;
                //}
                //k++;
            }
        }
        //for (int i = 0; i < mazePieces.Count; i++)
        //{
        //    for(int j = 0; j< mazePieces[i].directions.Length; j++)
        //    Debug.Log("Bools = " + mazePieces[i].directions[j]);
        //}
        SpawnMazePieces();
    }

    void SpawnMazePieces()
    {
        for (int i = 0; i < mazePieces.Count; i++)
        {
            MazePiece curr = new MazePiece(new Vector3(0,0), false, false, false, false);
            curr = mazePieces[i];
            curr.position = new Vector3(((curr.position.x + 1) * mazePieceWidth), 0, ((curr.position.z + 1) * -mazePieceHeight));

            Quaternion wallRotation = new Quaternion();
            float widthOffset = mazePieceWidth / 2;
            float heightOffset = mazePieceHeight / 2;

            for (int directionIdx = 0; directionIdx < 4; directionIdx++)
            {
                if(mazePieces[i].directions[directionIdx] == true)
                {
                    switch(directionIdx)
                    {
                        case 0://left
                            wallRotation.eulerAngles = new Vector3(0, 0, 0);
                            Debug.Log("left pos " + curr.position);
                            Instantiate(wallPrefab, new Vector3(curr.position.x - widthOffset, curr.position.y, curr.position.z), wallRotation);
                            break;

                        case 1:
                            wallRotation.eulerAngles = new Vector3(0, 90, 0);
                            Debug.Log("up pos " + curr.position);
                            Instantiate(wallPrefab, new Vector3(curr.position.x, curr.position.y, curr.position.z + heightOffset), wallRotation);
                            break;

                        case 2:
                            wallRotation.eulerAngles = new Vector3(0, 0, 0);
                            Debug.Log("right pos " + curr.position);
                            Instantiate(wallPrefab, new Vector3(curr.position.x + widthOffset, curr.position.y, curr.position.z), wallRotation);
                            break;

                        case 3:
                            Debug.Log("Bottom pos " + curr.position);
                            wallRotation.eulerAngles = new Vector3(0, 90, 0);
                            Instantiate(wallPrefab, new Vector3(curr.position.x, curr.position.y, curr.position.z - heightOffset), wallRotation);
                            Debug.Log("Bottom pos new pos " + curr.position);
                            break;

                        default:
                            break;
                    }

                }
            }
        }
    }
}