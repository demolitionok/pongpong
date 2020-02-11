using System;
using System.Collections;
using System.Collections.Generic;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace pongpong
{
    public abstract class BaseObject
    {
        public float speed;
        public Vector2f startPos;
        public Vector2f dirVector = new Vector2f(0f, 0f);
        public List<Vector2f> vertexses;
        public float shapeSize;
        public Shape shape;

        public abstract void InitVertexes();

        protected virtual void MoveObject()
        {
            Vector2f velocity = dirVector * speed;
            shape.Position += velocity;
        }
    }

    public class Player : BaseObject
    {
        public RectangleShape playerShape;
        public List<Keyboard.Key> playerKeys;

        public void Player_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == playerKeys[0])
            {
                dirVector = new Vector2f(1, 0);
                MoveObject();
            }

            if (e.Code == playerKeys[1])
            {
                dirVector = new Vector2f(-1, 0);
                MoveObject();
            }
        }

        protected override void MoveObject()
        {
            base.MoveObject();
        }

        public override void InitVertexes()
        {
            //vertexses.Add(playerShape.Position);
        }

        public Player(float shapeSize, float speed, Vector2f startPos, List<Keyboard.Key> playerKeys)
        {
            playerShape = new RectangleShape(new Vector2f(shapeSize, 10f));
            shape = playerShape;
            this.startPos = startPos;
            playerShape.Position = startPos;

            this.speed = speed;
            this.playerKeys = playerKeys;
            this.shapeSize = shapeSize;
        }
    }

    public class Game
    {
        public RenderWindow window;
        private static List<Keyboard.Key> Player1_Keys = new List<Keyboard.Key>();
        public static List<Keyboard.Key> Player2_Keys = new List<Keyboard.Key>();
        public Player Player1 = new Player(15f, 5f, new Vector2f(400, 550), Player1_Keys);
        public Player Player2 = new Player(15f, 5f, new Vector2f(400, 50), Player2_Keys);

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            var window = (Window) sender;
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
            window.Draw(Player1.playerShape);
            window.Draw(Player2.playerShape);
        }

        public void Run()
        {
            InitKeys();
            window = new RenderWindow(new VideoMode(800, 600), "dingdong");
            window.KeyPressed += Window_KeyPressed;
            window.KeyPressed += Player1.Player_KeyPressed;
            window.KeyPressed += Player2.Player_KeyPressed;

            while (window.IsOpen)
            {
                Logic();
                MakeGraphic();
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