using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Whaledevelop.UI;

namespace Sopka
{
    public class PuzzleUIView : UIView<PuzzleUIViewModel>
    {
        [SerializeField]
        private RectTransform _pieceParent;

        [SerializeField]
        private Image _hintImage;

        [SerializeField]
        private PuzzlePieceView _piecePrefab;

        private readonly List<PuzzlePieceView> _pieces = new();
        
        public override void Initialize()
        {
            _pieces.Clear();
            _hintImage.sprite = DerivedModel.HintSprite;

            foreach (Transform child in _pieceParent)
            {
                Destroy(child.gameObject);
            }

            foreach (var data in DerivedModel.Pieces)
            {
                var piece = Instantiate(_piecePrefab, _pieceParent);
                var rect = (RectTransform)piece.transform;

                rect.sizeDelta = DerivedModel.PuzzleSize;
                rect.anchoredPosition = data.AnchoredPosition;

                piece.Setup(DerivedModel.OnClick, data.Sprite, data.PuzzleIndex, data.SpriteIndex);
                _pieces.Add(piece);
            }
            DerivedModel.OnCreated(_pieces.ToArray());
        }
    }
}