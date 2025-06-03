using System;
using UnityEngine;
using UnityEngine.Serialization;
using Whaledevelop.Dialogs;
using Whaledevelop.Utility;

namespace Sopka
{
    [Serializable]
    public class AudioData
    {
        public AudioClip AudioClip;
        public float Volume = 1f;
    }
    
    [CreateAssetMenu(fileName = "AudioBank", menuName = "Sopka/Settings/AudioBank")]
    public class AudioBank : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<DialogSettings, AudioData> _dialogsAudio;

        [SerializeField] private AudioData _mapAudioData;

        public SerializableDictionary<DialogSettings, AudioData> DialogsAudio => _dialogsAudio;

        public AudioData MapAudioData => _mapAudioData;
    }
}