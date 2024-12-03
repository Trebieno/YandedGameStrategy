using CodeBase.SystemGame;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


namespace CodeBase
{
    public class Miner : MonoCache
    {
        private Transform _target;
        [SerializeField] private float _oreAmount = 0;
        [SerializeField] private float _oreLimit = 10;
        [SerializeField] private float _damage = 1;

        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _timeForAttack;

        [SerializeField] private CircleCollider2D _circleCollider;

        [SerializeField] private Animation _animation;

        public Ore TargetOre;

        private float _timeForDecay;
        private Base _base;

        public Transform Target
        {
            get
            {
                return _target;
            }

            set
            {
                _target = value;
                _agent.SetDestination(value.transform.position);                
            }

        }

        private void OnEnable()
        {
            AddFixedUpdate();
        }


        private void Start()
        {        
            ObjectStorage.Instance.Miners.Add(this);
            _base = ObjectStorage.Instance.Base;
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
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
            

                if(_oreAmount < _oreLimit && TargetOre != null && Vector2.Distance(transform.position, TargetOre.transform.position) <= 0.8f)
                {
                    if (_timeForDecay <= 0)
                    {
                        if (_animation.isPlaying)
                            _animation.Stop();
                        _oreAmount += TargetOre.TakeAmount(_damage);                        
                        _timeForDecay = _timeForAttack;

                        if(_oreAmount >= _oreLimit)
                        {
                            Target = _base.transform;
                        }
                    }
                    else
                    {
                        if (!_animation.isPlaying)
                            _animation.Play();
                        
                        _timeForDecay -= Time.fixedDeltaTime;
                    }
                }

                else if (TargetOre == null && Target != _base.transform)
                {
                    ObjectStorage.Instance.MinerLogistics.FindOre(this);
                }


                if (Target == _base.transform && Vector2.Distance(transform.position, _base.transform.position) <= 1.4f)
                {
                    if(_base.CurOre < _base.MaxOre)
                    {
                        _oreAmount = _base.TakeOre(_oreAmount);
                        if(_oreAmount < _oreLimit && TargetOre != null)
                            Target = TargetOre.transform;
                        else
                            ObjectStorage.Instance.MinerLogistics.FindOre(this);
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
}
