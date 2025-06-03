using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop.Services;

namespace Sopka
{
    public interface IAudioService : IService
    {
        UniTask PlayMusicAsync(AudioClip clip, float volume = 1f);
        void StopMusic();
    }
}