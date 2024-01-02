/*
    Copyright 2012 MCLawl
 
    Dual-licensed under the	Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
    
    http://www.opensource.org/licenses/ecl2.php
    http://www.gnu.org/licenses/gpl-3.0.html
    
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
 */
using MCSharp.Heartbeat;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using static MCSharp.Player;
namespace MCSharp
{
    public sealed class ClassiCubeBeat : IBeat
    {
        public int Attempts { get { return _attempts; } }
        protected int _attempts = 0;
        public static int MissedBeats { get { return Instance.Attempts; } }

        static BackgroundWorker worker;
        public static bool UrlSaid = false;

        static ClassiCubeBeat instance;
        public static ClassiCubeBeat Instance
        {
            get
            {
                if (instance == null)
                {
                    Init();
                }
                return instance;
            }

            set { instance = value; }
        }

        static string _hash = null;
        public static string Hash { get { return _hash; } }

        public static void Init()
        {
            if (instance == null)
            {
                instance = new ClassiCubeBeat();
                worker = new BackgroundWorker();
            }
        }

        public string URL
        {
            get
            {
                return "https://www.classicube.net/heartbeat.jsp";
            }
        }

        public bool Persistance
        {
            get { return true; }
        }

        public string Prepare()
        {
            // Increment the attempts
            _attempts++;

            return "&port=" + Properties.ServerPort +
                "&max=" + Properties.MaxPlayers +
                "&name=" + Uri.EscapeDataString(Properties.ServerName) +
                "&public=" + Properties.PublicServer +
                "&version=7" +
                 "&users=" + number +
                "&salt=" + Uri.EscapeDataString(Properties.GenerateSalt()) +
                "&software=" + Server.SoftwareNameVersioned +
                 "&web=true";

        }
        public void OnResponse(string line)
        {

            // Only run the code below if we receive a response
            if (!String.IsNullOrEmpty(line.Trim()))
            {
                string newHash = line.Substring(line.LastIndexOf('/') + 1);

                // Run this code if we don't already have a hash or if the hash has changed
                if (String.IsNullOrEmpty(Server.Hash) || !newHash.Equals(Server.Hash))
                {
                    Server.Hash = newHash;
                    Server.CCURL = line;
                    Server.s.UpdateCCUrl(Server.CCURL);
                    File.WriteAllText("ccexternalurl.txt", Server.CCURL);
                    if (UrlSaid == false)
                    {
                        Logger.Log("ClassiCube URL found: " + Server.CCURL);
                        UrlSaid = true;
                    }
                }
            }
        }
    }
}
