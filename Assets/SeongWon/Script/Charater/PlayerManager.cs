using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerLocomotionManager mPlayerLocomotionManager;
    [HideInInspector] public PlayerAnimatorManager mPlayerAnimatorManager;
    [HideInInspector] public PlayerNetworkManager mPlayerNetworkManager;
    [HideInInspector] public PlayerStatsManager mPlayerStatsManager;

    protected override void Awake()
    {
        base.Awake();
        mPlayerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        mPlayerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        mPlayerNetworkManager = GetComponent<PlayerNetworkManager>();
        mPlayerStatsManager = GetComponent<PlayerStatsManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (!IsOwner)
            return;

        mPlayerLocomotionManager.HandleAllMovement();
        mPlayerStatsManager.RegenerateStamina();
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

            mPlayerNetworkManager.mNetworkCurrentStamina.OnValueChanged += (OldValue, NewValue) =>
            {
                PlayerUIManager.Instance.mPlayerUIHUDManager.SetStaminaValue(OldValue, NewValue);
                mPlayerStatsManager.ResetRegenerateStamina(OldValue, NewValue);
            };

            mPlayerNetworkManager.mNetworkCurrentStamina.Value = mPlayerStatsManager.CalculateStaminaBaseOnEndurance(
                mPlayerNetworkManager.mNetworkEndurence.Value);

            mPlayerNetworkManager.mNetworkMaxStamina.Value = mPlayerStatsManager.CalculateStaminaBaseOnEndurance(
                mPlayerNetworkManager.mNetworkEndurence.Value);

            PlayerUIManager.Instance.mPlayerUIHUDManager.SetMaxStaminaValue(
                mPlayerNetworkManager.mNetworkMaxStamina.Value);

        }
    }

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData NewCharacterData) 
    {
        NewCharacterData.mSceneIndex = SceneManager.GetActiveScene().buildIndex;
        NewCharacterData.mCharacterName = mPlayerNetworkManager.mCharacterName.Value.ToString();
        NewCharacterData.mPositionX = transform.position.x;
        NewCharacterData.mPositionZ = transform.position.z;
        NewCharacterData.mPositionY = transform.position.y;
        
    }

    public void LoadGameDataToCurrentCharacterData(ref CharacterSaveData NewCharacterData) 
    {
        mPlayerNetworkManager.mCharacterName.Value = NewCharacterData.mCharacterName;

        Vector3 NewPosition = new Vector3(NewCharacterData.mPositionX, NewCharacterData.mPositionY,
            NewCharacterData.mPositionZ);

        transform.position = NewPosition;

    }
}
