using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    [Header("Debug Delete Later")]
    [SerializeField] InstantCharacterEffect mInstantCharacterEffect;
    [SerializeField] bool IsProcessEffect = false;

    private void Update()
    {
        if (IsProcessEffect) 
        {
            IsProcessEffect = false;
            InstantCharacterEffect mEffect = Instantiate(mInstantCharacterEffect);
            ProcessInstantEffect(mEffect);
        }
    }
}
