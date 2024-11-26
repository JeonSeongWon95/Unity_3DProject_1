using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharaterNetworkManager : NetworkBehaviour
{
    [Header("Position")]
    public NetworkVariable<Vector3> mNetworkPosition = new NetworkVariable<Vector3>(Vector3.zero, 
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<Quaternion> mNetworkRotation = new NetworkVariable<Quaternion>(Quaternion.identity,
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public Vector3 mNetworkPositionVelocity;
    public float mNetworkPositionSmoothTime = 0.1f;
    public float mNetworkRotateSmoothTime = 0.1f;
}
