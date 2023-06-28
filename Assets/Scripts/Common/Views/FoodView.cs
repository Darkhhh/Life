using UnityEngine;

namespace Common.Views
{
    public class FoodView : MonoBehaviour
    {
        private Transform _objectPosition;
        
        private void Awake() => _objectPosition = transform;

        public Vector3 Coordinates => _objectPosition.position;
        
        public void ChangeLocalScale(Vector3 newScale)
        {
            _objectPosition.localScale = newScale;
        }
    }
}
