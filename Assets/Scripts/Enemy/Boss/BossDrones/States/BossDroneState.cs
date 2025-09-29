using UnityEngine;

public enum BossDroneState
{
    Idle,
    MoveToReadyPos,
    MoveToShootingPos,
    Shooting,
    GoBackToBossPos,
    Hurt,
    Die
}
