using UnityEngine;

namespace ggj20
{
    public class SpriteRotator : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed;

        void Update()
        {
            var rot = transform.localRotation;

            rot *= Quaternion.AngleAxis(_rotationSpeed * Time.deltaTime, Vector3.forward);

            transform.localRotation = rot;
        }
    }
}