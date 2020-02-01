using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ggj20
{
    public class Flammable : MonoBehaviour
    {
        [SerializeField] private float _buildUpDuration;
        [SerializeField] private float _burnUpDuration;
        [SerializeField] private float _expandRate;
        [SerializeField] private CircleCollider2D _burnRadius;

        private float _currentBuildUp = 0;
        private bool _caughtFire = false;
        private float _burnTimer = 0;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(_caughtFire)
            {
                _burnTimer += Time.deltaTime;

            }
        }

        public void HeatUp()
        {
            _currentBuildUp += Time.deltaTime;

            if(_currentBuildUp >= _buildUpDuration)
            {
                _caughtFire = true;
            }
        }
    }
}