using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class RoomGenerator : MonoBehaviour
{
    public enum Direction { Up, Down, Left, Right }

    [Header("Room Settings")]
    public GameObject roomPrefab;
    public int roomNumber;
    public Color startColor, endColor;
    private int gridSize = 10;
    private GameObject[,] grid;
    private List<GameObject> rooms = new List<GameObject>();
    private GameObject startRoom, endRoom;

    [Header("Position Settings")]
    public Transform generatorPoint;
    public float xOffset;
    public float yOffset;
    public float perlinOffsetX;
    public float perlinOffsetY;

    public GameObject perlinOffsetXSlider;
    public GameObject perlinOffsetYSlider;
    public GameObject roomNumberSlider;
    public GameObject applyButton;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void GenerateRoom()
    {
        grid = new GameObject[gridSize, gridSize];
        InitializePositions();
        startRoom.GetComponent<SpriteRenderer>().color = startColor;
        endRoom.GetComponent<SpriteRenderer>().color = endColor;
    }

    void InitializePositions()
    {
        Vector2Int pos = new Vector2Int(gridSize / 2, gridSize / 2);
        Stack<Vector2Int> lastPositionStack = new Stack<Vector2Int>();
        grid[pos.x, pos.y] = Instantiate(roomPrefab, generatorPoint.position, Quaternion.identity);
        rooms.Add(grid[pos.x, pos.y]);
        startRoom = rooms[0];
        startRoom.name = "Room 0";

        for (int i = 1; i < roomNumber; i++)
        {
            List<Direction> availableDirections = new List<Direction>();

            if (pos.y + 1 < gridSize && grid[pos.x, pos.y + 1] == null) availableDirections.Add(Direction.Up);
            if (pos.y - 1 >= 0 && grid[pos.x, pos.y - 1] == null) availableDirections.Add(Direction.Down);
            if (pos.x - 1 >= 0 && grid[pos.x - 1, pos.y] == null) availableDirections.Add(Direction.Left);
            if (pos.x + 1 < gridSize && grid[pos.x + 1, pos.y] == null) availableDirections.Add(Direction.Right);

            if (availableDirections.Count > 0)
            {
                // perlinOffsetX = UnityEngine.Random.Range(0f, 100f);
                // perlinOffsetY = UnityEngine.Random.Range(0f, 100f);

                float perlinValue = Mathf.PerlinNoise(perlinOffsetX, perlinOffsetY);
                //Debug.Log(Math.Round(perlinValue, 1));
                int index = Mathf.FloorToInt(perlinValue * availableDirections.Count);
                if (index >= availableDirections.Count)
                {
                    index = availableDirections.Count - 1;
                }
                Direction dir = availableDirections[index];
                // Debug.Log(dir);

                //Direction dir = availableDirections[UnityEngine.Random.Range(0, availableDirections.Count)];


                switch (dir)
                {
                    case Direction.Up:
                        pos.y++;
                        break;
                    case Direction.Down:
                        pos.y--;
                        break;
                    case Direction.Left:
                        pos.x--;
                        break;
                    case Direction.Right:
                        pos.x++;
                        break;
                }

                Vector3 newPosition = new Vector3(generatorPoint.position.x + (pos.x - gridSize / 2) * xOffset, generatorPoint.position.y + (pos.y - gridSize / 2) * yOffset, 0);
                grid[pos.x, pos.y] = Instantiate(roomPrefab, newPosition, Quaternion.identity);
                rooms.Add(grid[pos.x, pos.y]);
                rooms[i].name = "Room " + i;
                lastPositionStack.Push(pos);
            }
            else
            {
                if (lastPositionStack.Count > 0)
                {
                    pos = lastPositionStack.Pop();
                    i--;
                }
                else
                {
                    Debug.Log("No available directions");
                    break;
                }
            }
        }

        //use bfs to find the farthest room
        Vector2Int start = new Vector2Int(gridSize / 2, gridSize / 2);
        Vector2Int endPos = start;
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);
        int[,] distance = new int[gridSize, gridSize];
        bool[,] visited = new bool[gridSize, gridSize];
        visited[start.x, start.y] = true;
        distance[start.x, start.y] = 0;

        while (queue.Count > 0)
        {
            Vector2Int currentPos = queue.Dequeue();
            int currentDistance = distance[currentPos.x, currentPos.y];

            Vector2Int[] directions = new Vector2Int[]
            {new Vector2Int(0, 1),new Vector2Int(0, -1),new Vector2Int(-1, 0),new Vector2Int(1, 0)};

            foreach (Vector2Int dir in directions)
            {
                Vector2Int newPos = new Vector2Int(currentPos.x + dir.x, currentPos.y + dir.y);
                if (newPos.x >= 0 && newPos.x < gridSize && newPos.y >= 0 && newPos.y < gridSize && grid[newPos.x, newPos.y] != null)
                {
                    if (!visited[newPos.x, newPos.y])
                    {
                        queue.Enqueue(newPos);
                        visited[newPos.x, newPos.y] = true;
                        distance[newPos.x, newPos.y] = currentDistance + 1;
                        if (distance[newPos.x, newPos.y] > distance[endPos.x, endPos.y])
                        {
                            endPos = newPos;
                        }
                    }
                }
            }
        }

        endRoom = grid[endPos.x, endPos.y];
    }

    public void setPerlinOffsetX()
    {
        perlinOffsetX = perlinOffsetXSlider.GetComponent<UnityEngine.UI.Slider>().value;
    }

    public void setPerlinOffsetY()
    {
        perlinOffsetY = perlinOffsetYSlider.GetComponent<UnityEngine.UI.Slider>().value;
    }

    public void setRoomNumber()
    {
        roomNumber = (int)roomNumberSlider.GetComponent<UnityEngine.UI.Slider>().value;
    }

    public void applyButtonOnClick()
    {
        DestoryRoom();
        GenerateRoom();
    }

    public void DestoryRoom()
    {
        foreach (GameObject room in rooms)
        {
            Destroy(room);
        }
        rooms.Clear();
    }
}
