using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{
    protected AICharacterManager mAICharacterManager;

    [Header("Action Recovery")]
    public float mActionRecoveryTimer = 0;

    [Header("Target Information")]
    public float mDistanceFromTarget;
    public float mViewableAngle;
    public Vector3 mTargetDirection;

    [Header("Detection")]
    [SerializeField] float mDetectionRadius = 15;
    public float mMinmumDetectionAngle = -35;
    public float mMaximumDetectionAngle = 35;

    protected override void Awake()
    {
        base.Awake();

        mLockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
        mAICharacterManager = GetComponent<AICharacterManager>();
    }

    public void FindTargetOfSight(AICharacterManager AICharacter) 
    {
        if (mCurrentTarget != null)
            return;

        Collider[] colliders = Physics.OverlapSphere(AICharacter.transform.position, mDetectionRadius,
            WorldUtilityManager.Instance.GetCharacterLayers());

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager TargetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            if (TargetCharacter == null) 
                continue;

            if (TargetCharacter == AICharacter)
                continue;

            if (TargetCharacter.mIsDead.Value)
                continue;

            if (WorldUtilityManager.Instance.mCanDamageThisTarget(mCharacterManager.mCharacterGroup,
                TargetCharacter.mCharacterGroup)) 
            {

                Vector3 TargetDirection = TargetCharacter.transform.position - AICharacter.transform.position;
                float AngleOfTarget = Vector3.Angle(TargetDirection, AICharacter.transform.forward);

                if (AngleOfTarget < mMaximumDetectionAngle && AngleOfTarget > mMinmumDetectionAngle) 
                {
                    if (Physics.Linecast(AICharacter.mCharacterCombatManager.mLockOnTransform.position,
                        TargetCharacter.mCharacterCombatManager.mLockOnTransform.position,
                        WorldUtilityManager.Instance.GetEnviroLayers()))
                    {

                    }
                    else 
                    {
                        AICharacter.mCharacterCombatManager.SetTarget(TargetCharacter);
                    }
                }
            }
        }
    }

    public void HandleActionRecovery(AICharacterManager AICharacter) 
    {
        if (mActionRecoveryTimer > 0) 
        {
            if (AICharacter.IsPerformingAction) 
            {
                mActionRecoveryTimer -= Time.deltaTime;
            }
        }
    }
}
