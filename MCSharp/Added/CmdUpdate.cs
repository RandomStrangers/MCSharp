
namespace MCSharp
{
    public class CmdUpdate : Command
    {

        // Constructor
        public CmdUpdate(CommandGroup g, GroupEnum group, string name) : base(g, group, name) { blnConsoleSupported = true; /* By default no console support*/ }

        // Command usage help
        public override void Help(Player p)
        {
            p.SendMessage("/update - Updates the server");
        }

        // Code to run when used by a player
        public override void Use(Player p, string message)
        {
            MCSharpUpdater.Program.Main(null);
        }
    }
}