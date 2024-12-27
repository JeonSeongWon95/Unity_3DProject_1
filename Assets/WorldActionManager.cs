using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldActionManager : MonoBehaviour
{
    static public WorldActionManager Instance;

    [Header("Weapon Item Actions")]
    public WeaponItemAction[] mWeaponItemActions;

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

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < mWeaponItemActions.Length; i++)
        {
            mWeaponItemActions[i].mActionID = i;
        }
    }

    public WeaponItemAction GetWeaponItemActionByID(int ID) 
    {
        return mWeaponItemActions.FirstOrDefault(Action => Action.mActionID == ID);
    }

}
