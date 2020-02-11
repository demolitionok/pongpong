using System;
using System.Collections;
using System.Collections.Generic;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace pongpong
{
    public interface IVertexContaining
    {
        void InitVertexes();
    }
    public class BaseObject
    {
        public float speed;
        public Vector2f startPos;
        public Vector2f dirVector = new Vector2f(0f, 0f);
        public List<Vector2f> vertexes;
        public Vector2f shapeSize;
        public Shape shape;

        protected virtual void MoveObject()
        {
            Vector2f velocity = dirVector * speed;
            shape.Position += velocity;
        }
    }

    public class Obstacle : BaseObject, IVertexContaining
    {
        public RectangleShape obstacleShape;
        public void InitVertexes()
        {
            vertexes.Add(obstacleShape.Position);
            vertexes.Add(new Vector2f(shapeSize.X + obstacleShape.Position.X, obstacleShape.Position.Y));
            vertexes.Add(shapeSize + obstacleShape.Position);
            vertexes.Add(new Vector2f(obstacleShape.Position.X, shapeSize.Y + obstacleShape.Position.Y));
            /*
             *    [0]-------------[1]
             *    |                |
             *    |    obstacle    |
             *    |                |
             *    [3]-------------[2]
             * 
             */
        }
        public Obstacle(Vector2f startPos, Vector2f shapeSize)
        {
            this.startPos = startPos;
            this.shapeSize = shapeSize;
            
            obstacleShape = new RectangleShape(shapeSize);
            obstacleShape.Position = startPos;
            shape = obstacleShape;
        }
    }

    public class Puck : BaseObject, IVertexContaining
    {
        public CircleShape puckShape;
        protected override void MoveObject()
        {
            base.MoveObject();
        }
        public void InitVertexes()
        {
            vertexes.Add(new Vector2f(0.5f * shapeSize.X + puckShape.Position.X, puckShape.Position.Y));
            vertexes.Add(new Vector2f(shapeSize.X + puckShape.Position.X, 0.5f * shapeSize.Y + puckShape.Position.Y));
            vertexes.Add(new Vector2f(0.5f * shapeSize.X + puckShape.Position.X, shapeSize.Y + puckShape.Position.Y));
            vertexes.Add(new Vector2f(puckShape.Position.X, 0.5f * shapeSize.Y + puckShape.Position.Y));
            /*
             *    ./----[0]----\
             *    |            |
             *  [3]    Puck    [1]
             *    |            |
             *     \---[2]---/
             *
             *     It should look like a circle.
             */
        }

    }

    public class Player : BaseObject, IVertexContaining
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

        public void InitVertexes()
        {
            vertexes.Add(playerShape.Position);
            vertexes.Add(new Vector2f(shapeSize.X + playerShape.Position.X, playerShape.Position.Y));
            vertexes.Add(shapeSize + playerShape.Position);
            vertexes.Add(new Vector2f(playerShape.Position.X, shapeSize.Y + playerShape.Position.Y));
            /*
             *    [0]-------[1]
             *    |           |
             *    |   Player  |
             *    |           |
             *    [3]-------[2]
             * 
             */
        }

        public Player(Vector2f shapeSize, float speed, Vector2f startPos, List<Keyboard.Key> playerKeys)
        {
            playerShape = new RectangleShape(shapeSize);
            this.startPos = startPos;
            playerShape.Position = startPos;

            this.speed = speed;
            this.playerKeys = playerKeys;
            this.shapeSize = shapeSize;
            shape = playerShape;
        }
    }

    public class Game
    {
        public RenderWindow window;
        public static List<Keyboard.Key> Player1_Keys = new List<Keyboard.Key>();
        public static List<Keyboard.Key> Player2_Keys = new List<Keyboard.Key>();
        public Player Player1 = new Player(new Vector2f(30f, 20f), 5f, new Vector2f(400, 550), Player1_Keys);
        public Player Player2 = new Player(new Vector2f(30f, 20f), 5f, new Vector2f(400, 50), Player2_Keys);
        public List<IVertexContaining> VertexContainings = new List<IVertexContaining>();
        public List<BaseObject> BaseObjects = new List<BaseObject>();

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            var window = (Window) sender;
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
        }

        public void InitObstacles()
        {
            BaseObjects.Add(Player1);
            BaseObjects.Add(Player2);
            BaseObjects.Add(new Obstacle(new Vector2f(0f, 0f), new Vector2f(50f, 600f)));
            BaseObjects.Add(new Obstacle(new Vector2f(750f, 0f), new Vector2f(50f, 600f)));
        }

        public void InitKeys()
        {
            Player1_Keys.Add(Keyboard.Key.F);
            Player1_Keys.Add(Keyboard.Key.D);
            Player2_Keys.Add(Keyboard.Key.Right);
            Player2_Keys.Add(Keyboard.Key.Left);
        }

        public void DetectCollision(List<IVertexContaining> figures)
        {
        }
        public void Logic()
        {
        }

        public void MakeGraphic()
        {
            InitObstacles();
            foreach (BaseObject figure in BaseObjects)
            {
                window.Draw(figure.shape);
            }
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