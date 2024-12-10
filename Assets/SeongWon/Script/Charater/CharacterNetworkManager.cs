using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterNetworkManager : NetworkBehaviour
{
    CharacterManager mCharacterManager;

    [Header("Position")]
    public NetworkVariable<Vector3> mNetworkPosition = new NetworkVariable<Vector3>(Vector3.zero, 
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<Quaternion> mNetworkRotation = new NetworkVariable<Quaternion>(Quaternion.identity,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public Vector3 mNetworkPositionVelocity;
    public float mNetworkPositionSmoothTime = 0.1f;
    public float mNetworkRotateSmoothTime = 0.1f;

    [Header("Animator")]
    public NetworkVariable<float> mNetworkAnimatorHorizontalParameter = new NetworkVariable<float>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> mNetworkAnimatorVerticalParameter = new NetworkVariable<float>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> mNetworkAnimatorMoveAmountParameter = new NetworkVariable<float>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Flags")]
    public NetworkVariable<bool> mNetworkIsSprint = new NetworkVariable<bool>(false,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Status")]
    public NetworkVariable<int> mNetworkEndurence = new NetworkVariable<int>(1,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> mNetworkCurrentStamina = new NetworkVariable<float>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> mNetworkMaxStamina = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected virtual void Awake() 
    {
        mCharacterManager = GetComponent<CharacterManager>();
    }

    [ServerRpc]
    public void PlayTargetActionAnimationServerRpc(ulong ClinetID, string AnimationName, bool ApplyRootMotion)
    {
        if (IsServer)
        {
            PlayTargetActionAnimationClientRpc(ClinetID, AnimationName, ApplyRootMotion);
        }
    }

    [ClientRpc]
    public void PlayTargetActionAnimationClientRpc(ulong ClinetID, string AnimationName, bool ApplyRootMotion) 
    {
        if (ClinetID != NetworkManager.Singleton.LocalClientId) 
        {
            PerformActionAnimationFromServer(AnimationName, ApplyRootMotion);
        }
    }

    private void PerformActionAnimationFromServer(string AnimationName, bool ApplyRootMotion) 
    {
        mCharacterManager.mAnimator.CrossFade(AnimationName, 0.2f);
        mCharacterManager.ApplyRootMotion = ApplyRootMotion;
    }
}
