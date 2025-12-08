
using System.Collections.Generic;
using UnityEngine;
using GKIT;

namespace Game
{
    
    public class SoundMan : GSingleton<SoundMan>
    {
        public const string K_SOUND_PATYH = "sounds";

        public const string K_SOUND_ENABLE_FLAG = "sound_enable_flag";
        public const string K_MUSIC_ENABLE_FLAG = "music_enable_flag";

        public SFXPath SFX = new SFXPath();

        private Dictionary<string, AudioClip> _soundLib;
        private Dictionary<string, AudioClip> _musicLib;
        private Dictionary<string, AudioClip> SoundLib
        {
            get
            {
                if (_soundLib == null) _soundLib = new Dictionary<string, AudioClip>(20);
                return _soundLib;
            }
        }
        private Dictionary<string, AudioClip> MusicLib
        {
            get
            {
                if (_musicLib == null) _musicLib = new Dictionary<string, AudioClip>(20);
                return _musicLib;
            }
        }
        
        private bool _enableSound = false;
        private bool _enableMusic= false;

        private AudioClip _curMusic;

        public bool SoundEnabled
        {
            get => _enableSound;
            set => SetSoundEnabled(value);
        }
        
        public bool MusicEnabled
        {
            get => _enableMusic;
            set => SetMusicEnabled(value);
        }

        private GameObject _gameObject;
        private AudioSource _musicSource;
        private AudioListener _listener;

        #region 初始化

        public void Init()
        {
            _gameObject = new GameObject("Sound");
            Object.DontDestroyOnLoad(_gameObject);
            _musicSource = _gameObject.AddComponent<AudioSource>();
            _listener = _gameObject.AddComponent<AudioListener>();

            PreloadSounds();
            GetSoundEnabled();
            GetMusicEnabled();
        }
        
        
        public AudioClip LoadClip(string address)
        {
            var instance = ResMan.AddressLoad<AudioClip>(address);
            return instance;
        }
        
        private void PreloadSounds()
        {
            string[] preloadKeys = new string[]
            {

            };
			
            LoadSounds(preloadKeys); // 预加载声音
        }


        #endregion

        #region 音效缓存


        public void LoadSound(string key)
        {
            if (!SoundLib.ContainsKey(key)) SoundLib[key] = LoadClip(key);
        }


        public void LoadSounds(IList<string> keys)
        {
            foreach (var k in keys)
            {
                LoadSound(k);
            }
        }


        public void LoadMusic(string key)
        {
            if (!MusicLib.ContainsKey(key)) MusicLib[key] = LoadClip(key);
        }


        public void LoadMusics(IList<string> keys)
        {
            foreach (var k in keys)
            {
                LoadMusic(k);
            }
        }

        public AudioClip GetSound(string key)
        {
            if (SoundLib.TryGetValue(key, out var s))
            {
                return s;
            }
            s = LoadClip(key);
            SoundLib[key] = s;
            return s;
        }
        
        
        public AudioClip GetMusic(string key)
        {
            if (MusicLib.TryGetValue(key, out var s))
            {
                return s;
            }
            s = LoadClip(key);
            MusicLib[key] = s;
            return s;
        }

        

        #endregion

        #region 播放音效
        
        public void Play(string key, float delay = 0, float pitch =1)
        {
            if (!SoundEnabled) return;
            // 播放音效
            var clip = GetSound(key);
            if (clip != null)
            {
                var source = _gameObject.AddComponent<AudioSource>();
                source.clip = clip;
                source.pitch = pitch;
                source.clip = clip;
                if (delay > 0)
                {
                    source.PlayDelayed(delay);
                }
                else
                {
                    source.Play();
                }
                
                // Play Oneshot
                Object.Destroy(source, (delay+ clip.length)*Time.timeScale);
            }
        }

        #endregion
        
        #region 播放音乐

        public void StopMusic()
        {
            _musicSource.Stop();
        }

        public void PlayMusic(AudioClip clip,  float delay = 0)
        {
            _curMusic = clip;
            _musicSource.clip = clip;
            _musicSource.loop = true;
            _musicSource.volume = 0.6f;  // 设置音量
            if (delay > 0)
            {
                _musicSource.PlayDelayed(delay);
            }
            else
            {
                _musicSource.Play();
            }
        }
        
        
        public void PlayMusic(string key, float delay = 0)
        {
            var clip = GetMusic(key);
            if (_musicSource.clip != null && _musicSource.isPlaying 
                && _musicSource.clip.name == clip.name)
            {
                return;
            }
            _curMusic = clip;
            if (!MusicEnabled) return;
            PlayMusic(clip, delay);
        }

        public string GetBGMKey(string name)
        {
            return $"{K_SOUND_PATYH}/{name}";
        }

        public bool IsBGMStarted => (_musicSource.clip != null);

        
        #endregion

        #region 音效开关
        
        /// <summary>
        /// 获取Sound开关状态
        /// </summary>
        private void GetSoundEnabled()
        {
            if (!PlayerPrefs.HasKey(K_SOUND_ENABLE_FLAG))
            {
                SetSoundEnabled(true);
                return;
            } 
            _enableSound = PlayerPrefs.GetInt(K_SOUND_ENABLE_FLAG, 0) == 1;
        }
        
        /// <summary>
        /// 设置Sound开关状态
        /// </summary>
        /// <param name="value"></param>
        private void SetSoundEnabled(bool value)
        {
            _enableSound = value;
            PlayerPrefs.SetInt(K_SOUND_ENABLE_FLAG, value ? 1 : 0);
        }

        private void GetMusicEnabled()
        {
            if (!PlayerPrefs.HasKey(K_MUSIC_ENABLE_FLAG))
            {
                SetMusicEnabled(true);
                return;
            } 
            _enableMusic = PlayerPrefs.GetInt(K_MUSIC_ENABLE_FLAG, 0) == 1;
        }

        private void SetMusicEnabled(bool value)
        {
            _enableMusic = value;
            PlayerPrefs.SetInt(K_MUSIC_ENABLE_FLAG, value? 1 : 0);
            if (_musicSource.isPlaying != value)
            {
                if (value)
                {
                    if (_curMusic != null)
                    {
                        PlayMusic(_curMusic);
                    }
                    else
                    {
                        _musicSource.UnPause();
                    }
                }
                else
                {
                    _musicSource.Pause();
                }
            }
        }

        #endregion
        
        #region 音效名称常量

        public class SFXPath
        {
            public static string GetPath(string name)
            {
                return $"{K_SOUND_PATYH}/{name}";
            }
            
            public string FXStart => GetPath("fx_start");
            public string FXCool => GetPath("fx_cool");
            public string FXExcellent => GetPath("fx_excellent");
            public string FXGreat => GetPath("fx_great");
            public string FXFireworks => GetPath("fx_fireworks");
            public string Merge => GetPath("fx_merge");
            public string UIButtonClick => GetPath("ui_button_click");
            public string UIWindowClose => GetPath("ui_window_close");
           
            
            // public string ComboVoicePath(int number)
            // {
            //     number = Mathf.Clamp(number, 2, 6 );
            //     return GetPath($"sfx_combo_{((BingoType)number).ToString().ToLower()}");
            // } 
        }
        
        
        #endregion

    
    }
}






