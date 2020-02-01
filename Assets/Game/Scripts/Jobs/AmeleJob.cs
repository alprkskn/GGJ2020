using System;
using UnityEngine;

namespace ggj20
{
    public class AmeleJob : JobBase
    {
        private GameObject _jobTarget;

        public class Executor : IJobExecutor
        {
            public event Action JobFinished;

            private Agent _agent;
            private Transform _target;
            private bool _reachedTarget;
            public Executor(Transform target)
            {
                _target = target;
            }

            public bool IsJobFinished()
            {
                return _reachedTarget;
            }

            public void StartJob(Agent agent)
            {
                _agent = agent;
                Debug.LogError("Amele job Start");
                _agent.SetDestination(_target.position, () =>
                {
                    _reachedTarget = true;
                    Debug.Log("Geldim AMK.");
                    JobFinished?.Invoke();
                });
            }

            public void JobUpdate()
            {
            }
        }
        
        protected override void InitializeJob()
        {
            _jobTarget = GameObject.Instantiate(MapManager.Instance.TargetPrefab);
        }

        public override IJobExecutor GetJobExecutor()
        {
            return new Executor(_jobTarget.transform);
        }

        protected override void DesignationUpdate()
        {
            var point = MapManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);

            _jobTarget.transform.position = new Vector3(point.x, point.y, -10);

            if(Input.GetMouseButtonDown(0))
            {
                _state = JobState.InProgress;
            }
        }
    }
}