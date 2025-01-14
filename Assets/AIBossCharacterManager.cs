using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AIBossCharacterManager : AICharacterManager
{
    public int mBossID = 0;
    [SerializeField] bool mHasBeenDefeated = false;
    [SerializeField] bool mHasBeenAwakened = false;
    [SerializeField] List<FogWallInteractable> mFogWalls;

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
                mHasBeenAwakened = WorldSaveGameManager.Instance.mCurrentCharacterData.mBossesAwakened[mBossID];

                StartCoroutine(GetFogWallsFromWorldObjectManager());

                if (mHasBeenAwakened) 
                {
                    for (int i = 0; i < mFogWalls.Count; i++)
                    {
                        mFogWalls[i].mNetworkIsActive.Value = true;
                    }
                }

                if (mHasBeenDefeated)
                {
                    for (int i = 0; i < mFogWalls.Count; i++)
                    {
                        mFogWalls[i].mNetworkIsActive.Value = false;
                    }

                    mAICharacterNetworkManager.mNetworkIsActive.Value = false;
                }
            }
        }
    }

    private IEnumerator GetFogWallsFromWorldObjectManager() 
    {
        while (WorldObjectManager.Instance.mFogWallInteractables.Count == 0)
        {
            yield return new WaitForEndOfFrame();
        }

        mFogWalls = new List<FogWallInteractable>();

        foreach (var FogWall in WorldObjectManager.Instance.mFogWallInteractables)

        {
            if (FogWall.FogWallID == mBossID)
            {
                mFogWalls.Add(FogWall);
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
