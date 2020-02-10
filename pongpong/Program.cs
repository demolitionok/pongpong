using System;
using System.Collections;
using System.Collections.Generic;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace pongpong
{

    public class Player
    {
        public Shape PlayerShape;
        public List<Keyboard.Key> playerKeys;
    }

    public class Game
    {
        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            var window = (Window)sender;
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
        }
        public void MakeGraphic()
        {
            
        }
        public void Run()
        {
            var window = new RenderWindow(new VideoMode(800, 600), "dingdong");
            window.KeyPressed += Window_KeyPressed;
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Display();
                window.Clear();
            }
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var game = new Game();
            
        }
    }
}