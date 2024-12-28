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

    [Header("Target")]
    public NetworkVariable<ulong> mCurrentTargetNetworkObjectID = new NetworkVariable<ulong>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Flags")]
    public NetworkVariable<bool> mNetworkIsSprint = new NetworkVariable<bool>(false,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> mNetworkIsJumping = new NetworkVariable<bool>(false,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> mNetworkIsLockOn = new NetworkVariable<bool>(false,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Status")]
    public NetworkVariable<int> mNetworkEndurence = new NetworkVariable<int>(1,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> mNetworkVitality = new NetworkVariable<int>(1,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Resources")]
    public NetworkVariable<float> mNetworkCurrentHealth = new NetworkVariable<float>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> mNetworkMaxHealth = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> mNetworkCurrentStamina = new NetworkVariable<float>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> mNetworkMaxStamina = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected virtual void Awake() 
    {
        mCharacterManager = GetComponent<CharacterManager>();
    }

    public void CheckHP(float OldValue, float NewValue) 
    {
        if (mNetworkCurrentHealth.Value <= 0) 
        {
            StartCoroutine(mCharacterManager.ProcessDeathEvent());
        }

        if (mCharacterManager.IsOwner) 
        {
            if (mNetworkCurrentHealth.Value > mNetworkMaxHealth.Value) 
            {
                mNetworkCurrentHealth.Value = mNetworkMaxHealth.Value;
            }
        }
    }

    public void LockOnTargetIDChange(ulong OldID, ulong NewID) 
    {
        if (!IsOwner) 
        {
            mCharacterManager.mCharacterCombatManager.mCurrentTarget = 
                NetworkManager.Singleton.SpawnManager.SpawnedObjects[NewID].gameObject.GetComponent<CharacterManager>();
        }
    }

    public void OnIsLockedOnChanged(bool Old, bool New) 
    {
        if (!New) 
        {
            mCharacterManager.mCharacterCombatManager.mCurrentTarget = null;
        }
    }

    [ServerRpc]
    public void PlayTargetActionAnimationServerRpc(ulong ClientID, string AnimationName, bool ApplyRootMotion)
    {
        if (IsServer)
        {
            PlayTargetActionAnimationClientRpc(ClientID, AnimationName, ApplyRootMotion);
        }
    }

    [ServerRpc]
    public void PlayTargetAttackActionAnimationServerRpc(ulong ClientID, string AnimationName, bool ApplyRootMotion)
    {
        if (IsServer)
        {
            PlayTargetAttackActionAnimationClientRpc(ClientID, AnimationName, ApplyRootMotion);
        }
    }

    [ClientRpc]
    public void PlayTargetActionAnimationClientRpc(ulong ClientID, string AnimationName, bool ApplyRootMotion) 
    {
        if (ClientID != NetworkManager.Singleton.LocalClientId) 
        {
            PerformActionAnimationFromServer(AnimationName, ApplyRootMotion);
        }
    }

    [ClientRpc]
    public void PlayTargetAttackActionAnimationClientRpc(ulong ClientID, string AnimationName, bool ApplyRootMotion)
    {
        if (ClientID != NetworkManager.Singleton.LocalClientId)
        {
            PerformAttackActionAnimationFromServer(AnimationName, ApplyRootMotion);
        }
    }

    private void PerformActionAnimationFromServer(string AnimationName, bool ApplyRootMotion) 
    {
        mCharacterManager.mAnimator.CrossFade(AnimationName, 0.2f);
        mCharacterManager.ApplyRootMotion = ApplyRootMotion;
    }

    private void PerformAttackActionAnimationFromServer(string AnimationName, bool ApplyRootMotion)
    {
        mCharacterManager.mAnimator.CrossFade(AnimationName, 0.2f);
        mCharacterManager.ApplyRootMotion = ApplyRootMotion;
    }

    [ServerRpc(RequireOwnership = false)]
    public void NotifyTheServerOfCharacterDamageServerRPC(ulong DamagedCharacter, ulong CusingDamageCharacter, float PhysicsDamage, 
        float MagicDamage, float FireDamage, float HolyDamage, float PoiseDamage, float AngleHitFrom,
        float ContactPointX, float ContactPointY, float ContactPointZ) 
    {
        if (IsServer)
        {
            NotifyTheServerOfCharacterDamageClientRPC(DamagedCharacter, CusingDamageCharacter, PhysicsDamage, MagicDamage, FireDamage, HolyDamage,
                PoiseDamage, AngleHitFrom, ContactPointX, ContactPointY, ContactPointZ);
        }
    }

    [ClientRpc]
    public void NotifyTheServerOfCharacterDamageClientRPC(ulong DamagedCharacter, ulong CusingDamageCharacter, 
        float PhysicsDamage, float MagicDamage, float FireDamage, float HolyDamage, float PoiseDamage,
        float AngleHitFrom, float ContactPointX, float ContactPointY, float ContactPointZ)
    {
        ProcessCharacterDamageFromServer(DamagedCharacter, CusingDamageCharacter, PhysicsDamage, MagicDamage, FireDamage, HolyDamage,
            PoiseDamage, AngleHitFrom, ContactPointX, ContactPointY, ContactPointZ);   
    }

    private void ProcessCharacterDamageFromServer(ulong DamagedCharacter, ulong CusingDamageCharacter, 
        float PhysicsDamage, float MagicDamage, float FireDamage, float HolyDamage, float PoiseDamage,
        float AngleHitFrom, float ContactPointX, float ContactPointY, float ContactPointZ)
    {
        CharacterManager DamagedCharacterManager = NetworkManager.Singleton.SpawnManager
            .SpawnedObjects[DamagedCharacter].gameObject.GetComponent<CharacterManager>();

        CharacterManager CusingDamageCharacterManager = NetworkManager.Singleton.SpawnManager
            .SpawnedObjects[CusingDamageCharacter].gameObject.GetComponent<CharacterManager>();

        TakeHealthDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.mTakeHealthDamageEffect);
        damageEffect.mPhysicalDamage = PhysicsDamage;
        damageEffect.mMagicDamage = MagicDamage;
        damageEffect.mFireDamage = FireDamage;
        damageEffect.mHolyDamage = HolyDamage;
        damageEffect.mPoiseDamage = PoiseDamage;
        damageEffect.mAngleHitForm = AngleHitFrom;
        damageEffect.mContactPoint = new Vector3(ContactPointX, ContactPointY, ContactPointZ);
        damageEffect.mCharacterCauingDamage = CusingDamageCharacterManager;

        DamagedCharacterManager.mCharacterEffectsManager.ProcessInstantEffect(damageEffect);
    }
}
