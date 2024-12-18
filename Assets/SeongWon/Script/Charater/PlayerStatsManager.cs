using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    PlayerManager mPlayerManager;

    protected override void Awake()
    {
        base.Awake();
        mPlayerManager = GetComponent<PlayerManager>();
    }

    protected override void Start()
    {
        base.Start();

        CalculateHealthBaseOnVitality(mPlayerManager.mPlayerNetworkManager.mNetworkVitality.Value);
        CalculateStaminaBaseOnEndurance(mPlayerManager.mPlayerNetworkManager.mNetworkEndurence.Value);
    }

}
