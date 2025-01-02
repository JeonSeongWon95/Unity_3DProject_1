using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WorldItemDataBase : MonoBehaviour
{
    public static WorldItemDataBase Instance;
    public WeaponItem mUnarmedWeapon;
    [SerializeField] List<WeaponItem> mWeapons = new List<WeaponItem>();
    private List<Item> mItems = new List<Item>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }

        foreach (var weapon in mWeapons)
        {
            mItems.Add(weapon);
        }

        for (int i = 0; i < mItems.Count; i++)
        {
            mItems[i].mItemID = i;
        }
    }

    public WeaponItem GetWeaponByID(int ID) 
    {
        return mWeapons.FirstOrDefault(weapon => weapon.mItemID == ID);
    }
}
