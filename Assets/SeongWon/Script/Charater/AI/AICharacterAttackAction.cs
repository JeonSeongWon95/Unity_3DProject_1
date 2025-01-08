using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="A.I/Actions/Attack")]
public class AICharacterAttackAction : ScriptableObject
{
    [Header("Attack")]
    [SerializeField] private string AttackAnimation;

    [Header("Combo Action")]
    public AICharacterAttackAction mComboAction;

    [Header("Action Valus")]
    [SerializeField] AttackType mAttackType;
    public int mAttackWeight = 50;
    public float mActionRecoveryTime = 1.5f;
    public float mMinmumAttackAngle = -35;
    public float mMaxmumAttackAngle = 35;
    public float mMinmumAttackDistance = 0;
    public float mMaxmumAttackDistnace = 2;

    public void AttemToPerformAction(AICharacterManager mAiCharacter) 
    {
        mAiCharacter.mCharacterAnimatorManager.PlayTargetAttackActionAnimation(mAttackType, AttackAnimation, true);
    }
}
