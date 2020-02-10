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
        public float playerSpeed;
        public float shapeSize;
        public RectangleShape playerShape;
        public List<Keyboard.Key> playerKeys;

        public void Player_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == playerKeys[0])
            {
                
            }
        }

        public Player(float shapeSize,float playerSpeed, List<Keyboard.Key> playerKeys)
        {
            this.playerKeys = playerKeys;
            this.shapeSize = shapeSize;
            playerShape = new RectangleShape(new Vector2f(shapeSize, 10f));
        }
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
            game.Run();
        }
    }
}