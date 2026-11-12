using System.Collections;
using UnityEngine;
using static UniqueCharacters;

namespace VerticalSlice
{
    public class BattleDialogue_FirstReviveFinish : BattleSequence
    {
        public override TriggerType Trigger => TriggerType.MoveEnd;

        public override bool ShouldPlay(Character interactor, Character target)
        {
            return !HasPlayed && target && interactor.Faction == target.Faction && target.GetLastRecordedState().Health <= 0;
        }

        protected override IEnumerator GetSequence(Character interactor, Character target)
        {
            yield return Dialogue.FirstMatchingMainCharacterSay(interactor,
                "Hey! Are you okay?",
                "Are you alright?",
                "Woah! Are you okay?!",
                "Oh? You're up? Are you alright?");

            yield return Dialogue.FirstMatchingMainCharacterSay(target,
                "Yeah, I think so. I'm pretty exhausted, but I'm not in any pain.",
                "...I think so. I feel exhausted, but I'm not in any pain.",
                "I feel amazing! Although, *yawn, I could really use a nap...",
                "...Yes. I'm tired but my wounds are all healed.");

            if (target == Alfred)
            {
                yield return interactor.Say("Alfred, Focus!");
                yield return target.Say("Right!");
            }

            yield return Dialogue.FirstMatchingMainCharacterSay(target,
                "Was this the Wisp's doing? What happened?",
                "What happened?",
                "Sooooo, what happened?!",
                "What happened?");

            yield return Dialogue.FirstMatchingMainCharacterSay(interactor,
                "It was the Wisp's doing. Somehow it used me to heal you.",
                "I'm not sure. Somehow the Wisp used me to heal you.",
                "Beats me! I felt the Wisp's energy coursing through me, and suddenly you were okay!",
                "I think the Wisp used me to heal you.");

            yield return Dialogue.FirstMatchingMainCharacterSay(target,
                "So it WAS the Wisp!",
                "The Wisp? Remarkable...",
                "So it was the Wisp, huh?",
                "The Wisp? Well... that should help to even the odds.");

            yield return Dialogue.FirstMatchingMainCharacterSay(target,
                "We can win now, I'm sure of it! Arkalux will uphold us with his Wisp!",
                "Using this power, we can work together and cut down this horde!",
                "We can use this power to break through their ranks! Let's go!",
                "Let's use this power to overcome these monsters.");
        }
    }
}