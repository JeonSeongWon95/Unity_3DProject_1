using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSessionManager : MonoBehaviour
{
    [Header("Active Players In Session")]
    public List<PlayerManager> Players = new List<PlayerManager>();

    static public GameSessionManager Instance;

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

        DontDestroyOnLoad(gameObject);
    }

    public void AddPlayerToActivePlayersList(PlayerManager playerManager) 
    {
        if (!Players.Contains(playerManager))
        {
            Players.Add(playerManager);
        }

        for (int i = Players.Count; i > -1; i--)
        {
            if (Players[i] == null)
            {
                Players.RemoveAt(i);
            }
        }
    }

    public void RemovePlayerToActivePlayersList(PlayerManager playerManager)
    {
        if (Players.Contains(playerManager))
        {
            Players.Remove(playerManager);
        }

        for (int i = Players.Count; i > -1; i--)
        {
            if (Players[i] == null)
            {
                Players.RemoveAt(i);
            }
        }
    }
}
