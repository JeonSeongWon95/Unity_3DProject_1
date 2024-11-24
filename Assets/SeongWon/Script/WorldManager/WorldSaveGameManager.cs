using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager Instance;

    [SerializeField] int WorldSceneIndex = 1;


    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator LoadNewGame() 
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(WorldSceneIndex);
        yield return null;
    }

    public int GetWorldSceneIndex() 
    {
        return WorldSceneIndex;
    }
}
