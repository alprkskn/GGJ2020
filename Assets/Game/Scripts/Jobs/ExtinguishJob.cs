using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ggj20
{
    public class ExtinguishJob : JobBase
    {
        public class Executor : IJobExecutor
        {
            private ExtinguishJob _job;
            private Agent _agent;
            private Flammable _currentTargetFlammable;
            private bool _reachedDestination;

            public event Action<Flammable> JobFinished;

            public Executor(ExtinguishJob job)
            {
                _job = job;
            }

            event Action IJobExecutor.JobFinished
            {
                add
                {
                    throw new NotImplementedException();
                }

                remove
                {
                    throw new NotImplementedException();
                }
            }

            public bool IsJobFinished()
            {
                throw new NotImplementedException();
            }

            public void JobUpdate()
            {
                if(_reachedDestination)
                {
                    _currentTargetFlammable.Extinguish(_agent.ExtinguishRate);

                    if(_currentTargetFlammable.Extinguished)
                    {
                        JobFinished?.Invoke(_currentTargetFlammable);
                        if(!AssignNewFlammable())
                        {
                            Debug.Log("No flammables remain in the area.");
                        }
                    }
                }
            }

            public void StartJob(Agent agent)
            {
                _agent = agent;


                throw new NotImplementedException();
            }

            private bool AssignNewFlammable()
            {
                var f = _job.GetClosestFlammable(_agent.transform.position);

                if(f)
                {
                    var d = f.transform.position - _agent.transform.position;
                    d.Normalize();

                    var p = f.transform.position - d;
                    _reachedDestination = false;

                    _agent.SetDestination(p, () =>
                    {
                        _reachedDestination = true;
                        _agent.StartExtinguishing(f);
                    });
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private GameObject _jobTarget;
        private Collider2D _targetCollider;
        private Vector2 _targetPosition;

        private List<Flammable> _flammables;
        public override IJobExecutor GetJobExecutor()
        {
            var e = new Executor(this);
            e.JobFinished += OnWorkerFinishedJob;
            throw new System.NotImplementedException();
        }

        private void OnWorkerFinishedJob(Flammable obj)
        {
            _flammables.Remove(obj);

            if(_flammables.Count == 0)
            {
                FinishJob();
            }
        }

        public Flammable GetClosestFlammable(Vector2 p)
        {
            var minDist = float.MaxValue;
            Flammable selected = null;

            foreach(var f in _flammables)
            {
                var d = Vector2.Distance(p, f.transform.position);

                if(d < minDist)
                {
                    minDist = d;
                    selected = f;
                }
            }
            return selected;
        }

        protected override void InitializeJob()
        {
            _jobTarget = GameObject.Instantiate(MapManager.Instance.TargetPrefab);
            _targetCollider = _jobTarget.GetComponent<Collider2D>();
        }

        protected override void DesignationUpdate()
        {
            var point = MapManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);

            _jobTarget.transform.position = new Vector3(point.x, point.y, -10);

            if (Input.GetMouseButtonDown(0))
            {
                _state = JobState.InProgress;
                _targetPosition = _jobTarget.transform.position;
            }
        }

        protected override void ProgressUpdate()
        {
            _flammables = PhysicsUtils.GetOverlappingFlammables(_targetCollider);
            _flammables = _flammables.Where(x => x.CaughtFire).ToList();
        }
    }
}