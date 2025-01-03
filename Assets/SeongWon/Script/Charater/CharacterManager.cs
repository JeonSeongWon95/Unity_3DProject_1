using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterManager : NetworkBehaviour
{

    [HideInInspector] public CharacterController mCharaterController;
    [HideInInspector] public Animator mAnimator;
    [HideInInspector] public CharacterNetworkManager mCharacterNetworkManager;
    [HideInInspector] public CharacterEffectsManager mCharacterEffectsManager;
    [HideInInspector] public CharacterAnimatorManager mCharacterAnimatorManager;
    [HideInInspector] public CharacterCombatManager mCharacterCombatManager;
    [HideInInspector] public CharacterSoundFXManager mCharacterSoundFXManager;
    [HideInInspector] public CharacterLocomotionManager mCharacterLocomotionManager;

    [Header("Status")]
    public NetworkVariable<bool> mIsDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    [Header("Character Group")]
    public CharacterGroup mCharacterGroup;

    [Header("FLAGS")]
    public bool IsPerformingAction = false;
    public bool ApplyRootMotion = false;
    public bool IsGround = true;
    public bool CanRotate = true;
    public bool CanMove = true;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        mCharaterController = GetComponent<CharacterController>();
        mCharacterNetworkManager = GetComponent<CharacterNetworkManager>();
        mAnimator = GetComponent<Animator>();
        mCharacterEffectsManager = GetComponent<CharacterEffectsManager>();
        mCharacterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        mCharacterCombatManager = GetComponent<CharacterCombatManager>();
        mCharacterSoundFXManager = GetComponent<CharacterSoundFXManager>();
        mCharacterLocomotionManager = GetComponent<CharacterLocomotionManager>();
    }

    protected virtual void Start() 
    {
        IgnoreMyOwnColliders();
    }

    protected virtual void Update() 
    {
        mAnimator.SetBool("IsGround", IsGround);

        if (IsOwner)
        {
            mCharacterNetworkManager.mNetworkPosition.Value = transform.position;
            mCharacterNetworkManager.mNetworkRotation.Value = transform.rotation;
        }
        else 
        {
            transform.position = Vector3.SmoothDamp(transform.position, mCharacterNetworkManager.mNetworkPosition.Value,
                ref mCharacterNetworkManager.mNetworkPositionVelocity, mCharacterNetworkManager.mNetworkPositionSmoothTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, mCharacterNetworkManager.mNetworkRotation.Value,
                mCharacterNetworkManager.mNetworkRotateSmoothTime);
        }
    }

    protected virtual void FixedUpdate() 
    {

    }

    protected virtual void LateUpdate() 
    {

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        mCharacterNetworkManager.mNetworkIsMoving.OnValueChanged += mCharacterNetworkManager.OnIsMovingChanged;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        mCharacterNetworkManager.mNetworkIsMoving.OnValueChanged -= mCharacterNetworkManager.OnIsMovingChanged;
    }

    public virtual IEnumerator ProcessDeathEvent(bool ManuallySelectDeathAnimation = false) 
    {
        if (IsOwner) 
        {
            mCharacterNetworkManager.mNetworkCurrentHealth.Value = 0;
            mIsDead.Value = true;

            if (!ManuallySelectDeathAnimation)
            {
                mCharacterAnimatorManager.PlayTargetActionAnimation("Death", true);
            }

        }


        yield return new WaitForSeconds(5);

    }

    public virtual void ReviveCharacter() 
    {

    }

    protected virtual void IgnoreMyOwnColliders() 
    {
        Collider mCharacterControllerCollider = GetComponent<Collider>();
        Collider[] mDamageableCharacterCollider = GetComponentsInChildren<Collider>();

        List<Collider> mIgnoreColliders = new List<Collider>();

        foreach (var Collider in mDamageableCharacterCollider) 
        {
            mIgnoreColliders.Add(Collider);
        }

        mIgnoreColliders.Add(mCharacterControllerCollider);

        foreach (var Collider in mIgnoreColliders)
        {
            foreach (var OtherCollider in mIgnoreColliders)
            {
                Physics.IgnoreCollision(Collider, OtherCollider, true);
            }

        }
    }

}
