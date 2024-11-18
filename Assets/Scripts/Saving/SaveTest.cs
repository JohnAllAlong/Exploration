using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saving;
using Saveables;

public class SaveTest : MonoBehaviour
{
    public GameObject test;

    private void Awake()
    {
        SaveFramework.Initalize();
    }

    void Start()
    {
        if (SaveFramework.SaveDataExists("TestingPos") && SaveFramework.SaveDataExists("TestingRot"))
        {
            Vector3 loadedPos = SaveFramework.GetSaveData<SaveableVector3>("TestingPos");
            Quaternion loadedRot = SaveFramework.GetSaveData<SaveableQuaternion>("TestingRot");
            test.transform.position = loadedPos;
            test.transform.rotation = loadedRot;
        }
    }

    private void OnApplicationQuit()
    {
        SaveFramework.NewSaveData("TestingPos", (SaveableVector3)test.transform.position);
        SaveFramework.NewSaveData("TestingRot", (SaveableQuaternion)test.transform.rotation);
        SaveFramework.Save();
    }
}
