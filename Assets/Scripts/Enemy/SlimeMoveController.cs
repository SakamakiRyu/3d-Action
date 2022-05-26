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
        switch (next)
        {
            case State.Idle:
                {

                }
                break;
            case State.Chase:
                {
                    _navMesh.SetDestination(_player.transform.position);
                }
                break;
            case State.Attack:
                { }
                break;
            case State.Dizzy:
                { }
                break;
            case State.Die:
                { }
                break;
        }

        _currentState = next;
    }

    private void StateUpdate()
    {
        var distance = Vector3.Distance(this.transform.position, _player.transform.position);
        if (distance < _chaseStartDistance) ChengeState(State.Chase);
    }

    private void Walk()
    {

    }
}
