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

        private float musicVolume = 0.2f;
        private float effectVolume = 0.2f;

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
            music.Add("MenuMusic", cm.Load<Song>("Audio/Music/teamstairwell Intro"));
            music.Add("Level1Music", cm.Load<Song>("Audio/Music/teamstairwell Theme"));
            music.Add("Level2Music", cm.Load<Song>("Audio/Music/Forest Theme"));
            music.Add("Level3Music", cm.Load<Song>("Audio/Music/Forest Boss"));
            music.Add("Level4Music", cm.Load<Song>("Audio/Music/StarWars1"));
            music.Add("Level5Music", cm.Load<Song>("Audio/Music/Boogie Wonderland"));
            music.Add("Level6Music", cm.Load<Song>("Audio/Music/StarCraft Terran2"));
            music.Add("Level7Music", cm.Load<Song>("Audio/Music/StarWars2"));
            music.Add("Level8Music", cm.Load<Song>("Audio/Music/IndependenceDay1"));
            music.Add("Level9Music", cm.Load<Song>("Audio/Music/SuperMarioAnnoying"));
            music.Add("Level10Music", cm.Load<Song>("Audio/Music/StarTrek1"));
            music.Add("ZihaoVictoryMusic", cm.Load<Song>("Audio/Music/ChronoTrigger1"));
            music.Add("NotusVictoryMusic", cm.Load<Song>("Audio/Music/StarWars3"));

            effects.Add("ButtonClick", cm.Load<SoundEffect>("Audio/SoundEffects/HenryButtonClick"));
            effects.Add("ButtonRollover", cm.Load<SoundEffect>("Audio/SoundEffects/HenryButtonRollover"));
            effects.Add("PlayerHit", cm.Load<SoundEffect>("Audio/SoundEffects/Shield Zap"));
            effects.Add("PlayerShieldDown", cm.Load<SoundEffect>("Audio/SoundEffects/PlayerShieldDown"));
            effects.Add("PlayerShieldUp", cm.Load<SoundEffect>("Audio/SoundEffects/PlayerShieldUp"));
            effects.Add("PlayerDeath", cm.Load<SoundEffect>("Audio/SoundEffects/HenryPlayerDeath"));
            effects.Add("BossHit", cm.Load<SoundEffect>("Audio/SoundEffects/Boss Hit"));
            effects.Add("Launch", cm.Load<SoundEffect>("Audio/SoundEffects/Launch"));
            effects.Add("BossDeath", cm.Load<SoundEffect>("Audio/SoundEffects/HenryBossDeath"));
            effects.Add("LevelUp", cm.Load<SoundEffect>("Audio/SoundEffects/LevelUp"));
            effects.Add("UpgradeInstall", cm.Load<SoundEffect>("Audio/SoundEffects/UpgradeInstall"));
            effects.Add("Laser", cm.Load<SoundEffect>("Audio/SoundEffects/Laser"));
            effects.Add("IonBeam", cm.Load<SoundEffect>("Audio/SoundEffects/IonBeam"));
            effects.Add("Shockwave", cm.Load<SoundEffect>("Audio/SoundEffects/Shockwave"));
            effects.Add("Rocket", cm.Load<SoundEffect>("Audio/SoundEffects/Rocket"));
            effects.Add("SeekerMissles", cm.Load<SoundEffect>("Audio/SoundEffects/SeekerMissles"));
            effects.Add("PeaShooter", cm.Load<SoundEffect>("Audio/SoundEffects/PeaShooter"));
            effects.Add("PhotonTorpedo", cm.Load<SoundEffect>("Audio/SoundEffects/PhotonTorpedo"));
            effects.Add("PlasmaTorpedo", cm.Load<SoundEffect>("Audio/SoundEffects/PlasmaTorpedo"));
            effects.Add("QuantumMines", cm.Load<SoundEffect>("Audio/SoundEffects/QuantumMines"));
            effects.Add("RingOfFire", cm.Load<SoundEffect>("Audio/SoundEffects/RingOfFire"));
            effects.Add("PlasmaWall", cm.Load<SoundEffect>("Audio/SoundEffects/PlasmaWall"));
            effects.Add("Lightning", cm.Load<SoundEffect>("Audio/SoundEffects/Lightning"));
            effects.Add("ExplosionSmall", cm.Load<SoundEffect>("Audio/SoundEffects/Explosion small"));
            effects.Add("ExplosionMedium", cm.Load<SoundEffect>("Audio/SoundEffects/Explosion medium"));





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
