using System.Collections;
using UnityEngine;

public class BattleSequence_AttackTutorial : BattleSequence
{
    public override TriggerType Trigger => TriggerType.TurnStart;

    public override bool ShouldPlay(Character interactor, Character target)
    {
        return !HasPlayed;
    }

    protected override IEnumerator GetSequence(Character interactor, Character target)
    {
        Debug.Log("Attack tutorial sequence started.");
        yield break;
    }
}
