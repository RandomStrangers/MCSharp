using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MCSharp
{
    class CmdShutdown : Command
    {
        // Constructor
        public CmdShutdown(CommandGroup g, GroupEnum group, string name) : base(g, group, name) { blnConsoleSupported = true; /* By default no console support*/ }

        // Command usage help
        public override void Help(Player p)
        {
            p.SendMessage("");
        }

        // Code to run when used by a player
        public override void Use(Player p, string message)
        {
            Logger.Log("Server shutdown requested by " + p.name);
            Server.ForceExit();
            //Environment.FailFast("Shutting down!");
            //Application.Exit();

        }

        // Code to run when used by the console
        public override void Use(string message)
        {
            Logger.Log("Server shutdown requested by Console");
            Server.ForceExit();
            //Environment.FailFast("Shutting down!");
            //Application.Exit();
        }
    }
}
