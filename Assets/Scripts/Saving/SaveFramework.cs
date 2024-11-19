/* 
    Aiden C. Desjarlais
     Saving Framework

A framework that makes saving for basic games easy.
Uses XML to save to disk.
 
 */

using System;
using System.Collections.Generic;
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
 Only ONE save can be accesible at a time.
 */
namespace Saving
{
    /// <summary>
    /// class for interacting with game saves <br></br>
    /// only ONE save can be loaded at a time.
    ///<br></br><br></br>
    /// if you need to interact with 2 saves, use GetRegisteredSaves()
    /// </summary>
    public static class SaveFramework
    {
        private static Save loadedSave = null;
        private static List<Save> registeredSaves = new();

        /// <summary>
        /// action ran when a save has been loaded
        /// </summary>
        public static Action<Save> OnSaveLoad = (save) => { };
        /// <summary>
        /// action ran when data has been saved
        /// </summary>
        public static Action OnSave = () => { };

        //default save is "save.data"
        private static string saveName = "save";
        private static string saveFileExtension = "data";

        // disgregards any file with any file extension defined below
        private static readonly List<string> ignoreFileExtensions = new() {
        "meta",
        "cs"
        };

        /// <summary>
        /// The directory where the save is located
        /// </summary>
        public static string SaveDirectory => UnityEngine.Application.streamingAssetsPath;

        /// <summary>
        /// the path of the currently loaded save
        /// </summary>
        public static string Path => SaveDirectory + "/" + saveName + "." + saveFileExtension;

        /// <summary>
        /// the path of the default save
        /// </summary>
        public static string DefaultSavePath => SaveDirectory + "/save.data";

        /// <summary>
        /// Saves the current data to its path on the disk
        /// </summary>
        public static void Save()
        {
            FileStream file = File.Create(Path);
            DataContractSerializer bf = new DataContractSerializer(typeof(SaveData), KnownTypes.knownTypes);
            MemoryStream streamer = new MemoryStream();
            bf.WriteObject(file, GetCurrentSave().GetSaveData());
            streamer.Seek(0, SeekOrigin.Begin);
            file.Write(streamer.GetBuffer(), 0, streamer.GetBuffer().Length);
            file.Close();
            UnityEngine.Debug.Log("[SaveFramework] Saved game to " + Path);
            OnSave();
        }

        /// <summary>
        /// Loads a save from the file system
        /// </summary>
        public static void LoadSave(string fileName, string fileExtension)
        {

            if (!File.Exists(SaveDirectory + "/" + fileName + "." + fileExtension))
            {
                UnityEngine.Debug.Log("[SaveFramework@LoadSave] Unable to locate the requested save at " + SaveDirectory + "/" + fileName + "." + fileExtension);
                return;
            }

            saveName = fileName;
            saveFileExtension = fileExtension;
            string text = File.ReadAllText(SaveDirectory + "/" + fileName + "." + fileExtension);
            SaveData data = null;
            using (Stream stream = new MemoryStream())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;
                DataContractSerializer serializer = new DataContractSerializer(typeof(SaveData), KnownTypes.knownTypes);
                data = serializer.ReadObject(stream) as SaveData;
                stream.Close();

                data.path = SaveDirectory + "/" + fileName + "." + fileExtension;
                loadedSave = new Save();
                loadedSave.SetSaveData(data);
                OnSaveLoad(loadedSave);
            }

            GetCurrentSave().SetSaveData(data);
            UnityEngine.Debug.Log("[SaveFramework] Loaded Save /" + fileName + "." + fileExtension);
        }

        /// <summary>
        /// Loads default save from the file system (save.data)
        /// </summary>
        public static void LoadDefaultSave()
        {

            if (!File.Exists(DefaultSavePath))
            {
                UnityEngine.Debug.Log("[SaveFramework@LoadSave] Unable to locate the requested save at " + DefaultSavePath);
                return;
            }

            string text = File.ReadAllText(DefaultSavePath);
            SaveData data = null;
            using (Stream stream = new MemoryStream())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;
                DataContractSerializer serializer = new DataContractSerializer(typeof(SaveData), KnownTypes.knownTypes);
                data = serializer.ReadObject(stream) as SaveData;
                stream.Close();

                data.path = DefaultSavePath;
                loadedSave = new Save();
                loadedSave.SetSaveData(data);
                OnSaveLoad(loadedSave);
            }

            GetCurrentSave().SetSaveData(data);
            UnityEngine.Debug.Log("[SaveFramework] Loaded Default Save");
        }

        /// <summary>
        /// creates a new save with name and extension
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileExtension"></param>
        public static void CreateNewSave(string fileName, string fileExtension)
        {
            if (!File.Exists(SaveDirectory + "/" + fileName + "." + fileExtension))  //if no save exists create it
            {
                //create a new save if one doesnt exist
                FileStream file = File.Create(SaveDirectory + "/" + fileName + "." + fileExtension);
                DataContractSerializer bf = new DataContractSerializer(typeof(SaveData), KnownTypes.knownTypes);
                MemoryStream streamer = new MemoryStream();

                Save save = new Save();
                SaveData saveData = new SaveData();
                saveData.path = SaveDirectory + "/" + fileName + "." + fileExtension;
                save.SetSaveData(saveData);

                bf.WriteObject(file, save.GetSaveData());
                streamer.Seek(0, SeekOrigin.Begin);
                file.Write(streamer.GetBuffer(), 0, streamer.GetBuffer().Length);
                file.Close();
                UnityEngine.Debug.Log("[SaveFramework] Created New Save /" + fileName + "." + fileExtension);
            }
        }

        /// <summary>
        /// creates the default save (save.data)
        /// </summary>
        public static void CreateDefaultSave()
        {
            if (!File.Exists(DefaultSavePath))  //if no default save exists create it
            {
                FileStream file = File.Create(DefaultSavePath);
                DataContractSerializer bf = new DataContractSerializer(typeof(SaveData), KnownTypes.knownTypes);
                MemoryStream streamer = new MemoryStream();

                Save save = new Save();
                SaveData saveData = new SaveData();
                saveData.path = DefaultSavePath;
                save.SetSaveData(saveData);

                bf.WriteObject(file, save.GetSaveData());
                streamer.Seek(0, SeekOrigin.Begin);
                file.Write(streamer.GetBuffer(), 0, streamer.GetBuffer().Length);
                file.Close();
                UnityEngine.Debug.Log("[SaveFramework] Created Default Save");
            }
        }

        /// <summary>
        /// Registers all files (saves) to the framework.<br></br>
        /// Has no effect on any framework calls or functions.<br></br>
        /// Only loads the unregistered files into an array for use outside of the framework<br></br>
        /// doesnt load anything if already loaded.
        /// <br></br>
        /// <br></br>
        /// Only run after a save has been loaded
        /// </summary>
        public static void RegisterAllSaves()
        {
            if (registeredSaves.Count != 0) return;

            //register all unloaded saves
            foreach (string file in Directory.EnumerateFiles(SaveDirectory))
            {
                if (!HasExcludedExtension(file))
                {
                    string text = File.ReadAllText(file);
                    SaveData data;
                    using (Stream stream = new MemoryStream())
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(text);
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Position = 0;
                        DataContractSerializer serializer = new DataContractSerializer(typeof(SaveData), KnownTypes.knownTypes);
                        data = serializer.ReadObject(stream) as SaveData;
                        stream.Close();
                    }

                    Save unregisteredSave = new Save();
                    unregisteredSave.SetSaveData(data);

                    //check to make sure save isint currently loaded
                    if (data.path != loadedSave.GetSaveData().path) {
                        registeredSaves.Add(unregisteredSave);
                        UnityEngine.Debug.Log("[SaveFramework] Registered file " + file);
                    }
                }
            }
            UnityEngine.Debug.Log("[SaveFramework] Registered all saves");
        }

        /// <summary>
        /// Gets all the raw saves in the save directory, NOT including the currently loaded save
        /// </summary>
        /// <returns>a list of Saves that have been registered</returns>
        public static List<Save> GetRegisteredSaves()
        {
            if (registeredSaves.Count == 0)
            {
                UnityEngine.Debug.Log("[SaveFramework] Please run GetRegisteredSaves(), no saves have been registered");
                return default;
            }
            return registeredSaves;
        }

        /// <summary>
        /// Trys to get dataName from a list of registered saves. <br></br>
        /// Returns the first instance of data from a save that has this name.<br></br>
        /// Not recommended if more than one save has this data name.<br></br>
        /// You cannot modify registered saves, only loaded.
        /// </summary>
        /// <returns>T as data in the found save</returns>
        public static T TryGetRegisteredSaveData<T>(string dataName)
        {
            if (registeredSaves.Count == 0)
            {
                UnityEngine.Debug.Log("[SaveFramework] Please run GetRegisteredSaves(), no saves have been registered");
                return default;
            }

            for (int i = 0; i != registeredSaves.Count; i++)
            {
                List<SaveData.Data> datas = registeredSaves[i].GetSaveData().datas;

                if (datas.Contains(datas.Find(d => d.dataName == dataName)))
                    return (T)registeredSaves[i].GetSaveData().datas.Find(d => d.dataName == dataName).data;
            }

            return default;
        }

        /// <summary>
        /// Checks if a filepath has an excluded exntesion
        /// </summary>
        /// <param name="filePath">filepath to check</param>
        /// <returns>true/false if the filepath contains any ignored file extension</returns>
        public static bool HasExcludedExtension(string filePath)
        {
            foreach (string ext in ignoreFileExtensions)
            {
                if (filePath.Contains("." + ext)) return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the current save
        /// </summary>
        /// <returns>Save object</returns>
        public static Save GetCurrentSave()
        {
            return loadedSave;
        }

        /// <summary>
        /// Add new data to the save
        /// </summary>
        /// <param name="name">the name of data</param>
        /// <param name="data">the value of data</param>
        public static void NewSaveData(string name, object data)
        {
            GetCurrentSave().GetSaveData().Add(name, data);
        }

        /// <summary>
        /// removes data from the save
        /// </summary>
        /// <param name="name">the name of the data to remove</param>
        public static void DestroySaveData(string name)
        {
            GetCurrentSave().GetSaveData().Delete(name);
        }

        /// <summary>
        /// removes ALL data from the save
        /// </summary>
        public static void DestroySaveData()
        {
            GetCurrentSave().GetSaveData().WipeData();
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
                return (T)GetCurrentSave().GetSaveData().datas.Find(d => d.dataName == name).data;
            else
                throw new Exception("[SaveFramework] No data found of the name " + name);
        }

        /// <summary>
        /// checks if a data name exists in the current save
        /// </summary>
        /// <param name="name">data name to check for</param>
        /// <returns>true/false if it exists</returns>
        public static bool SaveDataExists(string name)
        {
            List<SaveData.Data> datas = GetCurrentSave().GetSaveData().datas;
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
            /// <summary>
            /// the name of this data
            /// </summary>
            public readonly string dataName;
            /// <summary>
            /// the id of this data (also its index the dataset)
            /// </summary>
            public readonly int dataId;
            /// <summary>
            /// the data of this Data object
            /// </summary>
            public readonly object data;
            public Data(string dataName, int dataId, object data)
            {
                this.dataName = dataName;
                this.dataId = dataId;
                this.data = data;
            }
        }

        public List<Data> datas = new();
        public string path;

        public void Add(string name, object data)
        {
            if (datas.Contains(datas.Find(d => d.dataName == name)))
            {
                UnityEngine.Debug.LogWarning("[SaveFramework] Overwriting Save Data: " + name);
                datas[datas.IndexOf(datas.Find(d => d.dataName == name))] = new Data(name, datas.Count, data);
            }
            else
            {
                datas.Add(new Data(name, datas.Count - 1, data));
            }
        }

        public void Delete(string name)
        {
            datas.RemoveAt(datas.Find(d => d.dataName == name).dataId);
        }

        public void WipeData()
        {
            datas.Clear();
        }
    }
}