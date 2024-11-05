using UnityEngine;

namespace Game.UI.UIElements
{
    public class UICircle : UIElement
    {
        [SerializeField] private float _speed;
        
        private void Update()
        {
            transform.Rotate(0,0, 1 * _speed * Time.deltaTime);
        }
    }
}