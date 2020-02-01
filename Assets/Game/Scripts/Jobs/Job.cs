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

    public delegate void JobFinishDelegate();
    public abstract class JobBase
    {
        public event JobFinishDelegate OnJobFinished;

        private HashSet<Agent> _agents;
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

                    var finishedAgents = new List<Agent>();

                    foreach (var agent in _agents)
                    {
                        if (agent.State == AgentState.Finished)
                        {
                            finishedAgents.Add(agent);
                        }
                    }

                    foreach (var agent in finishedAgents)
                    {
                        _agents.Remove(agent);
                    }

                    if (_agents.Count == 0)
                    {
                        _state = JobState.Finished;
                        OnJobFinished?.Invoke();
                    }
                    break;
                case JobState.Designation:
                    DesignationUpdate();
                    break;
            }
        }

        public virtual void AssignAgent(Agent agent)
        {
            _agents.Add(agent);
            agent.AssignJob(this);
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