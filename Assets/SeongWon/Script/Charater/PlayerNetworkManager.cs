using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class PlayerNetworkManager : CharaterNetworkManager
{
    public NetworkVariable<FixedString64Bytes> mCharacterName = new NetworkVariable<FixedString64Bytes>("Character",
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
}
