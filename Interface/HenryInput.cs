using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace teamstairwell.Interface {

    public class HenryInput {

        private Dictionary<string, bool> input = new Dictionary<string, bool>();
        private int MouseX = (int)RNSEB.RESOLUTION.X / 2;
        private int MouseY = (int)RNSEB.RESOLUTION.Y / 2;
        private int currMouseWheel = 0, prevMouseWheel = 0;

        //control setup
        Keys PlayerUp = Keys.W;
        Keys PlayerDown = Keys.S;
        Keys PlayerLeft = Keys.A;
        Keys PlayerRight = Keys.D;
        Keys BossUp = Keys.W;
        Keys BossDown = Keys.S;
        Keys BossLeft = Keys.A;
        Keys BossRight = Keys.D;
        Keys PauseGame = Keys.P;
        Keys WeaponSelect1 = Keys.D1;
        Keys WeaponSelect2 = Keys.D2;
        Keys WeaponSelect3 = Keys.D3;
        Keys WeaponSelect4 = Keys.D4;
        Keys WeaponSelect5 = Keys.D5;
        Keys WeaponSelect6 = Keys.D6;
        Keys WeaponSelect7 = Keys.D7;
        Keys WeaponSelect8 = Keys.D8;
        Keys WeaponSelect9 = Keys.D9;
        Keys WeaponSelect10 = Keys.D0;


        public HenryInput(){
            input.Add("Mouse1", false);
            input.Add("Mouse2", false);
            input.Add("Pause", false);

            input.Add("PlayerUp", false);
            input.Add("PlayerDown", false);
            input.Add("PlayerLeft", false);
            input.Add("PlayerRight", false);
            input.Add("PlayerFire1", false);
            input.Add("PlayerFire2", false);

            input.Add("BossUp", false);
            input.Add("BossDown", false);
            input.Add("BossLeft", false);
            input.Add("BossRight", false);
            input.Add("BossFire1", false);
            input.Add("BossFire2", false);

            input.Add("WeaponSelect1", false);
            input.Add("WeaponSelect2", false);
            input.Add("WeaponSelect3", false);
            input.Add("WeaponSelect4", false);
            input.Add("WeaponSelect5", false);
            input.Add("WeaponSelect6", false);
            input.Add("WeaponSelect7", false);
            input.Add("WeaponSelect8", false);
            input.Add("WeaponSelect9", false);
            input.Add("WeaponSelect10", false);

        }

        public bool GetKey(string keyName){
            try { return input[keyName]; }
            catch { return false; }
        }

        public Vector2 GetCursor(){
            return new Vector2(MouseX, MouseY);
        }

        public int GetMouseWheelDelta(){
            return (currMouseWheel - prevMouseWheel) / 120;
        }

        public void Update(GameTime gt){
            //check battlefield mode
            bool bossMode = false;
            if(RNSEB.TheBattlefield != null)
                 bossMode = RNSEB.TheBattlefield.BossMode;

            //check mouse
            MouseState currMState = Mouse.GetState();
            MouseX = currMState.X;
            MouseY = currMState.Y;
            prevMouseWheel = currMouseWheel;
            currMouseWheel = currMState.ScrollWheelValue;
            input["Mouse1"] = (currMState.LeftButton == ButtonState.Pressed) ? true : false;
            input["Mouse2"] = (currMState.RightButton == ButtonState.Pressed) ? true : false;
            input["PlayerFire1"] = bossMode ? false : input["Mouse1"];
            input["PlayerFire2"] = bossMode ? false : input["Mouse2"];
            input["BossFire1"] = bossMode ? input["Mouse1"] : false;
            input["BossFire2"] = bossMode ? input["Mouse2"] : false;

            //check keyboard
            KeyboardState currKState = Keyboard.GetState();
            input["Pause"] = currKState.IsKeyDown(PauseGame);

            input["PlayerUp"] = bossMode ? false : currKState.IsKeyDown(PlayerUp);
            input["PlayerDown"] = bossMode ? false : currKState.IsKeyDown(PlayerDown);
            input["PlayerLeft"] = bossMode ? false : currKState.IsKeyDown(PlayerLeft);
            input["PlayerRight"] = bossMode ? false : currKState.IsKeyDown(PlayerRight);

            input["BossUp"] = bossMode ? currKState.IsKeyDown(BossUp) : false;
            input["BossDown"] = bossMode ? currKState.IsKeyDown(BossDown) : false;
            input["BossLeft"] = bossMode ? currKState.IsKeyDown(BossLeft) : false;
            input["BossRight"] = bossMode ? currKState.IsKeyDown(BossRight) : false;

            input["WeaponSelect1"] = currKState.IsKeyDown(WeaponSelect1);
            input["WeaponSelect2"] = currKState.IsKeyDown(WeaponSelect2);
            input["WeaponSelect3"] = currKState.IsKeyDown(WeaponSelect3);
            input["WeaponSelect4"] = currKState.IsKeyDown(WeaponSelect4);
            input["WeaponSelect5"] = currKState.IsKeyDown(WeaponSelect5);
            input["WeaponSelect6"] = currKState.IsKeyDown(WeaponSelect6);
            input["WeaponSelect7"] = currKState.IsKeyDown(WeaponSelect7);
            input["WeaponSelect8"] = currKState.IsKeyDown(WeaponSelect8);
            input["WeaponSelect9"] = currKState.IsKeyDown(WeaponSelect9);
            input["WeaponSelect10"] = currKState.IsKeyDown(WeaponSelect10);
        }

        public bool MouseIsIn(Vector2 pos, Vector2 area){ //helper function for button class (should probably be moved there)
            if (MouseX < pos.X + area.X / 2
                && MouseX > pos.X - area.X / 2
                && MouseY < pos.Y + area.Y / 2
                && MouseY > pos.Y - area.Y / 2)
                return true;
            return false;
        }

    }
}
