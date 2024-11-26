using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharaterManager
{
    PlayerLocomotionManager mPlayerLocomotionManager;
    protected override void Awake()
    {
        base.Awake();
        mPlayerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (!IsOwner)
            return;

        mPlayerLocomotionManager.HandleAllMovement();
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
        }
    }
}
