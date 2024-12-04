using Saveables;
using Saving;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProgression : MonoBehaviour
{
    [SerializeField] private bool _hasCompletedTutorial;
    //player pos to load for _loadLevel
    [SerializeField] private Vector2 _playerLoadPosition;
    [SerializeField] private List<EnemyChase> _enemies = new();

    [Space]

    //list of levels to load in order (first level is always _loadLevel)
    //level to load (can be overriden by a save loading)
    [SerializeField] private string _firstLevel;

    //list contains levels 2 to infinity
    [SerializeField] private List<string> _levelLoadList;
    [SerializeField] private string _currentLevel;
    public static GameProgression singleton;

    [SerializeField] private bool _loadedFromSave;
    [SerializeField] private bool _loadFromSave;
    [SerializeField] private SaveableGameProgression savedGameProgression;


    public void Awake()
    {
        singleton = this;
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;

        SaveFramework.CreateDefaultSave();
        SaveFramework.LoadDefaultSave();

        //if previous save has data load it
        if (SaveFramework.SaveDataExists("GameProgressionSave"))
        {
            savedGameProgression = SaveFramework.GetSaveData<SaveableGameProgression>("GameProgressionSave");
            _hasCompletedTutorial = savedGameProgression.tutorialCompleted;
            _firstLevel = savedGameProgression.lastSavedLevel;
            _playerLoadPosition = savedGameProgression.lastPlayerPosition;

            _loadedFromSave = true;
            _loadFromSave = true;
        }
    }

    protected void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    protected void OnApplicationQuit()
    {
        savedGameProgression.lastPlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        savedGameProgression.lastSavedLevel = _currentLevel;

        //save enemies pos
        if (_enemies.Count > 0)
        {
            SaveableEnemies[] enemiesToSave = new SaveableEnemies[_enemies.Count];

            for (int i = 0; i < _enemies.Count; i++)
            {
                EnemyChase enemy = _enemies[i];
                SaveableEnemies saveEnemy = new(enemy.enemyId, enemy.transform.position, enemy.isAlive);
                enemiesToSave[i] = saveEnemy;
            }
            savedGameProgression.enemies = enemiesToSave;

        }


        SaveFramework.NewSaveData("GameProgressionSave", savedGameProgression);
        SaveFramework.Save();
    }

    public void LoadFirstLevel()
    {
        _currentLevel = _firstLevel;
        singleton._enemies.Clear();
        SceneManager.LoadScene(_firstLevel);
    }

    public static void LoadSecondLevel()
    {
        singleton._currentLevel = singleton._levelLoadList[0];
        singleton._enemies.Clear();
        SceneManager.LoadScene(singleton._levelLoadList[0]);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //load enemies
        foreach (GameObject enemyObj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemyChase enemy = enemyObj.GetComponent<EnemyChase>();
            _enemies.Add(enemy);
        }

        //load enemies and player pos into game
        if (GameObject.FindGameObjectWithTag("Player") != null && _loadFromSave)
        {
            _loadFromSave = false;
            GameObject.FindGameObjectWithTag("Player").transform.position = _playerLoadPosition;

            //load enemy pos
            if (savedGameProgression.enemies != null)
            {
                foreach (var enemy in savedGameProgression.enemies)
                {
                    for (int i = 0; i != _enemies.Count; i++)
                    {
                        if (_enemies[i].enemyId == enemy.enemyId)
                        {
                            _enemies[i].transform.position = enemy.enemyPosition;
                            _enemies[i].isAlive = enemy.isAlive;
                            if (!enemy.isAlive) _enemies[i].gameObject.SetActive(false);
                            break;
                        }
                    }
                }
            }
        }

    }
}
