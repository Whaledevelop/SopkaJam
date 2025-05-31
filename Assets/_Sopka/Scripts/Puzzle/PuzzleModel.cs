using System;
using UnityEngine;

namespace Sopka
{
    public class PuzzleModel : IPuzzleModel
    {
        public Sprite HintSprite { get; }
        public Sprite[] PuzzleSprites { get; }

        public Action OnPuzzleSolved { get; set; }

        public PuzzleModel(Sprite[] puzzleSprites, Sprite hintSprite)
        {
            PuzzleSprites = puzzleSprites;
            HintSprite = hintSprite;
        }
    }
}