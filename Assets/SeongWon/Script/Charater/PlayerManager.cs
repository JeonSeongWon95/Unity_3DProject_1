using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : CharacterManager
{
    [Header("Debug Menu")]
    [SerializeField] bool mRespawnCharacter = false;
    [SerializeField] bool mSwitchRightWeapon = false;

    [HideInInspector] public PlayerLocomotionManager mPlayerLocomotionManager;
    [HideInInspector] public PlayerAnimatorManager mPlayerAnimatorManager;
    [HideInInspector] public PlayerNetworkManager mPlayerNetworkManager;
    [HideInInspector] public PlayerStatsManager mPlayerStatsManager;
    [HideInInspector] public PlayerInventoryManager mPlayerInventoryManager;
    [HideInInspector] public PlayerEquipmentManager mPlayerEquipmentManager;

    protected override void Awake()
    {
        base.Awake();
        mPlayerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        mPlayerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        mPlayerNetworkManager = GetComponent<PlayerNetworkManager>();
        mPlayerStatsManager = GetComponent<PlayerStatsManager>();
        mPlayerInventoryManager = GetComponent<PlayerInventoryManager>();
        mPlayerEquipmentManager = GetComponent<PlayerEquipmentManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (!IsOwner)
            return;

        mPlayerLocomotionManager.HandleAllMovement();
        mPlayerStatsManager.RegenerateStamina();
        DebugMenu();
    }

    protected override void LateUpdate()
    {
        if(!IsOwner) 
            return;

        base.LateUpdate();

        PlayerCamera.Instance.HandleAllCameraActions();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner) 
        {
            PlayerCamera.Instance.mPlayerManager = this;
            PlayerInputManager.Instance.mPlayerManager = this;
            WorldSaveGameManager.Instance.mPlayerManager = this;

            mPlayerNetworkManager.mNetworkVitality.OnValueChanged += mPlayerNetworkManager.SetHealthValue;
            mPlayerNetworkManager.mNetworkEndurence.OnValueChanged += mPlayerNetworkManager.SetStaminaValue;

            mPlayerNetworkManager.mNetworkCurrentStamina.OnValueChanged += (OldValue, NewValue) =>
            {
                PlayerUIManager.Instance.mPlayerUIHUDManager.SetStaminaValue(OldValue, NewValue);
                mPlayerStatsManager.ResetRegenerateStamina(OldValue, NewValue);
            };
            mPlayerNetworkManager.mNetworkCurrentHealth.OnValueChanged += (OldValue, NewValue) =>
            {
                PlayerUIManager.Instance.mPlayerUIHUDManager.SetHealthValue(OldValue, NewValue);
                //mPlayerStatsManager.ResetRegenerateHealth(OldValue, NewValue);
            };
        }

        mPlayerNetworkManager.mNetworkCurrentHealth.OnValueChanged += mPlayerNetworkManager.CheckHP;

        mPlayerNetworkManager.mCurrentRightHandWeaponID.OnValueChanged += 
            mPlayerNetworkManager.OnCurrentRightHandWeaponIDChange;

        mPlayerNetworkManager.mCurrentLeftHandWeaponID.OnValueChanged +=
            mPlayerNetworkManager.OnCurrentLeftHandWeaponIDChange;

        if (IsOwner && !IsServer) 
        {
            LoadGameDataToCurrentCharacterData(ref WorldSaveGameManager.Instance.mCurrentCharacterData);
        }

    }

    public override IEnumerator ProcessDeathEvent(bool ManuallySelectDeathAnimation = false)
    {
        if (IsOwner)
        {
            PlayerUIManager.Instance.mPlayerUIPopUpManager.SendYouDiedPopUp();
        }
        return base.ProcessDeathEvent(ManuallySelectDeathAnimation);
    }

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData NewCharacterData) 
    {
        NewCharacterData.mSceneIndex = SceneManager.GetActiveScene().buildIndex;
        NewCharacterData.mCharacterName = mPlayerNetworkManager.mCharacterName.Value.ToString();
        NewCharacterData.mPositionX = transform.position.x;
        NewCharacterData.mPositionZ = transform.position.z;
        NewCharacterData.mPositionY = transform.position.y;

        NewCharacterData.mVitality = mPlayerNetworkManager.mNetworkVitality.Value;
        NewCharacterData.mEndurace = mPlayerNetworkManager.mNetworkEndurence.Value;
        NewCharacterData.mCurrentHealth = mPlayerNetworkManager.mNetworkCurrentHealth.Value;
        NewCharacterData.mCurrentStamina = mPlayerNetworkManager.mNetworkCurrentStamina.Value;


    }

    public void LoadGameDataToCurrentCharacterData(ref CharacterSaveData NewCharacterData) 
    {
        mPlayerNetworkManager.mCharacterName.Value = NewCharacterData.mCharacterName;

        Vector3 NewPosition = new Vector3(NewCharacterData.mPositionX, NewCharacterData.mPositionY,
            NewCharacterData.mPositionZ);

        transform.position = NewPosition;

        mPlayerNetworkManager.mNetworkVitality.Value = NewCharacterData.mVitality;
        mPlayerNetworkManager.mNetworkEndurence.Value = NewCharacterData.mEndurace;

        mPlayerNetworkManager.mNetworkCurrentStamina.Value = mPlayerStatsManager.CalculateHealthBaseOnVitality(
            NewCharacterData.mVitality);

        mPlayerNetworkManager.mNetworkCurrentHealth.Value = NewCharacterData.mCurrentHealth;
        mPlayerNetworkManager.mNetworkCurrentStamina.Value = NewCharacterData.mCurrentStamina;

        mPlayerNetworkManager.mNetworkMaxStamina.Value = mPlayerStatsManager.CalculateStaminaBaseOnEndurance(
            NewCharacterData.mEndurace);

        PlayerUIManager.Instance.mPlayerUIHUDManager.SetMaxStaminaValue(
            mPlayerNetworkManager.mNetworkMaxStamina.Value);

    }

    public override void ReviveCharacter()
    {
        base.ReviveCharacter();

        if (IsOwner) 
        {
            mPlayerNetworkManager.mNetworkCurrentHealth.Value = mPlayerNetworkManager.mNetworkMaxHealth.Value;
            mPlayerNetworkManager.mNetworkCurrentStamina.Value = mPlayerNetworkManager.mNetworkMaxStamina.Value;

            mPlayerAnimatorManager.PlayTargetActionAnimation("Empty", false);
        }
    }

    private void DebugMenu() 
    {
        if (mRespawnCharacter) 
        {
            mRespawnCharacter = false;
            ReviveCharacter();
        }

        if (mSwitchRightWeapon) 
        {
            mSwitchRightWeapon = false;
            mPlayerEquipmentManager.SwitchRightWeapon();
        }
    }
}
