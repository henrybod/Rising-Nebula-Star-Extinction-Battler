using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using teamstairwell.Graphics;

namespace teamstairwell.Interface {
    [Serializable()]
    class HenryUpgradeButton : HenryButton {

        private string name = "";
        private string description = "";
        private bool displayingText = false;
        public HenrySprite selectorSquare;
        public RNSEB.HenryUpgrade Upgrade;
        public string Requirement = "NA";
        public enum UpgradeStatus {
            Obtainable,
            Unobtainable,
            Obtained
        };
        public UpgradeStatus Status = UpgradeStatus.Unobtainable;

        public HenryUpgradeButton(string name, int column, int row, RNSEB.HenryUpgrade up, string description)
            : base((int)(0.1f*RNSEB.RESOLUTION.X + column * 0.2f * RNSEB.RESOLUTION.X),
                   (int)(0.2f*RNSEB.RESOLUTION.Y + row * 0.125f * RNSEB.RESOLUTION.Y),
                   "", new RNSEB.OnClick(()=>{}), RNSEB.cm,
                   name+"Icon", name+"Icon", name+"Icon") {

            this.name = name;
            this.Upgrade = up;
            this.description = description;
            this.Scale = 1.5f;

            selectorSquare = new HenrySprite(cm);
            selectorSquare.LoadContent("SelectorSquare", true, 10);
            selectorSquare.CenterOrigin();
            selectorSquare.Position = Position;
            selectorSquare.Scale = 1.5f;
        }

        public new void Update(GameTime gt) {
            //displayingText = (GetButtonState() != ButtonState.Normal);
            displayingText = RNSEB.Input.MouseIsIn(Position, Size);

            Enabled = (Status == UpgradeStatus.Obtainable);

            if (Status == UpgradeStatus.Obtained)
                selectorSquare.Color = Color.Blue;
            else if (Status == UpgradeStatus.Unobtainable)
                selectorSquare.Color = Color.DimGray;
            else //is obtainable
                if (GetButtonState() == ButtonState.Normal)
                    selectorSquare.Color = Color.White;
                else if (GetButtonState() == ButtonState.Highlighted)
                    selectorSquare.Color = Color.Green;
                else if (GetButtonState() == ButtonState.Pressed)
                    selectorSquare.Color = Color.DarkGreen;

            selectorSquare.Update(gt);
            base.Update(gt);
        }

        public new void Draw(SpriteBatch sb) {
            if(displayingText)
                sb.DrawString(RNSEB.TextFont, description, new Vector2(0.2f * RNSEB.RESOLUTION.X, 0.9f * RNSEB.RESOLUTION.Y), Color.White);
            
            selectorSquare.Draw(sb);
            base.Draw(sb);
        }

        public new void ReloadContent() {
            base.ReloadContent();
            selectorSquare = new HenrySprite(RNSEB.cm);
            selectorSquare.LoadContent("SelectorSquare", true, 10);
            selectorSquare.CenterOrigin();
            selectorSquare.Position = Position;
            selectorSquare.Scale = 1.5f;
            buttonText = new HenryText(Position, RNSEB.ButtonFont, "");
            onClick = new RNSEB.OnClick(()=>{});
        }
    }
}