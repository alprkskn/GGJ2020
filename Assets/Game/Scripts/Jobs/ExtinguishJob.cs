using System;
using UnityEngine;

namespace ggj20
{
    public class ExtinguishJob : JobBase
    {
        public class Executor : IJobExecutor
        {
            private Vector2 _target;
            private float _targetRadius;

            public event Action JobFinished;

            public bool IsJobFinished()
            {
                throw new NotImplementedException();
            }

            public void JobUpdate()
            {
                throw new NotImplementedException();
            }

            public void StartJob(Agent agent)
            {
                throw new NotImplementedException();
            }
        }

        public override IJobExecutor GetJobExecutor()
        {
            throw new System.NotImplementedException();
        }
    }
}