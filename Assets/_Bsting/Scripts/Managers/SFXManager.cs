using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bsting.Ship.Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class SFXManager : Manager<SFXManager>
    {
        [Header("Audio SFX Players")]
        [SerializeField] private AudioSource _audioSourceForUISFX = null;
        [SerializeField] private AudioSource _audioSourceForPlayerBlastersSFX = null;
        [SerializeField] private AudioSource _audioSourceForPlayerEngineSFX = null;
        [Header("Audio SFX Clips To Play")]
        [SerializeField] private AudioClip _newGameButtonSFX = null;
        [SerializeField] private AudioClip _settingsButtonSFX = null;
        [SerializeField] private AudioClip _backButtonSFX = null;
        [SerializeField] private AudioClip _menuButtonSFX = null;
        [SerializeField] private AudioClip _exitGameButtonSFX = null;
        [SerializeField] private AudioClip _pauseGameSFX = null;
        [SerializeField] private AudioClip _blasterSFX = null;
        [SerializeField] private AudioClip _hyperspeedActiveSFX = null;
        [SerializeField] private AudioClip _hyperspeedReadySFX = null;
        [SerializeField] private AudioClip _hyperspeedOnCooldownSFX = null;

        #region MonoBehaviors
        protected override void Awake()
        {
            base.Awake();
        }

        void OnEnable()
        {
            // Backup assignments in case they're still null by the time it's loading:
            if (_audioSourceForUISFX == null)
            {
                _audioSourceForUISFX = this.gameObject.GetComponent<AudioSource>();
            }

            if (_audioSourceForPlayerBlastersSFX == null)
            {
                _audioSourceForPlayerBlastersSFX = this.gameObject.GetComponent<AudioSource>();
            }

            if (_audioSourceForPlayerEngineSFX == null)
            {
                _audioSourceForPlayerEngineSFX = this.gameObject.GetComponent<AudioSource>();
            }
            // Redundant, but provides a reminder to have this property off:
            _audioSourceForUISFX.playOnAwake = false;
            _audioSourceForPlayerBlastersSFX.playOnAwake = false;
            _audioSourceForPlayerEngineSFX.playOnAwake = false;
        }

        #endregion

        #region Helper Method(s)
        public void PlayNewGameButtonClickedSFX()
        {
            _audioSourceForUISFX?.PlayOneShot(_newGameButtonSFX);
        }

        public void PlaySettingsButtonClickedSFX()
        {
            _audioSourceForUISFX?.PlayOneShot(_settingsButtonSFX);
        }

        public void PlayExitButtonClickedSFX()
        {
            _audioSourceForUISFX?.PlayOneShot(_exitGameButtonSFX);
        }

        public void PlayBackButtonClickedSFX()
        {
            _audioSourceForUISFX?.PlayOneShot(_backButtonSFX);
        }

        public void PlayMenuButtonClickedSFX()
        {
            _audioSourceForUISFX?.PlayOneShot(_menuButtonSFX);
        }

        public void PlayBlasterFiredSFX()
        {
            _audioSourceForPlayerBlastersSFX?.PlayOneShot(_blasterSFX);
        }

        public void PlayPauseTriggeredSFX()
        {
            _audioSourceForUISFX?.PlayOneShot(_pauseGameSFX);
        }

        public void PlayHyperspeedActiveSFX()
        {
            if (_audioSourceForPlayerEngineSFX != null)
            {
                StopCurrentlyActivePlayerEngineSFX();
                _audioSourceForPlayerEngineSFX.clip = _hyperspeedActiveSFX;
                _audioSourceForPlayerEngineSFX.Play();
            }
        }

        public void PlayHyperspeedReadySFX()
        {
            if (_audioSourceForPlayerEngineSFX != null)
            {
                StopCurrentlyActivePlayerEngineSFX();
                _audioSourceForPlayerEngineSFX.clip = _hyperspeedReadySFX;
                _audioSourceForPlayerEngineSFX.Play();
            }
        }

        public void PlayHyperspeedOnCooldownSFX()
        {
            if (_audioSourceForPlayerEngineSFX != null)
            {
                StopCurrentlyActivePlayerEngineSFX();
                _audioSourceForPlayerEngineSFX.clip = _hyperspeedOnCooldownSFX;
                _audioSourceForPlayerEngineSFX.Play();
            }
        }

        public void StopCurrentlyActiveUISFX()
        {
            _audioSourceForUISFX?.Stop();
        }

        public void StopCurrentlyActivePlayerBlastersSFX()
        {
            _audioSourceForPlayerBlastersSFX?.Stop();
        }

        public void StopCurrentlyActivePlayerEngineSFX()
        {
            _audioSourceForPlayerEngineSFX?.Stop();
        }
        #endregion
    }
}