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
    }
}