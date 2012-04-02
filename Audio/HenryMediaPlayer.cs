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

        private string currentlyPlaying = "";
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
            music.Add("MenuMusic", cm.Load<Song>("Audio/Music/teamstairwell Intro"));
            music.Add("Level1Music", cm.Load<Song>("Audio/Music/teamstairwell Theme"));
            music.Add("Level2Music", cm.Load<Song>("Audio/Music/Forest Theme"));
            music.Add("Level3Music", cm.Load<Song>("Audio/Music/Forest Boss"));
            music.Add("Level4Music", cm.Load<Song>("Audio/Music/StarWars1"));
            music.Add("Level5Music", cm.Load<Song>("Audio/Music/teamstairwell Theme"));
            music.Add("Level6Music", cm.Load<Song>("Audio/Music/teamstairwell Theme"));
            music.Add("Level7Music", cm.Load<Song>("Audio/Music/teamstairwell Theme"));
            music.Add("Level8Music", cm.Load<Song>("Audio/Music/teamstairwell Theme"));
            music.Add("Level9Music", cm.Load<Song>("Audio/Music/teamstairwell Theme"));
            music.Add("Level10Music", cm.Load<Song>("Audio/Music/StarTrek1"));

            effects.Add("BossDeath", cm.Load<SoundEffect>("Audio/SoundEffects/HenryBossDeath"));
            effects.Add("ButtonClick", cm.Load<SoundEffect>("Audio/SoundEffects/HenryButtonClick"));
            effects.Add("ButtonRollover", cm.Load<SoundEffect>("Audio/SoundEffects/HenryButtonRollover"));
            effects.Add("Upgrade", cm.Load<SoundEffect>("Audio/SoundEffects/HenryUpgrade"));
            effects.Add("PlayerDeath", cm.Load<SoundEffect>("Audio/SoundEffects/HenryPlayerDeath"));
            effects.Add("BossHit", cm.Load<SoundEffect>("Audio/SoundEffects/Boss Hit"));
            effects.Add("PlayerHit", cm.Load<SoundEffect>("Audio/SoundEffects/Shield Zap"));
            effects.Add("ExplosionSmall", cm.Load<SoundEffect>("Audio/SoundEffects/Explosion small"));
            effects.Add("ExplosionMedium", cm.Load<SoundEffect>("Audio/SoundEffects/Explosion medium"));
            effects.Add("BulletTorpedo", cm.Load<SoundEffect>("Audio/SoundEffects/QuantumTorpedo"));
            effects.Add("BulletMissle", cm.Load<SoundEffect>("Audio/SoundEffects/Missle"));
            effects.Add("BulletLaser", cm.Load<SoundEffect>("Audio/SoundEffects/BasicLaser"));
            effects.Add("BulletPhoton", cm.Load<SoundEffect>("Audio/SoundEffects/PhotonTorpedo"));
            effects.Add("LaunchSpawner", cm.Load<SoundEffect>("Audio/SoundEffects/BossLaunchSpawner"));
            effects.Add("BulletPea", cm.Load<SoundEffect>("Audio/SoundEffects/BulletPea"));
        }

        public void PlayMusic(string name){
            if(name != currentlyPlaying){
                MediaPlayer.Play(music[name]);
                currentlyPlaying = name;
            }
        }

        public void PlayEffect(string name){
            effects[name].Play();
        }

        public void PlayFor(string name, int times, float everyThisManyMilliseconds){
            queueEntry e = new queueEntry();
            e.sound = effects[name].CreateInstance();
            e.numOfTimesToPlay = times;
            e.interval = everyThisManyMilliseconds;
            //this function is unfinished and unused
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
