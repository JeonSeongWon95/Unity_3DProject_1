using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharaterManager : NetworkBehaviour
{
    public CharacterController mCharaterController;
    CharaterNetworkManager mCharaterNetworkManager;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        mCharaterController = GetComponent<CharacterController>();
        mCharaterNetworkManager = GetComponent<CharaterNetworkManager>();
    }

    protected virtual void Update() 
    {
        if (IsOwner)
        {
            mCharaterNetworkManager.mNetworkPosition.Value = transform.position;
            mCharaterNetworkManager.mNetworkRotation.Value = transform.rotation;
        }
        else 
        {
            transform.position = Vector3.SmoothDamp(transform.position, mCharaterNetworkManager.mNetworkPosition.Value,
                ref mCharaterNetworkManager.mNetworkPositionVelocity, mCharaterNetworkManager.mNetworkPositionSmoothTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, mCharaterNetworkManager.mNetworkRotation.Value,
                mCharaterNetworkManager.mNetworkRotateSmoothTime);
        }
    }

    protected virtual void LateUpdate() 
    {

    }
}
