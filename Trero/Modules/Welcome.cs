﻿#region

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#endregion

namespace Trero.Modules
{
    internal class Welcome : Module // due to set person (superlupita#4062), (552396139232624640) you now have to include this extra annoying module!
    {
        public Welcome() : base("Welcome", (char)0x07, "Other", "Display our discord in the trero debug/developer console")
        {
        } // 0x07 = no keybind

        public override void OnEnable()
        {
            Console.Clear();
            Console.WriteLine("--- Links ---\r\n" +
                "Trero: \r\n" +
                "discord.gg/Zr6RDAJp9F\r\n");
        }
    }
}