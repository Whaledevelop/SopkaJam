using System;
using UnityEngine;

namespace Sopka
{
    public interface IPuzzleModel
    {
        Sprite HintSprite { get; }
        
        Sprite[] PuzzleSprites { get; }
        
        Action OnPuzzleSolved { get; set; }
    }
}