using System;
using Player.ActionHandlers;
using UnityEngine;


namespace Connection
{
    public class ColorNode : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color color;
        [SerializeField] private bool isEmpty;
        [SerializeField] private Sprite emptySprite;
        [SerializeField] private Sprite filledSprite;
        
        public event Action<Color> ColorChangedEvent;
        
        public Vector2 CenterPosition => transform.position;
        public Color Color => spriteRenderer.color;
        public bool IsEmpty => isEmpty;

        public bool IsInBounds(Vector3 point)
        {
            Vector3 localPos = transform.InverseTransformPoint(point);
            if (Mathf.Abs(localPos.x) < 0.5f && Mathf.Abs(localPos.y) < 0.5f && Mathf.Abs(localPos.z) < 0.5f)
                return true;
            else
                return false;
        }

        public void AddColor(Color additiveColor)
        {
            if (isEmpty)
            {
                spriteRenderer.sprite = filledSprite;
                SetColor(additiveColor);
                
                isEmpty = false;
            }
            else
            {
                SetColor(Color.Lerp(spriteRenderer.color, additiveColor, .5f));
            }
        }

        public void SetColor(Color newColor)
        {
            spriteRenderer.color = newColor;
            ColorChangedEvent?.Invoke(spriteRenderer.color);
        }

        public void SetEmpty(bool isEmpty)
        {
            this.isEmpty = isEmpty;
            
            if (isEmpty)
            {
                spriteRenderer.sprite = emptySprite;
                spriteRenderer.color = Color.white;
            }
        }
        

        private void OnValidate()
        {
            if (isEmpty)
            {
                spriteRenderer.sprite = emptySprite;
                spriteRenderer.color = Color.white;
            }
            else
            {
                spriteRenderer.sprite = filledSprite;
                spriteRenderer.color = color;
            }
        }
    }
}
