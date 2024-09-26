using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bsting.Ship.Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class SFXManager : Manager<SFXManager>
    {
        [Header("Audio SFX Player")]
        [SerializeField] private AudioSource _audioSourceForSFX = null;
        [Header("Audio SFX Clips To Play")]
        [SerializeField] private AudioClip _newGameButtonSFX = null;
        [SerializeField] private AudioClip _settingsButtonSFX = null;
        [SerializeField] private AudioClip _backButtonSFX = null;
        [SerializeField] private AudioClip _menuButtonSFX = null;
        [SerializeField] private AudioClip _exitGameButtonSFX = null;
        [SerializeField] private AudioClip _blasterSFX = null;
        // [SerializeField] private AudioClip _engineSFX = null;
        // [SerializeField] private AudioClip _hyperspeedSFX = null;

        #region MonoBehaviors
        protected override void Awake()
        {
            base.Awake();
        }

        void OnEnable()
        {
            if (_audioSourceForSFX == null)
            {
                _audioSourceForSFX = this.gameObject.GetComponent<AudioSource>();
            }
            // Redundant, but provides a reminder to have this property off:
            _audioSourceForSFX.playOnAwake = false;
        }

        #endregion

        #region Helper Method(s)
        public void PlayNewGameButtonClickedSFX()
        {
            _audioSourceForSFX?.PlayOneShot(_newGameButtonSFX);
        }

        public void PlaySettingsButtonClickedSFX()
        {
            _audioSourceForSFX?.PlayOneShot(_settingsButtonSFX);
        }

        public void PlayExitButtonClickedSFX()
        {
            _audioSourceForSFX?.PlayOneShot(_exitGameButtonSFX);
        }

        public void PlayBackButtonClickedSFX()
        {
            _audioSourceForSFX?.PlayOneShot(_backButtonSFX);
        }

        public void PlayMenuButtonClickedSFX()
        {
            _audioSourceForSFX?.PlayOneShot(_menuButtonSFX);
        }

        public void PlayBlasterFiredSFX()
        {
            _audioSourceForSFX?.PlayOneShot(_blasterSFX);
        }
        #endregion
    }
}