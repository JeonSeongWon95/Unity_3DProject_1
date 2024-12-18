using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{

    [Header("Item Information")]
    public string mItemName;
    public Sprite mItemIcon;
    public int mItemID;

    [TextArea] public string mItemDescription;
}
