using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Sopka
{
    public static class DOTweenUtility
    {
        public static async UniTaskVoid LoopScale(RectTransform target, CancellationToken token)
        {
            var scaleUp = Vector3.one * 1.1f;
            var scaleDown = Vector3.one;

            try
            {
                while (!token.IsCancellationRequested)
                {
                    var tweenUp = target.DOScale(scaleUp, 0.4f).SetEase(Ease.InOutSine);
                    await tweenUp.AsyncWaitForCompletion();

                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    var tweenDown = target.DOScale(scaleDown, 0.4f).SetEase(Ease.InOutSine);
                    await tweenDown.AsyncWaitForCompletion();
                }
            }
            catch (Exception)
            {
                // Игнорируем исключения, если они возникнут
            }

            target.localScale = Vector3.one;
        }
    }
}