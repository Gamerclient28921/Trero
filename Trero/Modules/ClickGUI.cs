﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trero.ClientBase;
using Trero.ClientBase.EntityBase;
using Trero.ClientBase.UIBase;

namespace Trero.Modules
{
    class ClickGUI : Module
    {
        public ClickGUI() : base("ClickGUI", (char)(int)Keys.Insert, "Visual", true) { } // Not defined
        public override void onEnable()
        {
            if (Overlay.handle != null)
            {
                Overlay.handle.Invoke((MethodInvoker)delegate {
                    foreach (Control ct in Overlay.handle.Controls)
                    {
                        if (Overlay.handle == null && ct.Tag.ToString() == "Category")
                            ct.Visible = true;
                    }
                });
            }
            base.onEnable();
        }
        public override void onDisable()
        {
            if (Overlay.handle != null)
            {
                Overlay.handle.Invoke((MethodInvoker)delegate {
                    foreach (Control ct in Overlay.handle.Controls)
                    {
                        if (Overlay.handle == null && ct.Tag.ToString() == "Category")
                            ct.Visible = false;
                    }
                });
            }
            base.onDisable();
        }
    }
}
