using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
