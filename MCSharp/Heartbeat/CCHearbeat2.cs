/*
    Copyright 2012 MCForge
 
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
using MCSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Threading;
namespace MCSharp
{
    public sealed class ClassiCubeBeat : IBeat
    {
        public bool UrlSaid = false;
        public HttpWebRequest request;

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
            return "&port=" + Properties.ServerPort +
                "&max=" + Properties.MaxPlayers +
                "&name=" + Uri.EscapeDataString(Properties.ServerName) +
                "&public=" + Properties.PublicServer +
                "&version=7" +
                "&salt=" + Server.salt +
                "&users=" + Player.number +
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
                if (String.IsNullOrEmpty(Server.salt) || !newHash.Equals(Server.salt))
                {

                    Server.salt = newHash;
                    Server.CCURL = line;
                    File.WriteAllText("ccexternalurl.txt", line);
                    {
                        Logger.Log("ClassiCube URL found: " + line);
                    }
                }
            }
        }
    }
}
