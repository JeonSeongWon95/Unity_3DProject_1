using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> mKeys = new List<TKey>();
    [SerializeField] private List<TValue> mValues = new List<TValue>();
    public void OnBeforeSerialize() 
    {
        mKeys.Clear();
        mValues.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this) 
        {
            mKeys.Add(pair.Key);
            mValues.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize() 
    {
        Clear();

        if (mKeys.Count != mValues.Count) 
        {
            Debug.LogError("Key Count Does Not Match Value Count");
        }

        for (int i = 0; i < mKeys.Count; i++)
        {
            Add(mKeys[i], mValues[i]);
        }
    }
}
