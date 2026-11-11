using System.Collections;
using static UniqueCharacters;

namespace VerticalSlice
{
    public class BattleDialogue_FirstHeal : BattleDialogueSequence
    {
        public override TriggerType Trigger => TriggerType.MoveEnd;

        private BattleDialogue_FirstReviveFinish _reviveFinish;

        private void Awake()
        {
            _reviveFinish = GetComponent<BattleDialogue_FirstReviveFinish>();
        }

        public override bool ShouldPlay(Character interactor, Character target)
        {
            bool didHeal = target && interactor.Faction == target.Faction;
            bool didRevive = target && target.GetLastRecordedState().Health <= 0;
            bool hasReviveDialoguePlayed = _reviveFinish && _reviveFinish.HasPlayed;
            return !HasPlayed && didHeal && !didRevive && !hasReviveDialoguePlayed;
        }

        protected override IEnumerator GetSequence(Character interactor, Character target)
        {
            yield return Dialogue.FirstMatchingMainCharacterSay(target,
                "Wow, I feel great! What just happened?",
                "Huh? That was refreshing. What was that?",
                "Woah! Woah! What? I feel like new! What just happened?",
                "Hm? My wounds are all healed. What was that?");

            yield return Dialogue.FirstMatchingMainCharacterSay(interactor,
                "I think the Wisp just used me to heal you!",
                "Not exactly sure, but I think the Wisp used me to heal you.",
                "No idea! I was just following the Wisp, then suddenly I felt energy race through me and into you.",
                "...It seems like the Wisp used me to heal you.");

            yield return Dialogue.FirstMatchingMainCharacterSay(target,
                "What an incredible power! We can use this to beat back the goblins!",
                "Interesting. Working together with this power, we should be able to get the upper hand.",
                "AMAZING! Now we can charge ahead without worrying about taking damage!",
                "This should give us a tactical advantage. We can use this power to beat the goblins and get out of here.");

            if (target == Alfred)
            {
                yield return interactor.Say("(So reckless...)");
            }
        }
    }
}