using UnityEngine;

namespace Sopka
{
    public struct PuzzlePieceData
    {
        public Sprite Sprite;
        public Vector2 AnchoredPosition;
        public int PuzzleIndex;
        public int SpriteIndex;

        public PuzzlePieceData(Sprite sprite, Vector2 anchoredPosition, int puzzleIndex, int spriteIndex)
        {
            Sprite = sprite;
            AnchoredPosition = anchoredPosition;
            PuzzleIndex = puzzleIndex;
            SpriteIndex = spriteIndex;
        }
    }
}