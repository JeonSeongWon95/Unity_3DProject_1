using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{
    [Header("Target Information")]
    public float mViewableAngle;
    public Vector3 mTargetDirection;

    [Header("Detection")]
    [SerializeField] float mDetectionRadius = 15;
    public float mMinmumDetectionAngle = -35;
    public float mMaximumDetectionAngle = 35;

    [Header("Rotation Variable")]
    [SerializeField] float mRotateSpeed = 15.0f;
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
                        TargetDirection = TargetCharacter.transform.position - transform.position;
                        mViewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, TargetDirection);
                        AICharacter.mCharacterCombatManager.SetTarget(TargetCharacter);
                        PivotTowardsTarget(AICharacter);
                    }
                }
            }
        }
    }

    public void PivotTowardsTarget(AICharacterManager mAiCharacter) 
    {
        if(mAiCharacter.IsPerformingAction)
            return;

        float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.y, mViewableAngle, Time.deltaTime * mRotateSpeed);
        Quaternion targetRotation = Quaternion.Euler(0, smoothAngle, 0);
        mAiCharacter.transform.rotation = targetRotation;
    }
}
