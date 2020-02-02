using System.Collections.Generic;
using UnityEngine;

namespace ggj20
{
    public enum JobState
    {
        Designation,
        InProgress,
        Finished
    }

    public delegate void JobFinishDelegate(JobBase job);
    public abstract class JobBase
    {
        public event JobFinishDelegate OnJobFinished;

        protected HashSet<Agent> _agents;
        protected JobState _state;

        public abstract IJobExecutor GetJobExecutor();

        public JobBase()
        {
            _agents = new HashSet<Agent>();
            _state = JobState.Designation;
            InitializeJob();
        }

        public void Update()
        {
            switch(_state)
            {
                case JobState.InProgress:
                    ProgressUpdate();
                    break;
                    var finishedAgents = new List<Agent>();
                case JobState.Designation:
                    DesignationUpdate();
                    break;
                case JobState.Finished:
                    OnJobFinished?.Invoke(this);
                    break;
            }
        }

        public virtual void AssignAgent(Agent agent)
        {
            _agents.Add(agent);
            agent.AssignJob(this);
        }

        protected virtual void FinishJob()
        {
            foreach(var agent in _agents)
            {
                agent.TerminateJob();
            }

            _state = JobState.Finished;
        }

        protected virtual void InitializeJob()
        {

        }

        protected virtual void DesignationUpdate()
        {

        }

        protected virtual void ProgressUpdate()
        {
        }
    }
}