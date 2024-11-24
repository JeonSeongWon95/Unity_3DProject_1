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
        mPlayerLocomotionManager.HandleAllMovement();
    }
}
