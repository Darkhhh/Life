using UnityEngine;

namespace Common.Views
{
    public class BeingView : MonoBehaviour
    {
        public Transform objectPosition;
        private SpriteRenderer _spriteRenderer;

        public Vector3 Coordinates => objectPosition.position;
        
        private void Awake()
        {
            objectPosition = transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void ChangeSexVisualisation(Sex val)
        {
            _spriteRenderer.color = val == Sex.Male ? Color.blue : Color.magenta;
        }

        public void ChangeLocalScale(Vector3 newScale)
        {
            objectPosition.localScale = newScale;
        }

        public void Move(Vector3 newPosition)
        {
            objectPosition.position = newPosition;
        }
    }
}
