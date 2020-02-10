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
        public Vector2f dirVector = new Vector2f(0f, 0f);
        public float shapeSize;
        public RectangleShape playerShape;
        public List<Keyboard.Key> playerKeys;

        public void Player_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == playerKeys[0])
            {
                dirVector = new Vector2f(1,0);
                movePlayer();
            }
            if (e.Code == playerKeys[1])
            {
                dirVector = new Vector2f(-1,0);
                movePlayer();
            }
        }
        public void movePlayer()
        {
            Vector2f velocity = dirVector*playerSpeed;
            playerShape.Position += velocity;
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
        public static List<Keyboard.Key> Player1_Keys = new List<Keyboard.Key>();
        public static List<Keyboard.Key> Player2_Keys = new List<Keyboard.Key>();
        public Player Player1 = new Player(15f, 5f, Player1_Keys);
        public Player Player2 = new Player(15f, 5f, Player2_Keys);
        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            var window = (Window)sender;
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
        }

        public void InitKeys()
        {
            Player1_Keys.Add(Keyboard.Key.F);
            Player1_Keys.Add(Keyboard.Key.D);
            Player2_Keys.Add(Keyboard.Key.Right);
            Player2_Keys.Add(Keyboard.Key.Left);
        }

        public void Logic()
        {
            
        }

        public void MakeGraphic()
        {
            
        }
        public void Run()
        {
            var window = new RenderWindow(new VideoMode(800, 600), "dingdong");
            window.KeyPressed += Window_KeyPressed;
            window.KeyPressed += Player1.Player_KeyPressed;
            window.KeyPressed += Player2.Player_KeyPressed;
            
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