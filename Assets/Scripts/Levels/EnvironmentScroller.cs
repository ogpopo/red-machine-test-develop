using DG.Tweening;
using Player;
using Player.ActionHandlers;
using UnityEngine;
using UnityEngine.UI;

namespace Levels
{
    public class EnvironmentScroller : MonoBehaviour
    {
        [SerializeField] private NodesHandler nodesHandler;

        [SerializeField] private Button CentralizeButton;

        private Vector3 _originPosition;
        private Vector3 _startDrugPosition;

        private ClickHandler _clickHandler;

        private bool _isScrolled;

        private void OnEnable()
        {
            _clickHandler = ClickHandler.Instance;
            
            _clickHandler.DragEvent += OnDrug;
            _clickHandler.DragEndEvent += OnDragEnd;
            
            CentralizeButton.onClick.AddListener(Centralize);
        }

        private void OnDisable()
        {
            _clickHandler.DragEvent -= OnDrug;
            _clickHandler.DragEndEvent -= OnDragEnd;
            
            CentralizeButton.onClick.RemoveListener(Centralize);
        } 
        
        private void OnDragEnd(Vector3 position)
        {
            _isScrolled = false;
        }

        private void OnDrug(Vector3 position)
        {
            if (PlayerController.PlayerState != PlayerState.Scrolling)
            {
                return;
            }
            
            
            if (_isScrolled == false)
            {
                _startDrugPosition = position;
                _originPosition = nodesHandler.transform.position;
            }
            
            _isScrolled = true;

            nodesHandler.transform.DOMove(_originPosition + (position - _startDrugPosition), .3f);
        }
        
        private void Centralize()
        {
            nodesHandler.transform.DOKill();
            nodesHandler.transform.DOMove(Vector3.zero, .6f);
        }
    }
}