using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;

namespace teamstairwell.Interface.HUD {
    [Serializable()]
    public class WeaponBar {

        //data members
        public Vector2 Position = Vector2.Zero; //the top left corner of the weapon bar
        public List<WeaponIndicator> Weapons = new List<WeaponIndicator>(); //a list of all the weapons in this weapon bar
        public HenrySprite selectorSquare;
        public bool SelectorVisible = false;

        //functions
        public WeaponBar() {
            selectorSquare = new HenrySprite(RNSEB.cm);
            selectorSquare.LoadContent("SelectorSquare", true, 10);
            selectorSquare.Position = this.Position;
        }

        public void AddWeapon(HenryWeapon w) {
            Weapons.Add(new WeaponIndicator(w, this.Position + new Vector2(45, 0) * Weapons.Count, (Weapons.Count+1).ToString()));
        }

        public void ChangeWeapon(int num, HenryWeapon w) {
            if(num < 1 || num > 10) {
                Console.WriteLine("Invalid weapon hardpoint access: " + num);
                return;
            }
            Weapons[num-1] = new WeaponIndicator(w, this.Position + new Vector2(45, 0) * (num-1));
        }

        public void SelectWeapon(int i) {
            selectorSquare.Position = this.Position + new Vector2(45, 0) * (i - 1) - new Vector2(6, 6);
        }

        public void Draw(SpriteBatch sb) {
            foreach (WeaponIndicator w in Weapons) {
                w.Draw(sb);
            }
            if(SelectorVisible) selectorSquare.Draw(sb);
        }

        public void Update(GameTime gt) {
            foreach (WeaponIndicator w in Weapons) {
                w.Update(gt);
            }
            selectorSquare.Update(gt);
        }

    }
}
