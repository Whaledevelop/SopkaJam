using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sopka
{
    public class PuzzlePieceView : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private Image _selectionFrame;

        [SerializeField]
        private Button _button;

        private Action<PuzzlePieceView> _onClicked;

        public Sprite Sprite => _image.sprite;

        public int PieceIndex { get; private set; }
        
        public int SpriteIndex { get; private set; }

        public void Setup(Action<PuzzlePieceView> onClicked, Sprite sprite, int pieceIndex, int spriteIndex)
        {
            _onClicked = onClicked;

            PieceIndex = pieceIndex;
            
            SetSprite(sprite, spriteIndex);

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => _onClicked?.Invoke(this));

            SetSelected(false);
        }

        public void SetSprite(Sprite sprite, int spriteIndex)
        {
            _image.sprite = sprite;
            SpriteIndex = spriteIndex;
        }

        public void SetSelected(bool selected)
        {
            _selectionFrame.enabled = selected;
        }
    }
}