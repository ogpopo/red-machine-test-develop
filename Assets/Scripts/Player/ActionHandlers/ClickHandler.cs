using System;
using Camera;
using UnityEngine;
using Utils.Singleton;


namespace Player.ActionHandlers
{
    public class ClickHandler : DontDestroyMonoBehaviourSingleton<ClickHandler>
    {
        [SerializeField] private float clickToDragDuration;

        public event Action<Vector3> PointerDownEvent;
        public event Action<Vector3> ClickEvent;
        public event Action<Vector3> PointerUpEvent;
        public event Action<Vector3> DragStartEvent;
        public event Action<Vector3> DragEvent;
        public event Action<Vector3> DragEndEvent;

        private Vector3 _pointerDownPosition;
        private Vector3 _pointerDrugPosition;

        private bool _isClick;
        private bool _isDrag;
        private float _clickHoldDuration;


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isClick = true;
                _clickHoldDuration = .0f;

                _pointerDownPosition = CameraHolder.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
                
                PointerDownEvent?.Invoke(_pointerDownPosition);
                
                _pointerDownPosition = new Vector3(_pointerDownPosition.x, _pointerDownPosition.y, .0f);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                var pointerUpPosition = CameraHolder.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
                    
                if (_isDrag)
                {
                    DragEndEvent?.Invoke(pointerUpPosition);

                    _isDrag = false;
                }
                else
                {
                    ClickEvent?.Invoke(pointerUpPosition);
                }
                
                PointerUpEvent?.Invoke(pointerUpPosition);

                _isClick = false;
            }
            
            if (Input.GetMouseButton(0))
            {
                _pointerDrugPosition = CameraHolder.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
                _pointerDrugPosition = new Vector3(_pointerDrugPosition.x, _pointerDrugPosition.y, .0f);
                
                DragEvent?.Invoke(_pointerDrugPosition);
            }
        }

        private void LateUpdate()
        {
            if (!_isClick)
                return;

            _clickHoldDuration += Time.deltaTime;
            if (_clickHoldDuration >= clickToDragDuration)
            {
                DragStartEvent?.Invoke(_pointerDownPosition);

                _isClick = false;
                _isDrag = true;
            }
        }
    }
}