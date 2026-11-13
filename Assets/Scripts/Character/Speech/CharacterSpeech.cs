using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterSpeech : MonoBehaviour
{
    private CharacterSpeechBubble _bubble;

    private void Awake()
    {
        _bubble = GetComponentInChildren<CharacterSpeechBubble>(true);
    }

    public Coroutine Say(string text)
    {
        CoroutineRunner.StopAll(this);
        return CoroutineRunner.Play(this, _bubble.GetDisplaySequence(text));
    }
}