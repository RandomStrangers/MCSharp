/*
	Copyright © 2009-2014 MCSharp team (Modified for use with MCZall/MCLawl/MCSharp/MCSharp-Redux)
	
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
using MCSharp.World;
using MCSharp;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MCSharp
{
    public static class SrvProperties
    {
        public static void Load(string givenPath, bool skipsalt = false)
        {
            /*
if (!skipsalt)
{
    Server.salt = "";
    string rndchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    Random rnd = new Random();
    for (int i = 0; i < 16; ++i) { Server.salt += rndchars[rnd.Next(rndchars.Length)]; }
}*/
            if (!skipsalt)
            {
                bool gotSalt = false;
                if (File.Exists("text/salt.txt"))
                {
                    string salt = File.ReadAllText("text/salt.txt");
                    if (salt.Length != 16)
                        Logger.Log("Invalid salt in salt.txt!");
                    else
                    {
                        Server.salt = salt;
                        gotSalt = true;
                    }

                }
                if (!gotSalt)
                {
                    RandomNumberGenerator prng = RandomNumberGenerator.Create();
                    StringBuilder sb = new StringBuilder();
                    byte[] oneChar = new byte[1];
                    while (sb.Length < 16)
                    {
                        prng.GetBytes(oneChar);
                        if (Char.IsLetterOrDigit((char)oneChar[0]))
                        {
                            sb.Append((char)oneChar[0]);
                        }
                    }
                    Server.salt = sb.ToString();
                }
            }
        }
    }
}