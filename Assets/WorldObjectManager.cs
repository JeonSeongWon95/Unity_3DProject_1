using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WorldObjectManager : MonoBehaviour
{
    static public WorldObjectManager Instance;

    [Header("Network Objects")]
    [SerializeField] List<NetworkObjectSpawner> mObjectSpawners;
    [SerializeField] List<GameObject> mSpawnedObjects;

    [Header("Fog Walls")]
    public List<FogWallInteractable> mFogWallInteractables;

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

    public void SpawnCharacter(NetworkObjectSpawner ObjectSpawner)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            mObjectSpawners.Add(ObjectSpawner);
            ObjectSpawner.AttemptToSpawn();
        }
    }

    public void AddFogWallToList(FogWallInteractable NewFogWall) 
    {
        if (!mFogWallInteractables.Contains(NewFogWall)) 
        {
            mFogWallInteractables.Add(NewFogWall);
        }
    }

    public void RemoveFogWallToList(FogWallInteractable NewFogWall)
    {
        if (mFogWallInteractables.Contains(NewFogWall))
        {
            mFogWallInteractables.Remove(NewFogWall);
        }
    }
}
