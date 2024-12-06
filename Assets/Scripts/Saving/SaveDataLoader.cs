using Saveables;
using Saving;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataLoader : MonoBehaviour
{
    public bool completedTutorial;
    public string saveFileName = "GameSave";

    private void Awake()
    {
        SaveFramework.CreateNewSave(saveFileName, "save");
        SaveFramework.LoadSave(saveFileName, "save");

        if (SaveFramework.SaveDataExists("HasCompletedTutorial"))
            completedTutorial = SaveFramework.GetSaveData<bool>("HasCompletedTutorial");
    }

    public void HasCompletedTutorial()
    {
        SaveFramework.NewSaveData("HasCompletedTutorial", true);
    }

    public void SetPlayerPosition(Vector2 pos)
    {
        SaveFramework.NewSaveData("PlayerPosition", (SaveableVector3)pos);
    }

    public void SetDoor(int doorID, bool open)
    {
        SaveFramework.NewSaveData("DoorID" + doorID, open);
    }

    public bool GetDoor(int doorID)
    {
        if (SaveFramework.SaveDataExists("DoorID" + doorID))
            return SaveFramework.GetSaveData<bool>("DoorID" + doorID);
        else
            return false;
    }

    private void OnApplicationQuit()
    {
        SaveFramework.Save();
    }

    private void OnDisable()
    {
        SaveFramework.Save();
    }
}
