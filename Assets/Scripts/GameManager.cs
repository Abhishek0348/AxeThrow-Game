using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public GameObject[] levelPrefabs;
    public Transform levelParent;
    public int currentLevelIndex = 0;

    private GameObject currentLevel;
    private int totalEnemies;
    private int remainingAxes;

    public GameObject axePrefab;
    private Transform axeSpawnPoint;

    [HideInInspector] public GameObject currentAxe;
    private List<GameObject> spawnedAxes = new List<GameObject>();

    public int maxAxes = 3;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadLevel(currentLevelIndex);
    }

    public void SpawnAxe()
    {
        Debug.Log("Spawning Axe");
        currentAxe = Instantiate(axePrefab, axeSpawnPoint.position, Quaternion.Euler(0f,0f,-90f), axeSpawnPoint);
        spawnedAxes.Add(currentAxe);
    }

    public void LoadLevel(int index)
    {
        foreach (GameObject axe in spawnedAxes)
        {
            if (axe != null)
                Destroy(axe);
        }
        spawnedAxes.Clear();

        if (currentLevel != null)
            Destroy(currentLevel);
        
        currentLevel = Instantiate(levelPrefabs[index], levelParent);

        axeSpawnPoint = currentLevel.transform.Find("Player/AxeSpawnPoint");
        if (axeSpawnPoint == null)
        {
            Debug.LogError("AxeSpawnPoint not found in level");
            return;
        }

        totalEnemies = currentLevel.GetComponentsInChildren<Transform>()
            .Count(t => t.CompareTag("Enemy"));

        remainingAxes = maxAxes;
        UIManager.Instance.UpdateAxeCount(remainingAxes);
        SpawnAxe();
    }



    public void OnEnemyHit()
    {
        totalEnemies--;
        if (totalEnemies <= 0)
        {
            StartCoroutine(NextLevel());
        }
    }

    public void OnAxeUsed()
    {
        remainingAxes--;
        UIManager.Instance.UpdateAxeCount(remainingAxes);
        if (remainingAxes > 0 )
        {
            SpawnAxe(); 
        }
        else if (remainingAxes <= 0 )
        {
            ReloadLevel(); 
        }
    }


    public void ReloadLevel()
    {
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(0.8f);
        LoadLevel(currentLevelIndex);
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(1.5f);
        currentLevelIndex++;
        if (currentLevelIndex < levelPrefabs.Length)
            LoadLevel(currentLevelIndex);
        else
            Debug.Log("Game Complete!");
    }
}
