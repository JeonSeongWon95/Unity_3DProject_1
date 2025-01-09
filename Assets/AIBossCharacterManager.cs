using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AIBossCharacterManager : AICharacterManager
{
    public int mBossID = 0;
    [SerializeField] bool mHasBeenDefeated = false;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer) 
        {
            if (!WorldSaveGameManager.Instance.mCurrentCharacterData.mBossesAwakened.ContainsKey(mBossID))
            {
                WorldSaveGameManager.Instance.mCurrentCharacterData.mBossesAwakened.Add(mBossID, false);
                WorldSaveGameManager.Instance.mCurrentCharacterData.mBossesDefeated.Add(mBossID, false);
            }
            else 
            {
                mHasBeenDefeated = WorldSaveGameManager.Instance.mCurrentCharacterData.mBossesDefeated[mBossID];

                if (mHasBeenDefeated)
                {
                    mAICharacterNetworkManager.mNetworkIsActive.Value = false;
                }
            }
        }
    }

    public override IEnumerator ProcessDeathEvent(bool ManuallySelectDeathAnimation = false)
    {
        if (IsOwner)
        {
            mCharacterNetworkManager.mNetworkCurrentHealth.Value = 0;
            mIsDead.Value = true;

            if (!ManuallySelectDeathAnimation)
            {
                mCharacterAnimatorManager.PlayTargetActionAnimation("Death", true);
            }

            mHasBeenDefeated = true;

            if (!WorldSaveGameManager.Instance.mCurrentCharacterData.mBossesAwakened.ContainsKey(mBossID))
            {
                WorldSaveGameManager.Instance.mCurrentCharacterData.mBossesAwakened.Add(mBossID, true);
                WorldSaveGameManager.Instance.mCurrentCharacterData.mBossesDefeated.Add(mBossID, true);
            }
            else
            {
                WorldSaveGameManager.Instance.mCurrentCharacterData.mBossesAwakened.Remove(mBossID);
                WorldSaveGameManager.Instance.mCurrentCharacterData.mBossesDefeated.Remove(mBossID);
                WorldSaveGameManager.Instance.mCurrentCharacterData.mBossesAwakened.Add(mBossID, true);
                WorldSaveGameManager.Instance.mCurrentCharacterData.mBossesDefeated.Add(mBossID, true);
            }

            WorldSaveGameManager.Instance.SaveGame();
        }


        yield return new WaitForSeconds(5);

    }
}
