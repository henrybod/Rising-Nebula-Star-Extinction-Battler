using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace teamstairwell.Interface {

    static class HenryInput {

        public static float MouseX = RNSEB.RESOLUTION.X / 2;
        public static float MouseY = RNSEB.RESOLUTION.Y / 2;
        public static bool M1 = false, M2 = false;
        //add other keys later

        public static void Update(GameTime gt){
            MouseState currMState = Mouse.GetState();
            MouseX = currMState.X;
            MouseY = currMState.Y;
            M1 = (currMState.LeftButton == ButtonState.Pressed) ? true : false;
            M2 = (currMState.RightButton == ButtonState.Pressed) ? true : false;
            //add mouse wheel later
        }

        public static bool MouseIsIn(Vector2 pos, Vector2 area){
            if (MouseX < pos.X + area.X / 2
                && MouseX > pos.X - area.X / 2
                && MouseY < pos.Y + area.X / 2
                && MouseY > pos.Y - area.Y / 2)
                return true;
            return false;
        }
    }
}
