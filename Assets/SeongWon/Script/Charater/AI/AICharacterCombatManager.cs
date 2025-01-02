using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{
    [Header("Detection")]
    [SerializeField] float mDetectionRadius = 15;
    [SerializeField] float mMinmumDetectionAngle = -35;
    [SerializeField] float mMaximumDetectionAngle = 35;
    public void FindTargetOfSight(AICharacterManager AICharacter) 
    {
        if (mCurrentTarget != null)
            return;


        Collider[] colliders = Physics.OverlapSphere(AICharacter.transform.position, mDetectionRadius,
            WorldUtilityManager.Instance.GetCharacterLayers());

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager TargetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            if(TargetCharacter == null) 
                continue;

            if (TargetCharacter == AICharacter)
                continue;

            if(TargetCharacter.mIsDead.Value)
                continue;

            if (WorldUtilityManager.Instance.mCanDamageThisTarget(mCharacterManager.mCharacterGroup,
                TargetCharacter.mCharacterGroup)) 
            {
                Vector3 TargetDirection = TargetCharacter.transform.position - AICharacter.transform.position;
                float ViewableAngle = Vector3.Angle(TargetDirection, AICharacter.transform.forward);

                if (ViewableAngle < mMaximumDetectionAngle && ViewableAngle > mMinmumDetectionAngle) 
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
}
