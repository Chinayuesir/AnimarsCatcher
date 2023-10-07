using UnityEngine;
using UnityEngine.Audio;

namespace AnimarsCatcher
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        public AudioMixer AudioMixer;
        private AudioSource mAudioSource;

        public AudioSource BGM;

        public AudioClip MenuBtnClick;
        public AudioClip SwitchBtnClick;
        public AudioClip UIBtnClick;

        private void Awake()
        {
            Instance = this;
            mAudioSource = GetComponent<AudioSource>();
        }

        public void PlayMenuBtnAudio()
        {
            mAudioSource.PlayOneShot(MenuBtnClick);
        }

        public void PlaySwitchBtnAudio()
        {
            mAudioSource.PlayOneShot(SwitchBtnClick);
        }

        public void PlayUIBtnAudio()
        {
            mAudioSource.PlayOneShot(UIBtnClick);
        }

        public void EnterMenu()
        {
            AudioMixer.SetFloat("GameVolume", -20f);
            BGM.Pause();
        }

        public void ExitMenu()
        {
            AudioMixer.SetFloat("GameVolume", -0f);
            BGM.UnPause();
        }

        #region Volume
        public void SetVolume(string name, float volume)
        {
            AudioMixer.SetFloat(name, volume);
        }
        public float GetVolume(string name)
        {
            AudioMixer.GetFloat(name, out float value);
            return value;
        }
        #endregion
    }
}