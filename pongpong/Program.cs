using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Net.WebSockets;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace pongpong
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var game = new Game();
            game.Run();
        }
    }
}