using System;
using Pathfinding;
using UnityEngine;

namespace ggj20
{
    public enum AgentState
    {
        Working,
        Finished,
        Returning,
        Idle
    }

    public enum AgentRole
    {
        Fireman,
        Builder
    }

    public delegate void StateChangeDelegate(AgentState newState);

    public class Agent : MonoBehaviour
    {
        public event StateChangeDelegate OnStateChanged;
        [SerializeField] private Seeker _seeker;
        [SerializeField] private CustomAIPath _aiPath;
        [SerializeField] private AIDestinationSetter _destinationSetter;
        [SerializeField] private Animator _animController;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private RuntimeAnimatorController _firemanAnimator;
        [SerializeField] private RuntimeAnimatorController _builderAnimator;

        [SerializeField] private float _extinguishRate;
        [SerializeField] private float _buildRate;

        private AgentRole _role;
        private JobBase _currentJob;
        private IJobExecutor _jobExecutor;
        private AgentState _state = AgentState.Idle;

        public AgentState State
        {
            get => _state;
            set
            {
                _state = value;
                OnStateChanged?.Invoke(_state);
            }
        }

        public AgentRole Role
        {
            get => _role;
            set
            {
                _role = value;

                switch (_role)
                {
                    case AgentRole.Fireman:
                        _animController.runtimeAnimatorController = _firemanAnimator;
                        break;

                    case AgentRole.Builder:
                        _animController.runtimeAnimatorController = _builderAnimator;
                        break;
                }
            }
        }

        public Seeker Seeker => _seeker;

        public float ExtinguishRate => _extinguishRate;
        public float BuildRate => _buildRate;
        
        public void AssignJob(JobBase job)
        {
            _currentJob = job;
            State = AgentState.Working;

            _jobExecutor = job.GetJobExecutor();
            _jobExecutor.StartJob(this);

            _jobExecutor.JobFinished += OnJobFinished;
        }

        public void TerminateJob()
        {
            OnJobFinished();
        }

        public void SetDestination(Vector3 dest, Action callback)
        {
            _aiPath.enabled = true;

            void callbackWrapper()
            {
                callback?.Invoke();
                _aiPath.OnDestinationReached -= callbackWrapper;
            }

            _aiPath.OnDestinationReached += callbackWrapper;

            _destinationSetter.SetDestinationOneShot(dest);
        }

        public void StartExtinguishing(Flammable f)
        {

        }

        public void TerminateMovement()
        {
            _aiPath.Terminate();
        }

        private void OnJobFinished()
        {
            State = AgentState.Returning;

            _destinationSetter.SetDestinationOneShot(MapManager.Instance.SpawnPoint);
            _aiPath.OnDestinationReached += OnReturnedToBase;
            _currentJob = null;
            _jobExecutor = null;
        }

        private void OnReturnedToBase()
        {
            State = AgentState.Idle;
            _aiPath.Terminate();
        }

        private void Update()
        {
            if(_jobExecutor != null)
            {
                _jobExecutor.JobUpdate();
            }

            var velocity = _aiPath.velocity;
            _animController.SetFloat("speed" ,velocity.magnitude);

            _spriteRenderer.flipX = (velocity.x > 0);
        }
    }
}