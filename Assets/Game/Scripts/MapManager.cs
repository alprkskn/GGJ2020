using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ggj20
{
    public class MapManager : Singleton<MapManager>
    {
        [SerializeField] private Agent _agentPrefab;
        [SerializeField] private GameObject _targetPrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private int _maxAgents;
        [SerializeField] private float _agentSpawnInterval;
        [SerializeField] private Camera _mainCamera;

        private HashSet<Agent> _agentPool;
        private Coroutine _agentSpawnRoutine;
        private HashSet<JobBase> _ongoingJobs;

        public Camera MainCamera => _mainCamera;

        public Vector3 SpawnPoint => _spawnPoint.position;
        public GameObject TargetPrefab => _targetPrefab;

        void Start()
        {
            _agentPool = new HashSet<Agent>();
            _ongoingJobs = new HashSet<JobBase>();

            _agentSpawnRoutine = StartCoroutine(AgentSpawner());
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                var job = new AmeleJob();
                _ongoingJobs.Add(job);
            }

            if(Input.GetKeyDown(KeyCode.Return))
            {
                var agent = GetAvailableAgent();

                if(agent)
                {
                    var job = _ongoingJobs.FindItem(j => j is AmeleJob);

                    if(job != null)
                    {
                        job.AssignAgent(agent);
                    }
                    else
                    {
                        Debug.LogError("They took our jobs.");
                    }
                }
                else
                {
                    Debug.LogError("Amele kalmamis.");
                }
            }

            foreach(var job in _ongoingJobs)
            {
                job.Update();
            }
        }

        private IEnumerator AgentSpawner()
        {
            while(_agentPool.Count < _maxAgents)
            {
                SpawnAgent();
                yield return new WaitForSeconds(_agentSpawnInterval);
            }
        }

        private void SpawnAgent()
        {
            var agent = Instantiate(_agentPrefab, _spawnPoint.position, Quaternion.identity);

            _agentPool.Add(agent);
        }

        private Agent GetAvailableAgent()
        {
            var availableAgent = _agentPool.FindItem(x => x.State == AgentState.Idle);
            return availableAgent;
        }
    }
}