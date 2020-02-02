using System.Collections;
using UnityEngine;

namespace ggj20
{
    public class Flammable : MonoBehaviour
    {
        [SerializeField] private float _buildUpDuration;
        [SerializeField] private float _burnUpDuration;
        [SerializeField] private float _expandRate;
        [SerializeField] private CircleCollider2D _burnRadius;
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private float _currentBuildUp = 0;
        private bool _caughtFire = false;
        private float _burnTimer = 0;
        private bool _burnt = false;
        private bool _extinguished = false;

        public bool CaughtFire => _caughtFire;
        public bool Burnt => _burnt;
        public bool Extinguished => _extinguished;

        public float HeatPercentage
        {
            get
            {
                if(_caughtFire)
                {
                    return 1f - _burnTimer / _burnUpDuration;
                }
                else
                {
                    return _currentBuildUp / _buildUpDuration;
                }
            }
        }

        void Update()
        {
            if(_caughtFire)
            {
                _burnTimer += Time.deltaTime;

                _burnRadius.radius += _expandRate * Time.deltaTime;

                if(_burnTimer >= _burnUpDuration)
                {
                    FinishBurning();
                }

                var nearbyFlammables = PhysicsUtils.GetOverlappingFlammables(_burnRadius);

                foreach (var flammable in nearbyFlammables)
                {
                    flammable.HeatUp();
                }
            }

            if (!_burnt)
            {
                _spriteRenderer.color = new Color(1f, 1f - HeatPercentage, 1f - HeatPercentage);
            }
            else
            {
                _spriteRenderer.color = Color.black;
            }
        }

        public void HeatUp()
        {
            if (!_burnt && !_extinguished)
            {
                _currentBuildUp += Time.deltaTime;

                if (_currentBuildUp >= _buildUpDuration)
                {
                    _caughtFire = true;
                }
            }
        }

        public void Extinguish(float rate)
        {
            var effectiveTime = rate * Time.deltaTime;

            if(_caughtFire)
            {
                _burnUpDuration -= effectiveTime;
                if(_burnUpDuration <= 0)
                {
                    _burnUpDuration = 0;
                    _extinguished = true;
                    _caughtFire = false;
                }
            }
            else
            {
                _currentBuildUp -= effectiveTime;
                if(_currentBuildUp <= 0)
                {
                    _extinguished = true;
                    _currentBuildUp = 0;
                }
            }
        }


        [InspectorButton("CatchFire")]
        public bool fireStarter;
        
        public void CatchFire()
        {
            StartCoroutine(CatchFireRoutine());
        }

        private void FinishBurning()
        {
            _burnt = true;
            _caughtFire = false;
            if(_particles) _particles.Stop(true);
            StartCoroutine(DestroyRoutine(3f));
        }

        private IEnumerator CatchFireRoutine()
        {
            while(!_caughtFire)
            {
                HeatUp();
                yield return null;
            }
        }

        private IEnumerator DestroyRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            Destroy(gameObject);
        }
    }
}