using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using JetBrains.Annotations;

public class PlayerNetworkManager : CharacterNetworkManager
{
    PlayerManager mPlayerManager;

    public NetworkVariable<FixedString64Bytes> mCharacterName = new NetworkVariable<FixedString64Bytes>("Character",
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Equipment")]
    public NetworkVariable<int> mCurrentRightHandWeaponID = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> mCurrentLeftHandWeaponID = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> mCurrentWeaponBeingUsed = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<bool> mIsUsingRightHand = new NetworkVariable<bool>(false,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<bool> mIsUsingLeftHand = new NetworkVariable<bool>(false,
     NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected override void Awake()
    {
        base.Awake();

        mPlayerManager = GetComponent<PlayerManager>();
    }

    public void SetCharacterActionHand(bool NewIsUsingRightHand)
    {
        if (NewIsUsingRightHand)
        {
            mIsUsingRightHand.Value = true;
            mIsUsingLeftHand.Value = false;
        }
        else
        {
            mIsUsingRightHand.Value = false;
            mIsUsingLeftHand.Value = true;
        }
    }

    public void SetHealthValue(int OldVitality, int NewVitality)
    {
        mNetworkMaxHealth.Value = mPlayerManager.mPlayerStatsManager.CalculateHealthBaseOnVitality(NewVitality);
        PlayerUIManager.Instance.mPlayerUIHUDManager.SetMaxHealthValue(mNetworkMaxHealth.Value);
        mNetworkCurrentHealth.Value = mNetworkMaxHealth.Value;
    }

    public void SetStaminaValue(int OldEndurance, int NewEndurance)
    {
        mNetworkMaxStamina.Value = mPlayerManager.mPlayerStatsManager.CalculateStaminaBaseOnEndurance(NewEndurance);
        PlayerUIManager.Instance.mPlayerUIHUDManager.SetMaxStaminaValue(mNetworkMaxStamina.Value);
        mNetworkCurrentStamina.Value = mNetworkMaxStamina.Value;
    }

    public void OnCurrentRightHandWeaponIDChange(int OldID, int NewID)
    {
        WeaponItem NewWeapon = Instantiate(WorldItemDataBase.Instance.GetWeaponByID(NewID));
        mPlayerManager.mPlayerInventoryManager.mCurrentRightWeapon = NewWeapon;
        mPlayerManager.mPlayerEquipmentManager.LoadRightWeapon();
    }

    public void OnCurrentLeftHandWeaponIDChange(int OldID, int NewID)
    {
        WeaponItem NewWeapon = Instantiate(WorldItemDataBase.Instance.GetWeaponByID(NewID));
        mPlayerManager.mPlayerInventoryManager.mCurrentLeftWeapon = NewWeapon;
        mPlayerManager.mPlayerEquipmentManager.LoadLeftWeapon();
    }

    public void OnCurrentWeaponBeingUsedIDChange(int OldID, int NewID)
    {
        WeaponItem NewWeapon = Instantiate(WorldItemDataBase.Instance.GetWeaponByID(NewID));
        mPlayerManager.mPlayerCombatManager.mCurrentWeaponBeingUsed = NewWeapon;
        mPlayerManager.mPlayerEquipmentManager.LoadLeftWeapon();
    }

    [ServerRpc]
    public void NotifyTheServerOfWeaponActionServerRPC(ulong ClientID, int ActionID, int WeaponID) 
    {
        if (IsServer) 
        {
            NotifyTheServerOfWeaponActionClientRPC(ClientID, ActionID, WeaponID);
        }
    }

    [ClientRpc]
    private void NotifyTheServerOfWeaponActionClientRPC(ulong ClientID, int ActionID, int WeaponID) 
    {
        if (ClientID != NetworkManager.Singleton.LocalClientId) 
        {
            PlayWeaponAction(ActionID, WeaponID);
        }
    }

    private void PlayWeaponAction(int ActionID, int WeaponID) 
    {
        WeaponItemAction mWeaponItemAction = WorldActionManager.Instance.GetWeaponItemActionByID(WeaponID);

        if (mWeaponItemAction != null)
        {
            mWeaponItemAction.AttemptToPerformAction(mPlayerManager, WorldItemDataBase.Instance.GetWeaponByID(WeaponID));
        }
        else 
        {

        }
    }


}
