using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MapData", menuName = "TECH-D/MapData", order = 0)]
public class MapData : ScriptableObject{
    public List<Building> buildingData;
}


[System.Serializable]
public class Building {
    public string buildingName;
    public string description;
    public List<Room> roomChildren;
}

[System.Serializable]
public class Room {
    public string roomName;
    public string roomDescription;
}
