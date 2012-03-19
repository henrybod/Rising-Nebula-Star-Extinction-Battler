using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hook.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace teamstairwell.Interface
{
    public class PauseScreen : Screen
    {
        TextSprite Paused;
        public PauseScreen()
            : base()
        {
            Microsoft.Xna.Framework.Content.ContentManager cm;
            HenryBattlefield b;
            Paused = new TextSprite("GAME BE PAUSED!");
            //SaveLoadGameButton = new HenryUpgradeButton(100,100,"Load / Save Game",cm,b,spriteNormal,spriteHighlite, spritePress,up);
            Paused.Color = Color.White;
            Paused.Position = new Vector3(RNSEB.RESOLUTION.X / 2 - 40, RNSEB.RESOLUTION.Y / 2 - 60, 0);
        }          

        public new void Update(GameTime gt)
        {
            base.Update(gt);
        }

        public new void Draw(SpriteBatch sb )
        {
            base.Draw(sb);
            Paused.Draw(sb);
        }

    }
}
