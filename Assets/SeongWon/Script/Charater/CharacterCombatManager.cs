using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterCombatManager : NetworkBehaviour
{
    protected CharacterManager mCharacterManager;

    [Header("Last Attack Animation Performed")]
    public string mLastAttackAnimationPerformed;

    [Header("Attack Target")]
    public CharacterManager mCurrentTarget;

    public AttackType mCurrentAttackType;

    [Header("Lock On Transform")]
    public Transform mLockOnTransform;

    protected virtual void Awake()
    {
        mCharacterManager = GetComponent<CharacterManager>();
    }

    protected virtual void Update() 
    {

    }

    public virtual void SetTarget(CharacterManager NewTarget) 
    {
        if (mCharacterManager.IsOwner) 
        {
            if (NewTarget != null)
            {
                mCurrentTarget = NewTarget;
                mCharacterManager.mCharacterNetworkManager.mCurrentTargetNetworkObjectID.Value =
                    NewTarget.GetComponent<NetworkObject>().NetworkObjectId;
            }
            else 
            {
                mCurrentTarget = null;
            }
        }
    }

    public void EnableIsInvulnerable() 
    {
        if(mCharacterManager.IsOwner)
            mCharacterManager.mCharacterNetworkManager.mNetworkIsInVlnerable.Value = true;
    }

    public void DisableIsInvulnerable()
    {
        if (mCharacterManager.IsOwner)
            mCharacterManager.mCharacterNetworkManager.mNetworkIsInVlnerable.Value = false;
    }

}
