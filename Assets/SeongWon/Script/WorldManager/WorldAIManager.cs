using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class WorldAIManager : MonoBehaviour
{
    static public WorldAIManager Instance;

    [Header("Characters")]
    [SerializeField] List<AICharacterSpawner> mCharacterSpawners;
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

    public void SpawnCharacter(AICharacterSpawner AISpawner) 
    {
        if (NetworkManager.Singleton.IsServer)
        {
            mCharacterSpawners.Add(AISpawner);
            AISpawner.AttemptToSpawn();
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
