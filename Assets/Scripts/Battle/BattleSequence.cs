using System;
using System.Collections;
using UnityEngine;

public abstract class BattleSequence : MonoBehaviour
{
    public enum TriggerType
    {
        MoveStart,
        MoveEnd,
        TurnStart,
        UndoRedoEnd
    }

    protected Battle Battle => _battle;
    public bool HasPlayed => _timesPlayed > 0;
    public abstract TriggerType Trigger { get; }
    public static readonly Comparison<BattleSequence> Comparison = (a, b) => a._priority.CompareTo(b._priority);

    [SerializeField] private int _priority = 0;
    private Battle _battle;
    private int _timesPlayed = 0;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
    }

    public abstract bool ShouldPlay(Character interactor, Character target);
    public IEnumerator Play(Character interactor, Character target)
    {
        _timesPlayed++;
        return GetSequence(interactor, target);
    }
    protected abstract IEnumerator GetSequence(Character interactor, Character target);

    protected void Cancel()
    {
        _timesPlayed--;
    }
}
