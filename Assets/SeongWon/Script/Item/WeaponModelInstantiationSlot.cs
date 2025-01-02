using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModelInstantiationSlot : MonoBehaviour
{
    public GameObject mCurrentWeaponModel;
    public WeaponModelSlot mWeaponModelSlot;

    public void UnloadWeaponModel() 
    {
        if (mCurrentWeaponModel != null) 
        {
            Destroy(mCurrentWeaponModel);
        }
    }

    public void LoadWeaponModel(GameObject NewWeaponModel) 
    {
        mCurrentWeaponModel = NewWeaponModel;
        NewWeaponModel.transform.parent = transform;
        NewWeaponModel.transform.localPosition = Vector3.zero;
        NewWeaponModel.transform.localRotation = Quaternion.identity;
        NewWeaponModel.transform.localScale = Vector3.one;
    }
}
