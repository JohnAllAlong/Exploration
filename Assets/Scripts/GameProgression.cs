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
    [SerializeField] private List<EnemyWander> _enemies;
    [Space]

    //list of levels to load in order (first level is always _loadLevel)
    //level to load (can be overriden by a save loading)
    [SerializeField] private string _firstLevel;
    [SerializeField] private List<string> _levelLoadList;
    [SerializeField] private bool _loadedFromSave;

    public void Awake()
    {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;

        SaveFramework.CreateDefaultSave();
        SaveFramework.LoadDefaultSave();

        //if previous save has data load it
        if (SaveFramework.SaveDataExists("GameProgressionSave"))
        {
            SaveableGameProgression savedGameProgression = SaveFramework.GetSaveData<SaveableGameProgression>("GameProgressionSave");
            _hasCompletedTutorial = savedGameProgression.tutorialCompleted;
            _firstLevel = savedGameProgression.lastSavedLevel;
            _playerLoadPosition = savedGameProgression.lastPlayerPosition;

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
            _loadedFromSave = true;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnApplicationQuit()
    {
        SaveableGameProgression saveGameProgression = new();
        saveGameProgression.tutorialCompleted = _hasCompletedTutorial;
        saveGameProgression.lastSavedLevel = _firstLevel;
        saveGameProgression.lastPlayerPosition = _playerLoadPosition;

        SaveableEnemies[] enemiesToSave = new SaveableEnemies[_enemies.Count - 1];

        for (int i = 0; i < _enemies.Count; i++)
        {
            EnemyWander enemy = _enemies[i];
            SaveableEnemies saveEnemy = new(enemy.enemyId, enemy.transform.position, enemy.isAlive);
            enemiesToSave[i] = saveEnemy;
        }

        saveGameProgression.enemies = enemiesToSave;
        SaveFramework.Save();
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(_firstLevel);
    }

    public void LoadSecondLevel()
    {
        SceneManager.LoadScene(_levelLoadList[0]);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //load enemies
        foreach (GameObject enemyObj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemyWander enemy = enemyObj.GetComponent<EnemyWander>();
            _enemies.Add(enemy);
        }
    }

}
