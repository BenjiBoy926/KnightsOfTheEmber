using UnityEngine;

[RequireComponent(typeof(Battle))]
public class BattleMusic : MonoBehaviour
{
    public static bool Mute { get; private set; } = false;

    [SerializeField] private Music _playerTurn;
    [SerializeField] private Music _enemyTurn;
    [SerializeField] private bool _muteThisBattle;
    private Battle _battle;
    private Music _current;

    private void Awake()
    {
        _battle = GetComponent<Battle>();
        Mute = _muteThisBattle && Application.isEditor;
    }

    private void Update()
    {
        SetCurrentMusic(CalculateCorrectMusic());
    }

    private Music CalculateCorrectMusic()
    {
        return ShouldBeSilent() ? null :
            _battle.CurrentFactionTurn == _battle.PlayerFaction ? _playerTurn : _enemyTurn;
    }

    private bool ShouldBeSilent()
    {
        return _battle.IsTurnChangeAnimationPlaying || !_battle.IsInProgress;
    }

    private void SetCurrentMusic(Music music)
    {
        if (_current == music)
            return;

        if (_current)
        {
            _current.Stop();
        }
        _current = music;
        if (_current)
        {
            _current.Play();
        }
    }
}