using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoomInfo
{
    public string name;

    public int X;

    public int Y;
}

public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    string currentWorldName = "Level";

    RoomInfo currentLoadRoomData;

    Room currentRoom;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;

    bool spawnedBoosRoom = false;

    bool updatedRoom = false;

    public Camera camera;

    public GameObject canvas;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
       
    }

    void Update()
    {
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom)
        {
            return;
        }

        if(loadRoomQueue.Count == 0)
        {
            if (!spawnedBoosRoom)
            {
                StartCoroutine(SpawnBoosRoom());
            }
            else if (spawnedBoosRoom && !updatedRoom)
            {
                foreach(Room room in loadedRooms)
                {
                    room.RomoveUnconnectedDoors();
                }
                UpdateRooms();
                updatedRoom = true;
            }
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator SpawnBoosRoom()
    {
        spawnedBoosRoom = true;
        yield return new WaitForSeconds(0.5f);

        if(loadRoomQueue.Count == 0)
        {
            Room boosRoom = loadedRooms[loadedRooms.Count - 1];
            Room tempRoom = new Room(boosRoom.X, boosRoom.Y);
            Destroy(boosRoom.gameObject);
            var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.X, tempRoom.Y);

            canvas.SetActive(true);
            camera.gameObject.SetActive(true);
        }
    }

    public void LoadRoom(string name, int x, int y)
    {
        if(DoesRoomExist(x, y))
        {
            return;
        }

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;

        loadRoomQueue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = currentWorldName + info.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while(loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom(Room room)
    {
        if (!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position = new Vector3(currentLoadRoomData.X * room.Width, currentLoadRoomData.Y * room.Height, 0);

            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentWorldName + " - " + currentLoadRoomData.name + " " + room.X + ", " + room.Y;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if (loadedRooms.Count == 0)
            {
                //  CameraController.instance.currentRoom = room;
            }

            loadedRooms.Add(room);
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }

    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }

    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public string GetRandomRoomName()
    {
        string[] possibleRooms = new string[]
        {
            "Empty", "Basic"
        };

        return possibleRooms[Random.Range(0, possibleRooms.Length)];
    }

    public void OnPlayerEnterRoom(Room room)
    {
        // CameraController.instance.currentRoom = room;
        currentRoom = room;

        StartCoroutine(RoomCourutine());
    }

    public IEnumerator RoomCourutine()
    {
        yield return new WaitForSeconds(0.2f);
        UpdateRooms();
    }

    public void UpdateRooms()
    {
        foreach(Room room in loadedRooms)
        {
            if(currentRoom != room)
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if (enemies != null)
                {
                    foreach (EnemyController enemy in enemies)
                    { 
                        enemy.notInRoom = true;
                       // Debug.Log("Not in room");
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        switch (door.doorType)
                        {
                            case Door.DoorType.right:
                                if (room.GetRight() != null)
                                    door.doorCollider.SetActive(false);
                                break;
                            case Door.DoorType.left:
                                if (room.GetLeft() != null)
                                    door.doorCollider.SetActive(false);
                                break;
                            case Door.DoorType.top:
                                if (room.GetTop() != null)
                                    door.doorCollider.SetActive(false);
                                break;
                            case Door.DoorType.bottom:
                                if (room.GetBottom() != null)
                                    door.doorCollider.SetActive(false);
                                break;
                        }
                    }
                }
            }
            else
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if (enemies.Length > 0)
                {
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = false;
                       // Debug.Log("In room");
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        room.minimap.GetComponent<SpriteRenderer>().color = Color.red;
                        door.doorCollider.SetActive(true);
                    }
                }
                else
                {
                    if (room.name.Contains("Start"))
                    {
                        room.minimap.GetComponent<SpriteRenderer>().color = Color.yellow;
                    }
                    else if (room.name.Contains("Basic") || room.name.Contains("Empty"))
                    {
                        room.minimap.GetComponent<SpriteRenderer>().color = Color.green;
                    }
                    else
                    {
                        //room.minimap.GetComponent<SpriteRenderer>().color = Color.blue;
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {  
                        switch (door.doorType)
                        {
                            case Door.DoorType.right:
                                if (room.GetRight() != null)
                                    door.doorCollider.SetActive(false);
                                break;
                            case Door.DoorType.left:
                                if (room.GetLeft() != null)
                                    door.doorCollider.SetActive(false);
                                break;
                            case Door.DoorType.top:
                                if (room.GetTop() != null)
                                    door.doorCollider.SetActive(false);
                                break;
                            case Door.DoorType.bottom:
                                if (room.GetBottom() != null)
                                    door.doorCollider.SetActive(false);
                                break;
                        }
                    }
                }
            }
        }
    }
}
