using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AnimarsCatcher
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        //Audio
        public AudioMixer AudioMixer;
        private AudioSource mAudioSource;
        
        //AudioClip
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
        }

        public void ExitMenu()
        {
            AudioMixer.SetFloat("GameVolume", 0f);
        }
    }
}


