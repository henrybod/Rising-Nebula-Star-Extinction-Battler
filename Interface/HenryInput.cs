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
        Keys BossUp = Keys.Up;
        Keys BossDown = Keys.Down;
        Keys BossLeft = Keys.Left;
        Keys BossRight = Keys.Right;
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
            //check mouse
            MouseState currMState = Mouse.GetState();
            MouseX = currMState.X;
            MouseY = currMState.Y;
            input["PlayerFire1"] = input["Mouse1"] = (currMState.LeftButton == ButtonState.Pressed) ? true : false;
            input["PlayerFire2"] = input["Mouse2"] = (currMState.RightButton == ButtonState.Pressed) ? true : false;

            //check keyboard
            KeyboardState currKState = Keyboard.GetState();
            input["Pause"] = currKState.IsKeyDown(PauseGame);

            input["PlayerUp"] = currKState.IsKeyDown(PlayerUp);
            input["PlayerDown"] = currKState.IsKeyDown(PlayerDown);
            input["PlayerLeft"] = currKState.IsKeyDown(PlayerLeft);
            input["PlayerRight"] = currKState.IsKeyDown(PlayerRight);

            input["BossUp"] = currKState.IsKeyDown(BossUp);
            input["BossDown"] = currKState.IsKeyDown(BossDown);
            input["BossLeft"] = currKState.IsKeyDown(BossLeft);
            input["BossRight"] = currKState.IsKeyDown(BossRight);
            input["BossFire1"] = currKState.IsKeyDown(Keys.Space); //will change to mouse 1 later
            input["BossFire2"] = currKState.IsKeyDown(Keys.LeftAlt); //will change to mouse 2 later
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
