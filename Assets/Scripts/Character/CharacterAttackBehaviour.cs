using DG.Tweening;
using Hellmade.Sound;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterAttackBehaviour : MonoBehaviour
{
    private const float ApproxFrameDuration = .1f;
    private const int MeleeFrames = 2;
    private const int RangedFrames = 3;

    [SerializeField] private Ease _chargeEase = Ease.InBack;
    [SerializeField] private float _chargeAloneOffset = .49f;
    [SerializeField] private float _chargeClashOffset = 0.3f;

    [Space]
    [SerializeField] private AudioClip _windUpClip;
    [SerializeField] private AudioClip _attackConnectClip;

    private Character _character;
    private ArrowBlur _arrowBlur;

    private void Awake()
    {
        _character = GetComponent<Character>();
        _arrowBlur = GetComponentInChildren<ArrowBlur>();
    }

    public IEnumerator GetSequence(Character other)
    {
        yield return _character.BeginMoveSequence(other);
        yield return GetAttackAloneSequence(other);
        if (InteractionResult.ShouldOtherCounter(_character, other, other.CurrentHealth))
        {
            yield return other.GetAttackAloneSequence(_character);
        }
        yield return _character.EndMoveSequence(other);
        RemoveDeadCombatantsFromBattlefield(other);
    }
    
    public IEnumerator GetAttackAloneSequence(Character other)
    {
        yield return GetMeleeSwipeSequence(other);

        _character.MoveToCellCenter();
        Coroutine otherHurtRoutine = StartCoroutine(other.GetHurtSequence(_character));
        while (_character.IsOneShotAnimationPlaying)
        {
            yield return null;
        }

        _character.PlayIdleAnimation();
        yield return otherHurtRoutine;
    }

    public IEnumerator GetMeleeSwipeSequence(Character other)
    {
        _character.PlayAttackAnimation();
        yield return ChargeSequence(other);
        if (_character.IsRanged)
        {
            _arrowBlur.Play(other);
        }
        yield return AttackConnectSequence(other);
    }

    private YieldInstruction ChargeSequence(Character other)
    {
        Vector2 cellPosition = _character.CurrentCellCenter;
        Vector2 towardsTarget = (other.Position - cellPosition).normalized;
        float chargeMagnitude = _character.IsRanged || other.IsRanged ? _chargeAloneOffset : _chargeClashOffset;
        float chargeDirection = _character.IsRanged ? -1 : 1;
        Vector2 chargeOffset = chargeDirection * chargeMagnitude * _character.CellSize * towardsTarget;
        Vector2 chargePosition = cellPosition + chargeOffset;

        EazySoundManager.PlaySound(_windUpClip);
        int frameCount = _character.IsRanged ? RangedFrames : MeleeFrames;
        float totalChargeDuration = frameCount * ApproxFrameDuration;
        return transform.DOMove(chargePosition, totalChargeDuration)
            .SetEase(_chargeEase)
            .WaitForCompletion();
    }

    private IEnumerator AttackConnectSequence(Character other)
    {
        EazySoundManager.PlaySound(_attackConnectClip);
        _character.SetAnimationSpeed(0);
        yield return other.PlayAttackConnectShake();
        _character.SetAnimationSpeed(1);
    }

    private void RemoveDeadCombatantsFromBattlefield(Character other)
    {
        if (other.ShouldRemoveFromBattlefield())
        {
            other.gameObject.SetActive(false);
        }
        if (_character.ShouldRemoveFromBattlefield())
        {
            _character.gameObject.SetActive(false);
        }
    }
}