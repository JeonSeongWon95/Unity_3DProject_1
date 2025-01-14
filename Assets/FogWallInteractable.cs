using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FogWallInteractable : NetworkBehaviour
{
    [Header("Fogs")]
    [SerializeField] GameObject[] FogWallGameObjects;
    public int FogWallID;

    [Header("Active")]
    public NetworkVariable<bool> mNetworkIsActive = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        OnIsActiveChanged(false, mNetworkIsActive.Value);
        mNetworkIsActive.OnValueChanged += OnIsActiveChanged;
        WorldObjectManager.Instance.AddFogWallToList(this);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        mNetworkIsActive.OnValueChanged -= OnIsActiveChanged;
        WorldObjectManager.Instance.RemoveFogWallToList(this);
    }

    private void OnIsActiveChanged(bool Old, bool New) 
    {
        if (mNetworkIsActive.Value)
        {
            foreach (var fogobject in FogWallGameObjects)
            {
                fogobject.SetActive(true);
            }
        }
        else 
        {
            foreach (var fogobject in FogWallGameObjects)
            {
                fogobject.SetActive(false);
            }
        }
    }
}
