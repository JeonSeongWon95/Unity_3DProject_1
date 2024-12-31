using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager mCharaterManager;

    int vertical;
    int horizontal;

    [Header("Damage Animations")]
    public string LastDamageAnimationPlayed;

    [SerializeField] string Hit_Forward_Medium_01 = "Hit_Forward_Medium_01";
    [SerializeField] string Hit_Forward_Medium_02 = "Hit_Forward_Medium_02";

    [SerializeField] string Hit_Backward_Medium_01 = "Hit_Backward_Medium_01";
    [SerializeField] string Hit_Backward_Medium_02 = "Hit_Backward_Medium_02";

    [SerializeField] string Hit_Right_Medium_01 = "Hit_Right_Medium_01";
    [SerializeField] string Hit_Right_Medium_02 = "Hit_Right_Medium_02";

    [SerializeField] string Hit_Left_Medium_01 = "Hit_Left_Medium_01";
    [SerializeField] string Hit_Left_Medium_02 = "Hit_Left_Medium_02";

    public List<string> Forward_Medium_Damage = new List<string>();
    public List<string> Backward_Medium_Damage = new List<string>();
    public List<string> Right_Medium_Damage = new List<string>();
    public List<string> Left_Medium_Damage = new List<string>();


    protected virtual void Awake()
    {
        mCharaterManager = GetComponent<CharacterManager>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");

    }

    protected virtual void Start() 
    {
        Forward_Medium_Damage.Add(Hit_Forward_Medium_01);
        Forward_Medium_Damage.Add(Hit_Forward_Medium_02);

        Backward_Medium_Damage.Add(Hit_Backward_Medium_01);
        Backward_Medium_Damage.Add(Hit_Backward_Medium_02);

        Right_Medium_Damage.Add(Hit_Right_Medium_01);
        Right_Medium_Damage.Add(Hit_Right_Medium_02);

        Left_Medium_Damage.Add(Hit_Left_Medium_01);
        Left_Medium_Damage.Add(Hit_Left_Medium_02);
    }

    public string GetRandomAnimationFromList(List<string> list) 
    {
        List<string> finalList = new List<string>();

        foreach (var item in list)
        {
            finalList.Add(item);
        }

        finalList.Remove(LastDamageAnimationPlayed);

        for(int i = finalList.Count -1; i > -1; i--)
        {
            if (finalList[i] == null) 
            {
                finalList.RemoveAt(i);
            }
        }

        int RandomIndex = Random.Range(0, finalList.Count);
        return finalList[RandomIndex];
    }

    public void UpdateAnimatorValues(float horizontalvalue, float verticalvalue, bool IsSprint) 
    {
        if (mCharaterManager.mAnimator != null)
        {
            float horizontalAmount = horizontalvalue;
            float verticalAmount = verticalvalue;

            if (horizontalvalue > 0 && horizontalvalue <= 0.5f)
            {
                horizontalAmount = 0.5f;
            }
            else if (horizontalvalue > 0.5f && horizontalvalue <= 1.0f)
            {
                horizontalAmount = 1.0f;
            }
            else if (horizontalvalue < 0 && horizontalvalue >= -0.5f)
            {
                horizontalAmount = -0.5f;
            }
            else if (horizontalvalue < -0.5f && horizontalvalue >= -1.0f)
            {
                horizontalAmount = -1.0f;
            }
            else
            {
                horizontalAmount = 0.0f;
            }

            if (verticalvalue > 0 && verticalvalue <= 0.5f)
            {
                verticalAmount = 0.5f;
            }
            else if (verticalvalue > 0.5f && verticalvalue <= 1.0f)
            {
                verticalAmount = 1.0f;
            }
            else if (verticalvalue < 0 && verticalvalue >= -0.5f)
            {
                verticalAmount = -0.5f;
            }
            else if (verticalvalue < -0.5f && verticalvalue >= -1.0f)
            {
                verticalAmount = -1.0f;
            }
            else 
            {
                verticalAmount = 0.0f;
            }

            if (IsSprint) 
            {
                verticalAmount = 2;
            }

            mCharaterManager.mAnimator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            mCharaterManager.mAnimator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
        }
    }

    public virtual void PlayTargetActionAnimation(string AnimationName, bool IsPerformingAction, bool IsRootMotion = true, 
        bool CanRotate = false, bool CanMove = false) 
    {

        mCharaterManager.ApplyRootMotion = IsRootMotion;
        mCharaterManager.mAnimator.CrossFade(AnimationName, 0.2f);
        mCharaterManager.IsPerformingAction = IsPerformingAction;
        mCharaterManager.CanRotate = CanRotate;
        mCharaterManager.CanMove = CanMove;

        mCharaterManager.mCharacterNetworkManager.PlayTargetActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId
            ,AnimationName, IsRootMotion);
    }

    public virtual void PlayTargetAttackActionAnimation(AttackType NewAttackType, string AnimationName, bool IsPerformingAction, bool IsRootMotion = true,
    bool CanRotate = false, bool CanMove = false)
    {
        mCharaterManager.mCharacterCombatManager.mLastAttackAnimationPerformed = AnimationName;
        mCharaterManager.mCharacterCombatManager.mCurrentAttackType = NewAttackType;
        mCharaterManager.ApplyRootMotion = IsRootMotion;
        mCharaterManager.mAnimator.CrossFade(AnimationName, 0.2f);
        mCharaterManager.IsPerformingAction = IsPerformingAction;
        mCharaterManager.CanRotate = CanRotate;
        mCharaterManager.CanMove = CanMove;

        mCharaterManager.mCharacterNetworkManager.PlayTargetAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId
            , AnimationName, IsRootMotion);
    }

    public virtual void EnableCanDoCombo()
    {

    }

    public virtual void DisableCanDoCombo()
    {
       
    }
}
