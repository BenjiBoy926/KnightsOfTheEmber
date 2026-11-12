using System.Collections;

namespace VerticalSlice
{
    public class BattleDialogue_FirstReviveStart : BattleSequence
    {
        public override TriggerType Trigger => TriggerType.MoveStart;

        public override bool ShouldPlay(Character interactor, Character target)
        {
            return !HasPlayed && target && interactor.Faction == target.Faction && target.IsDead;
        }

        protected override IEnumerator GetSequence(Character interactor, Character target)
        {
            yield return Dialogue.FirstMatchingMainCharacterSay(interactor,
                $"Huh? The Wisp... it's leading me over to {target.Name}",
                $"Strange. The Wisp is leading me over to {target.Name}?",
                $"What the heck? Why is the Wisp leading me over to {target.Name}?",
                $"Hm? The Wisp. It's leading me over to {target.Name}.");
        }
    }
}