using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SlimeMoveController : MonoBehaviour, IDamageable
{
    public enum State
    {
        None,
        Idle,
        Chase,
        Attack,
        Dizzy,
        Die
    }

    [SerializeField]
    private NavMeshAgent _navMesh = default;

    [SerializeField]
    private Animator _animator = default;

    [SerializeField]
    private PlayerMoveController _player = default;

    [SerializeField]
    private Collider _attackCollider = default;

    [SerializeField]
    private float _chaseStartDistance = default;

    private State _currentState = State.None;

    private void Update()
    {

    }

    public void AddDamage()
    {

    }

    private void ChengeState(State next)
    {

    }

    private void StateUpdate()
    {
        
    }

    private void Walk()
    {

    }
}
