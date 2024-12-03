using UnityEngine;
using UnityEngine.AI;

public class TesAgent : MonoBehaviour
{
    private NavMeshAgent _agent;
    [SerializeField] private Transform _target;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.SetDestination(_target.position);
    }
}
