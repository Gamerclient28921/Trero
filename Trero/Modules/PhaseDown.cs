﻿using System.Windows.Forms;
using Trero.ClientBase;
using Trero.ClientBase.UIBase;

namespace Trero.Modules
{
    class PhaseDown : Module
    {
        public PhaseDown() : base("PhaseDown", (char)0x07, "Flies") { } // Not defined
        public override void onTick()
        {
            Game.velocity = Base.Vec3();

            Vector3 newPos = Game.position;
            newPos.y -= 0.01f;
            Game.position = newPos;
        }
    }
}