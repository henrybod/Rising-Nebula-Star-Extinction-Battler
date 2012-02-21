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
        public static bool Up = false, Down = false, Left = false, Right = false;
        public static bool One = false, Two = false, Three = false;
        //temporary
        public static bool UpBoss = false, DownBoss = false, LeftBoss = false, RightBoss = false;
        //todo: add mouse wheel

        public static void Update(GameTime gt){
            //check mouse
            MouseState currMState = Mouse.GetState();
            MouseX = currMState.X;
            MouseY = currMState.Y;
            M1 = (currMState.LeftButton == ButtonState.Pressed) ? true : false;
            M2 = (currMState.RightButton == ButtonState.Pressed) ? true : false;
            //add mouse wheel later

            //check keyboard
            KeyboardState currKState = Keyboard.GetState();
            Up = currKState.IsKeyDown(Keys.W);
            Down = currKState.IsKeyDown(Keys.S);
            Left = currKState.IsKeyDown(Keys.A);
            Right = currKState.IsKeyDown(Keys.D);
            One = currKState.IsKeyDown(Keys.D1);
            Two = currKState.IsKeyDown(Keys.D2);
            Three = currKState.IsKeyDown(Keys.D3);
            //temporary
            UpBoss = currKState.IsKeyDown(Keys.Up);
            DownBoss = currKState.IsKeyDown(Keys.Down);
            LeftBoss = currKState.IsKeyDown(Keys.Left);
            RightBoss = currKState.IsKeyDown(Keys.Right);
        }

        public static bool MouseIsIn(Vector2 pos, Vector2 area){
            if (MouseX < pos.X + area.X / 2
                && MouseX > pos.X - area.X / 2
                && MouseY < pos.Y + area.Y / 2
                && MouseY > pos.Y - area.Y / 2)
                return true;
            return false;
        }
    }
}
