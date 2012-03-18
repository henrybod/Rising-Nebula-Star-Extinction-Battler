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
        }

        public bool GetKey(string keyName){
            try { return input[keyName]; }
            catch { return false; }
        }

        public Vector2 GetCursor(){
            return new Vector2(MouseX, MouseY);
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
