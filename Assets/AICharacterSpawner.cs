using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AICharacterSpawner : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] GameObject CharacterGameObject;
    [SerializeField] GameObject InstantiatedGameObject;

    private void Awake()
    {
    }

    private void Start()
    {
        WorldAIManager.Instance.SpawnCharacter(this);
        gameObject.SetActive(false);
    }

    public void AttemptToSpawn() 
    {
        if (CharacterGameObject != null)
        {
            InstantiatedGameObject = Instantiate(CharacterGameObject);
            InstantiatedGameObject.transform.position = transform.position;
            InstantiatedGameObject.transform.rotation = transform.rotation;
            InstantiatedGameObject.GetComponent<NetworkObject>().Spawn();
        }

    }

}
