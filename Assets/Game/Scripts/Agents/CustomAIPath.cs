using System;
using Pathfinding;

namespace ggj20
{
    public class CustomAIPath : AIPath
    {
        public event Action OnDestinationReached;
        private bool _pathCompleted;

        public override void OnTargetReached() 
        {
            if (!_pathCompleted)
            {
                OnDestinationReached?.Invoke();
                _pathCompleted = true;
            }
        }

        protected override void OnPathComplete(Path newPath)
        {
            base.OnPathComplete(newPath);
            _pathCompleted = false;
        }

        public void Terminate()
        {
            OnDestinationReached = null;
            this.enabled = false;
            _pathCompleted = true;
        }
    }
}