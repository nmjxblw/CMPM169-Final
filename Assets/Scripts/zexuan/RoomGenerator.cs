using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using TMPro;

public class RoomGenerator : MonoBehaviour
{
    public enum Direction { Up, Down, Left, Right }

    [Header("Room Settings")]
    public GameObject roomPrefab;
    public int roomNumber;
    public Color startColor, endColor;
    private int gridSize = 10;
    private Room[,] grid;
    private List<Room> rooms = new List<Room>();
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
        applyButtonOnClick();
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
        grid = new Room[gridSize, gridSize];
        InitializePositions();
        startRoom.GetComponent<SpriteRenderer>().color = startColor;
        endRoom.GetComponent<SpriteRenderer>().color = endColor;
    }

    void InitializePositions()
    {
        Vector2Int pos = new Vector2Int(gridSize / 2, gridSize / 2);
        Stack<Vector2Int> lastPositionStack = new Stack<Vector2Int>();
        grid[pos.x, pos.y] = Instantiate(roomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<Room>();
        rooms.Add(grid[pos.x, pos.y]);
        startRoom = rooms[0].gameObject;
        startRoom.name = "Room 0";
        grid[pos.x, pos.y].roomStep = 0;
        grid[pos.x, pos.y].isStartRoom = true;

        for (int i = 1; i < roomNumber - 1; i++)
        {
            List<Direction> availableDirections = new List<Direction>();

            if (pos.y + 1 < gridSize && grid[pos.x, pos.y + 1] == null)
            {
                availableDirections.Add(Direction.Up);
            }

            if (pos.y - 1 >= 0 && grid[pos.x, pos.y - 1] == null)
            {
                availableDirections.Add(Direction.Down);
            }

            if (pos.x - 1 >= 0 && grid[pos.x - 1, pos.y] == null)
            {
                availableDirections.Add(Direction.Left);
            }

            if (pos.x + 1 < gridSize && grid[pos.x + 1, pos.y] == null)
            {
                availableDirections.Add(Direction.Right);
            }


            if (availableDirections.Count > 0)
            {
                Direction dir = availableDirections[UnityEngine.Random.Range(0, availableDirections.Count)];

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
                grid[pos.x, pos.y] = Instantiate(roomPrefab, newPosition, Quaternion.identity).GetComponent<Room>();
                rooms.Add(grid[pos.x, pos.y]);
                rooms[i].gameObject.name = "Room " + i;
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

        //use BFS to find the farthest room
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
            Room currentRoom = grid[currentPos.x, currentPos.y];
            currentRoom.roomStep = currentDistance;

            Vector2Int[] directions = new Vector2Int[]
            {new Vector2Int(0, 1),new Vector2Int(0, -1),new Vector2Int(-1, 0),new Vector2Int(1, 0)};

            foreach (Vector2Int dir in directions)
            {
                Vector2Int newPos = new Vector2Int(currentPos.x + dir.x, currentPos.y + dir.y);
                if (newPos.x >= 0 && newPos.x < gridSize && newPos.y >= 0 && newPos.y < gridSize)
                {
                    if (grid[newPos.x, newPos.y] != null)
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

                        //connect the rooms
                        Room adjacentRoom = grid[newPos.x, newPos.y];
                        if (dir == new Vector2Int(0, 1))
                        {
                            currentRoom.roomUp = true;
                            adjacentRoom.roomDown = true;
                        }
                        else if (dir == new Vector2Int(0, -1))
                        {
                            currentRoom.roomDown = true;
                            adjacentRoom.roomUp = true;
                        }
                        else if (dir == new Vector2Int(-1, 0))
                        {
                            currentRoom.roomLeft = true;
                            adjacentRoom.roomRight = true;
                        }
                        else if (dir == new Vector2Int(1, 0))
                        {
                            currentRoom.roomRight = true;
                            adjacentRoom.roomLeft = true;
                        }
                    }
                }
            }
        }

        // Generate the final room that is only connected to the farthest room
        Vector2Int finalRoomPos = endPos;
        List<Direction> finalAvailableDirections = new List<Direction>();

        if (endPos.y + 1 < gridSize && grid[endPos.x, endPos.y + 1] == null)
        {
            finalAvailableDirections.Add(Direction.Up);
        }
        if (endPos.y - 1 >= 0 && grid[endPos.x, endPos.y - 1] == null)
        {
            finalAvailableDirections.Add(Direction.Down);
        }
        if (endPos.x - 1 >= 0 && grid[endPos.x - 1, endPos.y] == null)
        {
            finalAvailableDirections.Add(Direction.Left);
        }
        if (endPos.x + 1 < gridSize && grid[endPos.x + 1, endPos.y] == null)
        {
            finalAvailableDirections.Add(Direction.Right);
        }

        bool roomPlaced = false;
        while (!roomPlaced && finalAvailableDirections.Count > 0)
        {
            Direction finalDir = finalAvailableDirections[UnityEngine.Random.Range(0, finalAvailableDirections.Count)];
            finalAvailableDirections.Remove(finalDir);

            Vector2Int newFinalPos = endPos;
            switch (finalDir)
            {
                case Direction.Up:
                    newFinalPos.y++;
                    break;
                case Direction.Down:
                    newFinalPos.y--;
                    break;
                case Direction.Left:
                    newFinalPos.x--;
                    break;
                case Direction.Right:
                    newFinalPos.x++;
                    break;
            }

            if (newFinalPos.x >= 0 && newFinalPos.x < gridSize && newFinalPos.y >= 0 && newFinalPos.y < gridSize)
            {
                if (grid[newFinalPos.x, newFinalPos.y] == null)
                {
                    Vector3 finalPosition = new Vector3(generatorPoint.position.x + (newFinalPos.x - gridSize / 2) * xOffset, generatorPoint.position.y + (newFinalPos.y - gridSize / 2) * yOffset, 0);
                    grid[newFinalPos.x, newFinalPos.y] = Instantiate(roomPrefab, finalPosition, Quaternion.identity).GetComponent<Room>();
                    rooms.Add(grid[newFinalPos.x, newFinalPos.y]);
                    grid[newFinalPos.x, newFinalPos.y].gameObject.name = "Room " + (roomNumber - 1);
                    roomPlaced = true;

                    // Update connections for the final room
                    Room finalRoom = grid[newFinalPos.x, newFinalPos.y];
                    Room previousRoom = grid[endPos.x, endPos.y];
                    if (finalDir == Direction.Up)
                    {
                        previousRoom.roomUp = true;
                        finalRoom.roomDown = true;
                    }
                    else if (finalDir == Direction.Down)
                    {
                        previousRoom.roomDown = true;
                        finalRoom.roomUp = true;
                    }
                    else if (finalDir == Direction.Left)
                    {
                        previousRoom.roomLeft = true;
                        finalRoom.roomRight = true;
                    }
                    else if (finalDir == Direction.Right)
                    {
                        previousRoom.roomRight = true;
                        finalRoom.roomLeft = true;
                    }
                    
                    finalRoom.roomStep = previousRoom.roomStep + 1;
                    finalRoom.isEndRoom = true;
                    previousRoom.isBeforeEndRoom = true;
                    endRoom = finalRoom.gameObject;
                    
                }
            }
        }
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
        foreach (Room room in rooms)
        {
            Destroy(room.gameObject);
        }
        rooms.Clear();
    }


}
