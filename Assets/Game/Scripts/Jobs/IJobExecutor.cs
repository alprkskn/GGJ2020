using System;

namespace ggj20
{
    public interface IJobExecutor
    {
        event Action JobFinished;
        void StartJob(Agent agent);
        void JobUpdate();
        bool IsJobFinished();
    }
}