using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Saveables;
using Saving;

namespace Saveables
{
    /// <summary>
    /// Class used for all saveable types
    /// </summary>
    public static class KnownTypes
    {
        /// <summary>
        /// list of all types that can be saved
        /// </summary>
        public static List<Type> knownTypes = new()
        {
            typeof(SaveableVector3),
            typeof(SaveableQuaternion),
            typeof(SaveData.Data),
            typeof(string),
            typeof(int),
            typeof(float),
            typeof(bool),
        };
    }

    /* namespace for saveable unity datatypes 
     all saveable types must be added here to save properly
     */

    /// <summary>
    /// struct used to save Vectors
    /// </summary>
    public struct SaveableVector3
    {
        /// <summary>
        /// x component
        /// </summary>
        public float x;

        /// <summary>
        /// y component
        /// </summary>
        public float y;

        /// <summary>
        /// z component
        /// </summary>
        public float z;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rX"></param>
        /// <param name="rY"></param>
        /// <param name="rZ"></param>
        public SaveableVector3(float rX, float rY, float rZ)
        {
            x = rX;
            y = rY;
            z = rZ;
        }

        /// <summary>
        /// Returns a string representation of the object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2}]", x, y, z);
        }

        /// <summary>
        /// Automatic conversion from SaveableVector3 to Vector3
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static implicit operator UnityEngine.Vector3(SaveableVector3 rValue)
        {
            return new UnityEngine.Vector3(rValue.x, rValue.y, rValue.z);
        }

        /// <summary>
        /// Automatic conversion from Vector3 to SaveableVector3
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static implicit operator SaveableVector3(UnityEngine.Vector3 rValue)
        {
            return new SaveableVector3(rValue.x, rValue.y, rValue.z);
        }

        /// <summary>
        /// Automatic conversion from Vector2 to SaveableVector3
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static implicit operator SaveableVector3(UnityEngine.Vector2 rValue)
        {
            return new SaveableVector3(rValue.x, rValue.y, 0);
        }

        /// <summary>
        /// Automatic conversion from SaveableVector3 to Vector2
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static implicit operator UnityEngine.Vector2(SaveableVector3 rValue)
        {
            return new UnityEngine.Vector3(rValue.x, rValue.y, 0);
        }
    }

    /// <summary>
    /// The struct used to save Quats
    /// </summary>
    public struct SaveableQuaternion
    {
        /// <summary>
        /// x component
        /// </summary>
        public float x;

        /// <summary>
        /// y component
        /// </summary>
        public float y;

        /// <summary>
        /// z component
        /// </summary>
        public float z;
        /// <summary>
        /// z component
        /// </summary>
        public float w;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rX"></param>
        /// <param name="rY"></param>
        /// <param name="rZ"></param>
        /// <param name="rW"></param>
        public SaveableQuaternion(float rX, float rY, float rZ, float rW)
        {
            x = rX;
            y = rY;
            z = rZ;
            w = rW;
        }

        /// <summary>
        /// Returns a string representation of the object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2},{3}]", x, y, z, w);
        }

        /// <summary>
        /// Automatic conversion from SaveableVector3 to Vector3
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static implicit operator UnityEngine.Quaternion(SaveableQuaternion rValue)
        {
            return new UnityEngine.Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }

        /// <summary>
        /// Automatic conversion from Vector3 to SaveableVector3
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static implicit operator SaveableQuaternion(UnityEngine.Quaternion rValue)
        {
            return new SaveableQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }
    }


}


/* 
 Anything to be saved MUST be serialazable.
 */
namespace Saving
{
    /// <summary>
    /// class for interacting with game saves
    /// </summary>
    public static class SaveFramework
    {
        private static Save save = null;
        public static Action<Save> OnSaveLoad = (save) => { };
        public static Action OnSave = () => { };

        public static string Directory => UnityEngine.Application.streamingAssetsPath;

        /// <summary>
        /// Saves the current stored data to the disk
        /// </summary>
        public static void Save()
        {
            FileStream file = File.Create(Directory + "/save.data");
            DataContractSerializer bf = new DataContractSerializer(typeof(SaveData), KnownTypes.knownTypes);
            MemoryStream streamer = new MemoryStream();
            bf.WriteObject(file, GetSave().GetSaveData());
            streamer.Seek(0, SeekOrigin.Begin);
            file.Write(streamer.GetBuffer(), 0, streamer.GetBuffer().Length);
            file.Close();
            UnityEngine.Debug.Log("[SaveFramework] Saved Game!");
            OnSave();
        }

        /// <summary>
        /// Loads the Save from the file system
        /// </summary>
        public static void LoadSave()
        {
            string text = File.ReadAllText(Directory + "/save.data");
            SaveData data = null;
            using (Stream stream = new MemoryStream())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;
                DataContractSerializer serializer = new DataContractSerializer(typeof(SaveData), KnownTypes.knownTypes);
                data = serializer.ReadObject(stream) as SaveData;
                stream.Close();
            }

            GetSave().SetSaveData(data);
            UnityEngine.Debug.Log("[SaveFramework] Loaded Save");
        }

        /// <summary>
        /// Initalizes the saving system, also loads the save
        /// </summary>
        public static void Initalize()
        {
            //if no save is set load one
            if (save == null)
                if (File.Exists(Directory + "/save.data"))
                {
                    // save load
                    string text = File.ReadAllText(Directory + "/save.data");
                    SaveData data = null;
                    using (Stream stream = new MemoryStream())
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(text);
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Position = 0;
                        DataContractSerializer serializer = new DataContractSerializer(typeof(SaveData), KnownTypes.knownTypes);
                        data = serializer.ReadObject(stream) as SaveData;
                        stream.Close();
                    }

                    save = new Save();
                    save.SetSaveData(data);
                    OnSaveLoad(save);
                    UnityEngine.Debug.Log("[SaveFramework] Loaded Save");
                }
                else
                {
                    //inital save creation
                    FileStream file = File.Create(Directory + "/save.data");
                    DataContractSerializer bf = new DataContractSerializer(typeof(SaveData), KnownTypes.knownTypes);
                    MemoryStream streamer = new MemoryStream();
                    save = new Save();
                    save.SetSaveData(new SaveData());
                    bf.WriteObject(file, save.GetSaveData());
                    streamer.Seek(0, SeekOrigin.Begin);
                    file.Write(streamer.GetBuffer(), 0, streamer.GetBuffer().Length);
                    file.Close();
                    OnSaveLoad(save);
                    UnityEngine.Debug.Log("[SaveFramework] Loaded & Created New Save");
                }
        }

        /// <summary>
        /// Gets the current save
        /// </summary>
        /// <returns>Save object</returns>
        public static Save GetSave()
        {
            return save;
        }

        /// <summary>
        /// Add new data to the save
        /// </summary>
        /// <param name="name">the name of data</param>
        /// <param name="data">the value of data</param>
        public static void NewSaveData(string name, object data)
        {
            GetSave().GetSaveData().Add(name, data);
        }

        /// <summary>
        /// Ge data from the save
        /// </summary>
        /// <typeparam name="T">the data type</typeparam>
        /// <param name="name">the data name</param>
        /// <returns>T as Data</returns>
        public static T GetSaveData<T>(string name)
        {
            if (SaveDataExists(name))
                return (T)GetSave().GetSaveData().datas.Find(d => d.dataName == name).data;
            else 
                throw new Exception("[SaveFramework] No data found of the name " + name);
        }

        /// <summary>
        /// checks if a data name exists in a dataset
        /// </summary>
        /// <param name="name">data name to check for</param>
        /// <returns>true/false if it exists</returns>
        public static bool SaveDataExists(string name)
        {
            List<SaveData.Data> datas = GetSave().GetSaveData().datas;
            return datas.Contains(datas.Find(d => d.dataName == name));
        }
    }



    public class Save
    {

        private SaveData saveData;
        public void SetSaveData(SaveData saveData)
        {
            this.saveData = saveData;
        }

        public SaveData GetSaveData()
        {
            return saveData;
        }
    }

    [Serializable]
    public class SaveData
    {
        [Serializable]
        public readonly struct Data
        {
            public readonly string dataName;
            public readonly int dataId;
            public readonly object data;
            public Data(string dataName, int dataId, object data)
            {
                this.dataName = dataName;
                this.dataId = dataId;
                this.data = data;
            }
        }

        public List<Data> datas = new();

        public void Add(string name, object data)
        {
            if (datas.Contains(datas.Find(d => d.dataName == name)))
            {
                UnityEngine.Debug.LogWarning("[SaveFramework] Overwriting Save Data: " + name);
                datas[datas.IndexOf(datas.Find(d => d.dataName == name))] = new Data(name, datas.Count, data);
            }
            else
            {
                datas.Add(new Data(name, datas.Count, data));
            }
        }
    }
}
