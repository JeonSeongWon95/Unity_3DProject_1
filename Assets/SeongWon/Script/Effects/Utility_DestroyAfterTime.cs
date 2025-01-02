using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility_DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float mTimeUnitDestroyed = 5.0f;
    private void Awake()
    {
        Destroy(gameObject, mTimeUnitDestroyed);
    }
}
