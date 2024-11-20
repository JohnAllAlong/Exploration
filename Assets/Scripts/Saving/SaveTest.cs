using UnityEngine;
using Saving;
using Saveables;

public class SaveTest : MonoBehaviour
{
    public GameObject test;

    protected void Awake()
    {
        SaveFramework.CreateDefaultSave();
        SaveFramework.LoadDefaultSave();

        //get data from a seperate save (ADVANCED)

            //register all the saves in the save directory
        //SaveFramework.RegisterAllSaves();
            //using the list of registered save data, try to get a certian data piece from them. Returns the first save to have this data name (if more than one)
        //print(SaveFramework.TryGetRegisteredSaveData<SaveableVector3>("TestingPos"));


        //get data from the current save (EASY)

            //simply request the data as so
        //print(SaveFramework.GetSaveData<SaveableVector3>("TestingPos"));


        //create a custom save

            //create a custom save with name and ext (Testing.save)
        //SaveFramework.CreateNewSave("Testing", "save");
            //load custom save (Testing.save) into the system for usage
        //SaveFramework.LoadSave("Testing", "save");
    }

    protected void Start()
    {
        //load data into the game
        if (SaveFramework.SaveDataExists("TestingPos") && SaveFramework.SaveDataExists("TestingRot"))
        {
            Vector3 loadedPos = SaveFramework.GetSaveData<SaveableVector3>("TestingPos");
            Quaternion loadedRot = SaveFramework.GetSaveData<SaveableQuaternion>("TestingRot");
            test.transform.position = loadedPos;
            test.transform.rotation = loadedRot;
        }
    }

    protected void OnApplicationQuit()
    {
        SaveFramework.NewSaveData("TestingPos", (SaveableVector3)test.transform.position);
        SaveFramework.NewSaveData("TestingRot", (SaveableQuaternion)test.transform.rotation);
        SaveFramework.Save();
    }
}
