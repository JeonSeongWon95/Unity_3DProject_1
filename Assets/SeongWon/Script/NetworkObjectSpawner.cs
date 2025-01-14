using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkObjectSpawner : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] GameObject NetworkGameObject;
    [SerializeField] GameObject InstantiatedGameObject;

    private void Start()
    {
        WorldObjectManager.Instance.SpawnCharacter(this);
        gameObject.SetActive(false);
    }

    public void AttemptToSpawn()
    {
        if (NetworkGameObject != null)
        {
            InstantiatedGameObject = Instantiate(NetworkGameObject);
            InstantiatedGameObject.transform.position = transform.position;
            InstantiatedGameObject.transform.rotation = transform.rotation;
            InstantiatedGameObject.GetComponent<NetworkObject>().Spawn();
        }

    }
}
