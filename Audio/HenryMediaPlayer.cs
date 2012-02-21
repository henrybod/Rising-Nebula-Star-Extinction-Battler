using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace teamstairwell.Audio {

    public class HenryMediaPlayer {
        //todo: implement fade on stop/start (music)
        private float musicVolume = 0.1f;
        private float effectVolume = 0.5f;
        
        private ContentManager cm;
        private Dictionary<string, Song> music = new Dictionary<string,Song>();
        private Dictionary<string, SoundEffect> effects = new Dictionary<string,SoundEffect>();
        private Dictionary<string, queueEntry> effectQueue = new Dictionary<string, queueEntry>();
        
        public HenryMediaPlayer(ContentManager cm){
            this.cm = cm;
            MediaPlayer.Volume = this.musicVolume;
            SoundEffect.MasterVolume = this.effectVolume;
        }

        public void LoadContent(){
            music.Add("ForestMusic", cm.Load<Song>("Audio/Music/Forest Theme"));
            music.Add("ForestBoss", cm.Load<Song>("Audio/Music/Forest Boss"));
            music.Add("MenuMusic", cm.Load<Song>("Audio/Music/teamstairwell Theme"));
            music.Add("GameMusic", cm.Load<Song>("Audio/Music/teamstairwell Theme"));

            effects.Add("BossDeath", cm.Load<SoundEffect>("Audio/SoundEffects/HenryBossDeath"));
            effects.Add("ButtonClick", cm.Load<SoundEffect>("Audio/SoundEffects/HenryButtonClick"));
            effects.Add("ButtonRollover", cm.Load<SoundEffect>("Audio/SoundEffects/HenryButtonRollover"));
            effects.Add("Upgrade", cm.Load<SoundEffect>("Audio/SoundEffects/HenryUpgrade"));
            effects.Add("PlayerDeath", cm.Load<SoundEffect>("Audio/SoundEffects/HenryPlayerDeath"));
            effects.Add("BossDamage", cm.Load<SoundEffect>("Audio/SoundEffects/MEGAMAN!"));
        }

        public void Play(string name){
            try {
                if(MediaPlayer.State == MediaState.Stopped) MediaPlayer.Play(music[name]);
            } catch {}
            try {
                effects[name].Play();
            } catch {}
        }

        public void PlayFor(string name, int times, float everyThisManyMilliseconds){
            queueEntry e = new queueEntry();
            e.sound = effects[name].CreateInstance();
            e.numOfTimesToPlay = times;
            e.interval = everyThisManyMilliseconds;
        }


        public void StopMusic(){
            MediaPlayer.Stop();
        }
        public void PauseMusic() {
            MediaPlayer.Pause();
        }
        public void ResumeMusic() {
            MediaPlayer.Resume();
        }

        public void Update(GameTime gt){
            //for playing queued stuff

        }

    }

    public struct queueEntry{
        public SoundEffectInstance sound;
        public int numOfTimesToPlay;
        public float interval;
    }
}
