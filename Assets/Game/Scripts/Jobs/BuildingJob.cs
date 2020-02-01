using System;

namespace ggj20
{
    public class BuildingJob : JobBase
    {
        public class Executor : IJobExecutor
        {
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
            throw new NotImplementedException();
        }

    }
}