using UnityEngine;

public readonly struct InteractionResult
{
    public readonly CharacterInfo Interactor;
    public readonly CharacterInfo Target;

    public InteractionResult(CharacterInfo interactor, CharacterInfo target)
    {
        Interactor = interactor;
        Target = target;
    }

    public static InteractionResult Calculate(Character interactor, Character target)
    {
        if (interactor.Faction == target.Faction)
        {
            return CalculateHealResult(interactor, target);
        }
        else
        {
            return CalculateAttackResult(interactor, target);
        }
    }

    private static InteractionResult CalculateHealResult(Character interactor, Character target)
    {
        CharacterInfo selfInfo = new(interactor.CurrentHealth, interactor.CurrentEnergy - 1);

        int otherHealth = target.BaseHealth;
        int otherEnergy = target.IsDead ? target.CurrentEnergy - 1 : target.CurrentEnergy;
        CharacterInfo otherInfo = new(otherHealth, otherEnergy);

        return new(selfInfo, otherInfo);
    }

    private static InteractionResult CalculateAttackResult(Character interactor, Character target)
    {
        int otherHealth = CalculateHealthAfterHit(target.CurrentHealth, interactor.CurrentPower);
        CharacterInfo targetInfo = new(otherHealth, target.CurrentEnergy);

        int selfHealth;
        if (ShouldOtherCounter(interactor, target, otherHealth))
        {
            selfHealth = CalculateHealthAfterHit(interactor.CurrentHealth, target.CurrentPower);
        }
        else
        {
            selfHealth = interactor.CurrentHealth;
        }
        CharacterInfo selfInfo = new(selfHealth, interactor.CurrentEnergy - 1);

        return new(selfInfo, targetInfo);
    }

    public static bool ShouldOtherCounter(Character interactor, Character target, int targetHealthAfterHit)
    {
        return !interactor.IsRanged && !target.IsRanged && targetHealthAfterHit > 0;
    }

    private  static int CalculateHealthAfterHit(int health, int power)
    {
        return Mathf.Max(0, health - power);
    }
}