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

    protected override void Awake()
    {
        base.Awake();

        mPlayerManager = GetComponent<PlayerManager>();
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
}
