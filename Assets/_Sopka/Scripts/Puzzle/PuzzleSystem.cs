using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop;
using Whaledevelop.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/Systems/PuzzleSystem", fileName = "PuzzleSystem")]
    public class PuzzleSystem : GameSystem
    {
        [SerializeField]
        private PuzzleUIView _puzzleUIViewPrefab;

        [SerializeField]
        private Vector2Int _gridSize = new(4, 4);

        [SerializeField]
        private float _spacing = 16f;

        [SerializeField]
        private Vector2 _pieceSize = new(100f, 100f);

        private List<PuzzlePieceView> _pieces = new();

        private PuzzlePieceView _selected;

        [Inject] private IPuzzleModel _puzzleModel;

        [Inject] private IUIService _uiService;

        private PuzzleUIViewModel _puzzleUIViewModel;

        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            var total = _gridSize.x * _gridSize.y;

            var spriteList = _puzzleModel.PuzzleSprites.ToList();
            var shuffledSprites = spriteList.OrderBy(_ => Random.value).ToArray();

            var fullWidth = _gridSize.x * _pieceSize.x + (_gridSize.x - 1) * _spacing;
            var fullHeight = _gridSize.y * _pieceSize.y + (_gridSize.y - 1) * _spacing;

            var startX = -fullWidth / 2f + _pieceSize.x / 2f;
            var startY = fullHeight / 2f - _pieceSize.y / 2f;

            var piecesData = new List<PuzzlePieceData>();
            
            for (var i = 0; i < total; i++)
            {
                var col = i % _gridSize.x;
                var row = i / _gridSize.x;

                var pos = new Vector2(
                    startX + col * (_pieceSize.x + _spacing),
                    startY - row * (_pieceSize.y + _spacing)
                );

                var sprite = shuffledSprites[i];
                var spriteIndex = spriteList.IndexOf(sprite);
                piecesData.Add(new PuzzlePieceData(sprite, pos, i, spriteIndex));
            }

            _puzzleUIViewModel = new PuzzleUIViewModel
            {
                HintSprite = _puzzleModel.HintSprite,
                Pieces = piecesData,
                PuzzleSize = _pieceSize,
                OnClick = OnPieceClicked,
                OnCreated = OnCreatedPieces
            };

            _uiService.OpenView(_puzzleUIViewPrefab, _puzzleUIViewModel);

            return UniTask.CompletedTask;
        }

        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            if (_puzzleUIViewModel != null)
            {
                _uiService.CloseView(_puzzleUIViewModel);
            }

            return base.OnReleaseAsync(cancellationToken);
        }

        private void OnCreatedPieces(PuzzlePieceView[] pieces)
        {
            _pieces = pieces.ToList();
        }

        private void OnPieceClicked(PuzzlePieceView clicked)
        {
            if (_selected == null)
            {
                _selected = clicked;
                _selected.SetSelected(true);

                return;
            }

            if (_selected == clicked)
            {
                _selected.SetSelected(false);
                _selected = null;

                return;
            }

            SwapPieces(_selected, clicked);

            _selected.SetSelected(false);
            _selected = null;

            CheckIfSolved();
        }

        private void SwapPieces(PuzzlePieceView a, PuzzlePieceView b)
        {
            var aSprite = a.Sprite;
            var aIndex = a.SpriteIndex;
            
            a.SetSprite(b.Sprite, b.SpriteIndex);
            b.SetSprite(aSprite, aIndex);
        }

        private void CheckIfSolved()
        {
            for (var i = 0; i < _pieces.Count; i++)
            {
                var piece = _pieces[i];
                if (piece.PieceIndex != piece.SpriteIndex)
                {
                    return;
                }
            }
            _puzzleModel.OnPuzzleSolved?.Invoke();
        }
    }
}
