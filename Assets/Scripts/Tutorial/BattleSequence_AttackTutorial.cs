using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TutorialWindow))]
public class BattleSequence_AttackTutorial : BattleSequence
{
    public override TriggerType Trigger => TriggerType.TurnStart;

    private TutorialWindow _window;

    private void Awake()
    {
        _window = GetComponent<TutorialWindow>();
    }

    public override bool ShouldPlay(Character interactor, Character target)
    {
        return !HasPlayed;
    }

    protected override IEnumerator GetSequence(Character interactor, Character target)
    {
        yield return _window.Show();
    }
}
