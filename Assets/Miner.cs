using CodeBase.SystemGame;
using Pathfinding;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Miner : MonoCache
{
    private Transform _target;
    [SerializeField] private float _oreAmount = 0;
    [SerializeField] private float _oreLimit = 10;
    [SerializeField] private float _damage = 1;

    [SerializeField] private AIDestinationSetter _destinationSetter;
    [SerializeField] private float _timeForAttack;
    [SerializeField] private Base _base;

    [SerializeField] private CircleCollider2D _circleCollider;

    public Ore TargetOre;

    private float _timeForDecay;
    public Transform Target
    {
        get
        {
            return _target;
        }

        set
        {
            _target = value;
            _destinationSetter.target = value.transform;
        }

    }

    private void OnEnable()
    {
        AddFixedUpdate();
    }


    private void Start()
    {        
        ObjectStorage.Instance.Miners.Add(this);
    }


    public override void OnFixedTick()
    {
        if (Target != null)
        {
            Vector3 directionToTarget = Target.transform.position - transform.position;

            if (directionToTarget.x > 0)
                transform.rotation = Quaternion.Euler(0, 180, 0);
            
            else
                transform.rotation = Quaternion.Euler(0, 0, 0);
            

            if(_oreAmount < _oreLimit && Vector2.Distance(transform.position, TargetOre.transform.position) <= 0.8f)
            {
                if (_timeForDecay <= 0)
                {
                    _oreAmount += TargetOre.TakeAmount(_damage);
                    _timeForDecay = _timeForAttack;

                    if(_oreAmount >= _oreLimit)
                    {
                        Target = _base.transform;
                    }
                }
                else
                    _timeForDecay -= Time.fixedDeltaTime;
            }


            if (Target == _base.transform && Vector2.Distance(transform.position, _base.transform.position) <= 1.4f)
            {
                if(_base.CurOre < _base.MaxOre)
                {
                    _oreAmount = _base.TakeOre(_oreAmount);
                    if(_oreAmount < _oreLimit)
                        Target = TargetOre.transform;
                }
            }
        }

        if (_collisionTimer > 0)
            _collisionTimer -= Time.fixedDeltaTime;

        if (_collisionTrueTimer > 0)
        {
            _collisionTrueTimer -= Time.fixedDeltaTime;
            if(_collisionTrueTimer <= 0)
            {
                _circleCollider.enabled = true;
                _collisionTimer = _maxCollisionTimer;
            }             
        }
    }

    [SerializeField] private float _collisionTimer;
    [SerializeField] private float _maxCollisionTimer;

    [SerializeField] private float _collisionTrueTimer;
    [SerializeField] private float _maxCollisionTrueTimer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Miner"))
        {
            if(_collisionTimer <= 0)
            {
                _circleCollider.enabled = false;
                _collisionTrueTimer = _maxCollisionTimer;
            }
        }
    }

}
