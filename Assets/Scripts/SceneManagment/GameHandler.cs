using Player;
using Saveables;
using Saving;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public SaveDataLoader loader;
    public List<DoorInteractor> doors;

    private void Start()
    {
        if (SaveFramework.SaveDataExists("PlayerPosition"))
        {
            Vector2 pos = SaveFramework.GetSaveData<SaveableVector3>("PlayerPosition");
            PlayerData.GetTransform().position = pos;
        }
    }

    private void OnApplicationQuit()
    {
        loader.SetPlayerPosition(PlayerData.GetTransform().position);
        foreach (var door in doors)
        {
            loader.SetDoor(door.doorID, door.open);
        }
    }
}
