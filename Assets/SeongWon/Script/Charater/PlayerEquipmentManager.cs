using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager mPlayerManager;

    public WeaponModelInstantiationSlot mRightSlot;
    public WeaponModelInstantiationSlot mLeftSlot;

    [SerializeField] WeaponManager mRightWeaponManager;
    [SerializeField] WeaponManager mLeftWeaponManager;

    public GameObject mRightHandWeaponModel;
    public GameObject mLeftHandWeaponModel;

    protected override void Awake()
    {
        base.Awake();
        mPlayerManager = GetComponent<PlayerManager>();
        InitializeWeaponSlot();
    }

    protected override void Start()
    {
        base.Start();
        LoadWeaponOnBothHands();
    }

    private void InitializeWeaponSlot() 
    {
        WeaponModelInstantiationSlot[] WeaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

        foreach (var WeaponSlot in WeaponSlots) 
        {
            if (WeaponSlot.mWeaponModelSlot == WeaponModelSlot.RightHand)
            {
                mRightSlot = WeaponSlot;
            }
            else if (WeaponSlot.mWeaponModelSlot == WeaponModelSlot.LeftHand) 
            {
                mLeftSlot = WeaponSlot;
            }
        }
    }

    public void LoadWeaponOnBothHands() 
    {
        LoadRightWeapon();
        LoadLeftWeapon();
    }

    public void LoadRightWeapon() 
    {
        if (mPlayerManager.mPlayerInventoryManager.mCurrentRightWeapon != null) 
        {
            mRightSlot.UnloadWeaponModel();
            mRightHandWeaponModel = Instantiate(mPlayerManager.mPlayerInventoryManager.mCurrentRightWeapon.mWeaponModel);
            mRightSlot.LoadWeaponModel(mRightHandWeaponModel);
            mRightWeaponManager = mRightHandWeaponModel.GetComponent<WeaponManager>();

            mRightWeaponManager.SetWeaponDamage(mPlayerManager,
                mPlayerManager.mPlayerInventoryManager.mCurrentRightWeapon);
        }
    }

    public void SwitchRightWeapon() 
    {
        if (!mPlayerManager.IsOwner)
            return;

        mPlayerManager.mPlayerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true);

        WeaponItem mSelectedWeapon = null;

        mPlayerManager.mPlayerInventoryManager.mRightHandWeaponIndex += 1;

        if (mPlayerManager.mPlayerInventoryManager.mRightHandWeaponIndex < 0 ||
            mPlayerManager.mPlayerInventoryManager.mRightHandWeaponIndex > 2) 
        {

            mPlayerManager.mPlayerInventoryManager.mRightHandWeaponIndex = 0;

            float WeaponCount = 0;
            WeaponItem FirstWeapon = null;
            int FirstWeaponPosition = 0;

            for (int i = 0; i < mPlayerManager.mPlayerInventoryManager.mWeaponsInRightHandSlots.Length; i++)
            {
                if (mPlayerManager.mPlayerInventoryManager.mWeaponsInRightHandSlots[i].mItemID != WorldItemDataBase.Instance.mUnarmedWeapon.mItemID)
                {
                    WeaponCount++;

                    if (FirstWeapon == null)
                    {
                        FirstWeapon = mPlayerManager.mPlayerInventoryManager.mWeaponsInRightHandSlots[i];
                        FirstWeaponPosition = i;
                    }
                }
            }

            if (WeaponCount <= 1)
            {
                mPlayerManager.mPlayerInventoryManager.mRightHandWeaponIndex = -1;
                mSelectedWeapon = WorldItemDataBase.Instance.mUnarmedWeapon;
                mPlayerManager.mPlayerNetworkManager.mCurrentRightHandWeaponID.Value = mSelectedWeapon.mItemID;

            }
            else
            {
                mPlayerManager.mPlayerInventoryManager.mRightHandWeaponIndex = FirstWeaponPosition;
                mPlayerManager.mPlayerNetworkManager.mCurrentRightHandWeaponID.Value = FirstWeapon.mItemID;
            }

            return;
        }

        foreach (WeaponItem weaponitem in mPlayerManager.mPlayerInventoryManager.mWeaponsInRightHandSlots) 
        {

            if (mPlayerManager.mPlayerInventoryManager.mWeaponInLeftHandSlots
                [mPlayerManager.mPlayerInventoryManager.mRightHandWeaponIndex].mItemID != 
                WorldItemDataBase.Instance.mUnarmedWeapon.mItemID) 
            {
                mSelectedWeapon = mPlayerManager.mPlayerInventoryManager.mWeaponsInRightHandSlots
                    [mPlayerManager.mPlayerInventoryManager.mRightHandWeaponIndex];

                mPlayerManager.mPlayerNetworkManager.mCurrentRightHandWeaponID.Value =
                    mPlayerManager.mPlayerInventoryManager.mWeaponsInRightHandSlots
                    [mPlayerManager.mPlayerInventoryManager.mRightHandWeaponIndex].mItemID;
                return;
            }
        }

        if (mSelectedWeapon == null && mPlayerManager.mPlayerInventoryManager.mRightHandWeaponIndex <= 2)
        {
            SwitchRightWeapon();
        }
    }

    public void LoadLeftWeapon() 
    {
        if (mPlayerManager.mPlayerInventoryManager.mCurrentLeftWeapon != null)
        {
            mLeftSlot.UnloadWeaponModel();
            mLeftHandWeaponModel = Instantiate(mPlayerManager.mPlayerInventoryManager.mCurrentLeftWeapon.mWeaponModel);
            mLeftSlot.LoadWeaponModel(mLeftHandWeaponModel);
            mLeftWeaponManager = mLeftHandWeaponModel.GetComponent<WeaponManager>();
            mLeftWeaponManager.SetWeaponDamage(mPlayerManager, mPlayerManager.mPlayerInventoryManager.mCurrentLeftWeapon);
        }
    }

    public void SwitchLeftWeapon() 
    {
        if (!mPlayerManager.IsOwner)
            return;

        mPlayerManager.mPlayerAnimatorManager.PlayTargetActionAnimation("Swap_Left_Weapon_01", false, false, true, true);

        WeaponItem mSelectedWeapon = null;

        mPlayerManager.mPlayerInventoryManager.mLeftHandWeaponIndex += 1;

        if (mPlayerManager.mPlayerInventoryManager.mLeftHandWeaponIndex < 0 ||
            mPlayerManager.mPlayerInventoryManager.mLeftHandWeaponIndex > 2)
        {

            mPlayerManager.mPlayerInventoryManager.mLeftHandWeaponIndex = 0;

            float WeaponCount = 0;
            WeaponItem FirstWeapon = null;
            int FirstWeaponPosition = 0;

            for (int i = 0; i < mPlayerManager.mPlayerInventoryManager.mWeaponInLeftHandSlots.Length; i++)
            {
                if (mPlayerManager.mPlayerInventoryManager.mWeaponInLeftHandSlots[i].mItemID != WorldItemDataBase.Instance.mUnarmedWeapon.mItemID)
                {
                    WeaponCount++;

                    if (FirstWeapon == null)
                    {
                        FirstWeapon = mPlayerManager.mPlayerInventoryManager.mWeaponInLeftHandSlots[i];
                        FirstWeaponPosition = i;
                    }
                }
            }

            if (WeaponCount <= 1)
            {
                mPlayerManager.mPlayerInventoryManager.mLeftHandWeaponIndex = -1;
                mSelectedWeapon = WorldItemDataBase.Instance.mUnarmedWeapon;
                mPlayerManager.mPlayerNetworkManager.mCurrentLeftHandWeaponID.Value = mSelectedWeapon.mItemID;

            }
            else
            {
                mPlayerManager.mPlayerInventoryManager.mLeftHandWeaponIndex = FirstWeaponPosition;
                mPlayerManager.mPlayerNetworkManager.mCurrentLeftHandWeaponID.Value = FirstWeapon.mItemID;
            }

            return;
        }

        foreach (WeaponItem weaponitem in mPlayerManager.mPlayerInventoryManager.mWeaponInLeftHandSlots)
        {

            if (mPlayerManager.mPlayerInventoryManager.mWeaponInLeftHandSlots
                [mPlayerManager.mPlayerInventoryManager.mLeftHandWeaponIndex].mItemID !=
                WorldItemDataBase.Instance.mUnarmedWeapon.mItemID)
            {
                mSelectedWeapon = mPlayerManager.mPlayerInventoryManager.mWeaponInLeftHandSlots
                    [mPlayerManager.mPlayerInventoryManager.mLeftHandWeaponIndex];

                mPlayerManager.mPlayerNetworkManager.mCurrentLeftHandWeaponID.Value =
                    mPlayerManager.mPlayerInventoryManager.mWeaponInLeftHandSlots
                    [mPlayerManager.mPlayerInventoryManager.mLeftHandWeaponIndex].mItemID;
                return;
            }
        }

        if (mSelectedWeapon == null && mPlayerManager.mPlayerInventoryManager.mLeftHandWeaponIndex <= 2)
        {
            SwitchLeftWeapon();
        }
    }

    public void OpenDamageCollider() 
    {
        if (mPlayerManager.mPlayerNetworkManager.mIsUsingRightHand.Value) 
        {
            mRightWeaponManager.mMeleeDamageCollider.EnableDamageCollider();
        }
        else if (mPlayerManager.mPlayerNetworkManager.mIsUsingLeftHand.Value)
        {
            mLeftWeaponManager.mMeleeDamageCollider.EnableDamageCollider();
        }
    }

    public void CloseDamageCollider()
    {
        if (mPlayerManager.mPlayerNetworkManager.mIsUsingRightHand.Value)
        {
            mRightWeaponManager.mMeleeDamageCollider.DisableDamageCollider();
        }
        else if (mPlayerManager.mPlayerNetworkManager.mIsUsingLeftHand.Value)
        {
            mLeftWeaponManager.mMeleeDamageCollider.DisableDamageCollider();
        }
    }
}
