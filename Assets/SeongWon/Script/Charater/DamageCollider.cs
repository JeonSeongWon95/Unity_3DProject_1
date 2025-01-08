using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("Collider")]
    [SerializeField] protected Collider mDamageCollider;

    [Header("Damage")]
    public float mPhysicalDamage = 0;
    public float mMagicDamage = 0;
    public float mFireDamage = 0;
    public float mLightningDamage = 0;
    public float mHolyDamage = 0;

    [Header("Contact Point")]
    protected Vector3 mContactPoint;

    [Header("Character Damaged")]
    protected List<CharacterManager> mCahractersDamaged = new List<CharacterManager>();

    protected virtual void Awake() 
    {
        
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        CharacterManager mDamageTarget = other.GetComponentInParent<CharacterManager>();

        if (mDamageTarget != null) 
        {
            mContactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            DamageTarget(mDamageTarget);
        }
    }

    protected virtual void DamageTarget(CharacterManager DamageTarget) 
    {
        if (mCahractersDamaged.Contains(DamageTarget))
            return;

        mCahractersDamaged.Add(DamageTarget);

        TakeHealthDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.mTakeHealthDamageEffect);
        damageEffect.mPhysicalDamage = mPhysicalDamage;
        damageEffect.mMagicDamage = mMagicDamage;
        damageEffect.mLightningDamage = mLightningDamage;
        damageEffect.mHolyDamage = mHolyDamage;
        damageEffect.mFireDamage = mFireDamage;
        damageEffect.mContactPoint = mContactPoint;
        damageEffect.mAngleHitForm = Vector3.SignedAngle(transform.forward, DamageTarget.transform.forward, Vector3.up);

        DamageTarget.mCharacterEffectsManager.ProcessInstantEffect(damageEffect);
    }

    public virtual void EnableDamageCollider() 
    {
        mDamageCollider.enabled = true;
    }

    public virtual void DisableDamageCollider() 
    {
        mDamageCollider.enabled = false;
        mCahractersDamaged.Clear();
    }
}
