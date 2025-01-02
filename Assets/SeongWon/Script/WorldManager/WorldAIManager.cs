using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class WorldAIManager : MonoBehaviour
{
    static public WorldAIManager Instance;

    [Header("Characters")]
    [SerializeField] GameObject[] mAICharacters;
    [SerializeField] List<GameObject> mSpawnedCharacters;

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
        if (NetworkManager.Singleton.IsServer) 
        {
            StartCoroutine(WaitForSceneToLoadThenSpawnCharacters());
        }
    }

    private void Update()
    {
        
    }

    private IEnumerator WaitForSceneToLoadThenSpawnCharacters() 
    {
        while (!SceneManager.GetActiveScene().isLoaded) 
        {
            yield return null;
        }

        SpawnAllCharacters();
    }

    private void SpawnAllCharacters() 
    {
        foreach (var character in mAICharacters)
        {
            GameObject InstantiatedCharacter = Instantiate(character);
            InstantiatedCharacter.GetComponent<NetworkObject>().Spawn();
            mSpawnedCharacters.Add(InstantiatedCharacter);
        }
    }

    private void DespawnAllCharacters()
    {
        foreach (var character in mSpawnedCharacters)
        {
            character.GetComponent<NetworkObject>().Despawn();
        }
    }

    private void DisableAllCharacters() 
    {

    }
}
