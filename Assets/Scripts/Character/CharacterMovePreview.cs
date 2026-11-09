using Hellmade.Sound;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterMovePreview : MonoBehaviour
{
    [SerializeField] private AudioClip _previewSound;
    private Character _character;
    private readonly List<Character> _activePreviews = new();

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void PreviewMove(Character other)
    {
        Clear();

        InteractionResult result = InteractionResult.Calculate(_character, other);
        _character.Preview(result.Interactor);
        other.Preview(result.Target);

        _activePreviews.Add(_character);
        _activePreviews.Add(other);

        EazySoundManager.PlaySound(_previewSound);
    }

    public void Clear()
    {
        for (int i = 0; i < _activePreviews.Count; i++)
        {
            Character character = _activePreviews[i];
            character.ClearPreview();
        }
        _activePreviews.Clear();
    }
}