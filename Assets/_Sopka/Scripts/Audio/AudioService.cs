using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using Whaledevelop;
using Whaledevelop.Dialogs;
using Whaledevelop.Reactive;
using Whaledevelop.Services;

namespace Sopka
{
    [CreateAssetMenu(fileName = "AudioService", menuName = "Whaledevelop/Services/AudioService")]
    public class AudioService : Service, IAudioService
    {
        [SerializeField]
        private AudioSource _musicPrefab;

        [SerializeField]
        private float _fadeDuration = 1f;

        [SerializeField]
        private AudioMixerGroup _musicGroup;

        [SerializeField]
        private AudioBank _audioBank;

        [Inject]
        private IGameModel _gameModel;

        [Inject]
        private IGameStatesService _gameStatesService;

        [Inject]
        private SceneModel _sceneModel;

        private AudioSource _currentMusic;

        private CancellationTokenSource _fadeCts;

        private readonly List<IDisposable> _disposables = new();

        private ObjectPool<AudioSource> _musicPool;

        private const string MusicVolumeParam = "musicVolume";

        private const float MinDb = -80f;

        private const float MaxDb = 0f;

        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            _musicPool = new ObjectPool<AudioSource>(
                CreatePooledItem,
                OnTakeFromPool,
                OnReturnToPool,
                OnDestroyPoolObject,
                collectionCheck: false
            );

            _gameStatesService.CurrentState.Subscribe(OnChangeState).AddToCollection(_disposables);

            _gameModel.DialogModel.ProcessingDialog.Subscribe(OnProcessDialog).AddToCollection(_disposables);
            
            return base.OnInitializeAsync(cancellationToken);
        }

        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            _disposables.Dispose();

            _musicPool?.Clear();

            return base.OnReleaseAsync(cancellationToken);
        }

        private void OnProcessDialog(IDialogSettings dialogSettings)
        {
            if (dialogSettings == null)
            {
                return;
            }

            if (!_audioBank.DialogsAudio.TryGetValue((DialogSettings)dialogSettings, out var dialogAudio))
            {
                return;
            }

            PlayMusicAsync(dialogAudio.AudioClip, dialogAudio.Volume).Forget();
        }

        private void OnChangeState(IGameState gameState)
        {
            if (gameState is MapGameState or StartMenuGameState)
            {
                PlayMusicAsync(_audioBank.MapAudioData.AudioClip, _audioBank.MapAudioData.Volume).Forget();

                return;
            }
        }

        public async UniTask PlayMusicAsync(AudioClip clip, float volume = 1f)
        {
            if (_currentMusic != null && _currentMusic.clip == clip)
            {
                return;
            }

            _fadeCts?.Cancel();
            _fadeCts = new CancellationTokenSource();

            var token = _fadeCts.Token;

            if (_currentMusic != null)
            {
                await FadeOutAsync(_fadeDuration, token);

                _currentMusic.Stop();
                _musicPool.Release(_currentMusic);
            }

            var newMusic = _musicPool.Get();

            newMusic.clip = clip;
            newMusic.volume = 1f;
            newMusic.loop = true;
            newMusic.outputAudioMixerGroup = _musicGroup;

            SetNormalizedVolume(0f);

            newMusic.Play();

            _currentMusic = newMusic;

            await FadeInAsync(volume, _fadeDuration, token);
        }

        public void StopMusic()
        {
            _fadeCts?.Cancel();

            if (_currentMusic == null)
            {
                return;
            }

            _currentMusic.Stop();
            _musicPool.Release(_currentMusic);
            _currentMusic = null;
        }

        private AudioSource CreatePooledItem()
        {
            var instance = Instantiate(_musicPrefab, _sceneModel.SoundsRoot);
            instance.gameObject.SetActive(false);

            return instance;
        }

        private void OnTakeFromPool(AudioSource source)
        {
            source.gameObject.SetActive(true);
        }

        private void OnReturnToPool(AudioSource source)
        {
            source.gameObject.SetActive(false);
        }

        private void OnDestroyPoolObject(AudioSource source)
        {
            Destroy(source.gameObject);
        }

        private float GetNormalizedVolume()
        {
            if (_musicGroup.audioMixer.GetFloat(MusicVolumeParam, out var db))
            {
                return Mathf.InverseLerp(MinDb, MaxDb, db);
            }

            return 1f;
        }

        private void SetNormalizedVolume(float normalized)
        {
            var db = Mathf.Lerp(MinDb, MaxDb, normalized);
            _musicGroup.audioMixer.SetFloat(MusicVolumeParam, db);
        }

        private async UniTask FadeInAsync(float targetNormalizedVolume, float duration, CancellationToken token)
        {
            var tcs = new UniTaskCompletionSource();

            var tween = DOTween.To(
                    () => GetNormalizedVolume(),
                    value => SetNormalizedVolume(value),
                    targetNormalizedVolume,
                    duration
                )
                .SetEase(Ease.InOutSine)
                .OnComplete(() => tcs.TrySetResult());

            await using (token.Register(() =>
                         {
                             tween.Kill();
                             tcs.TrySetCanceled(token);
                         }))
            {
                await tcs.Task;
            }
        }

        private async UniTask FadeOutAsync(float duration, CancellationToken token)
        {
            var tcs = new UniTaskCompletionSource();

            var tween = DOTween.To(
                    GetNormalizedVolume,
                    SetNormalizedVolume,
                    0f,
                    duration
                )
                .SetEase(Ease.InOutSine)
                .OnComplete(() => tcs.TrySetResult());

            await using (token.Register(() =>
                         {
                             tween.Kill();
                             tcs.TrySetCanceled(token);
                         }))
            {
                await tcs.Task;
            }
        }
    }
}
