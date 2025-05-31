using System;
using System.Collections.Generic;
using Whaledevelop.UI;
using UnityEngine;

namespace Sopka
{
    public class PuzzleUIViewModel : IUIViewModel
    {
        public Sprite HintSprite;

        public List<PuzzlePieceData> Pieces;
        
        public Vector2 PuzzleSize;

        public Action<PuzzlePieceView> OnClick;

        public Action<PuzzlePieceView[]> OnCreated;
    }
}