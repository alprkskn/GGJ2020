using UnityEngine;

namespace ggj20
{
    public class SpriteLayerOrderer : MonoBehaviour
    {
        private SpriteRenderer _renderer;

        private float _bottomY;
        // Start is called before the first frame update
        void Start()
        {
            var bottomLeft = MapManager.Instance.MainCamera.ViewportToWorldPoint(new Vector3(0, 0));
            _bottomY = bottomLeft.y;
            _renderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            var bottom = _renderer.bounds.min.y;
            var d = -Mathf.Abs(bottom - _bottomY) * 100f;
            _renderer.sortingOrder = (int)d;
        }
    }
}