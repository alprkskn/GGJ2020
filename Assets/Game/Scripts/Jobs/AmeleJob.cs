using System;
using UnityEngine;

namespace ggj20
{
    public class AmeleJob : JobBase
    {
        public class Executor : IJobExecutor
        {
            public event Action JobFinished;

            private Agent _agent;
            private Vector3 _target;
            private bool _reachedTarget;
            public Executor(Vector3 target)
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
                _agent.SetDestination(_target, () =>
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

        private GameObject _jobTarget;
        private Vector3 _targetPosition;
        private int _workersNeeded = 3;

        protected override void InitializeJob()
        {
            _jobTarget = GameObject.Instantiate(MapManager.Instance.TargetPrefab);
        }

        public override IJobExecutor GetJobExecutor()
        {
            var executor = new Executor(_targetPosition);
            executor.JobFinished += OnWorkerFinishedJob;
            return executor;
        }

        private void OnWorkerFinishedJob()
        {
            _workersNeeded--;

            if(_workersNeeded == 0)
            {
                FinishJob();
            }
        }

        protected override void FinishJob()
        {
            base.FinishJob();
            GameObject.Destroy(_jobTarget);
        }

        protected override void DesignationUpdate()
        {
            var point = MapManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);

            _jobTarget.transform.position = new Vector3(point.x, point.y, -10);

            if(Input.GetMouseButtonDown(0))
            {
                _state = JobState.InProgress;
                _targetPosition = _jobTarget.transform.position;
            }
        }
    }
}