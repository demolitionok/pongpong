using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace pongpong
{
    public interface IVertexContaining
    {
        void InitVertexes();
    }
    public class BaseObject : IVertexContaining
    {
        public float speed;
        public Vector2f startPos;
        public Vector2f dirVector = new Vector2f(0f, 0f);
        public List<Vector2f> vertexes = new List<Vector2f>();
        public Vector2f shapeSize;
        public Shape shape;

        
        public void InitVertexes()
        {
            vertexes.Add(shape.Position);
            vertexes.Add(new Vector2f(shapeSize.X + shape.Position.X, shape.Position.Y));
            vertexes.Add(shapeSize + shape.Position);
            vertexes.Add(new Vector2f(shape.Position.X, shapeSize.Y + shape.Position.Y));
            /*
             *    [0]--------[1]
             *    |            |
             *    | BaseObject |
             *    |            |
             *    [3]--------[2]
             * 
             */
        }

        public virtual void MoveObject()
        {
            Vector2f velocity = dirVector * speed;
            shape.Position += velocity;
            InitVertexes();
        }
    }

    public class Obstacle : BaseObject
    {
        public Obstacle(Vector2f startPos, Vector2f shapeSize)
        {
            this.startPos = startPos;
            this.shapeSize = shapeSize;
            
            shape = new RectangleShape(shapeSize);
            shape.Position = startPos;
            InitVertexes();
        }
    }

    public class Puck : BaseObject, IVertexContaining
    {
        public new void InitVertexes()
        {
            vertexes.Add(new Vector2f(0.5f * shapeSize.X + shape.Position.X, shape.Position.Y));
            vertexes.Add(new Vector2f(shapeSize.X + shape.Position.X, 0.5f * shapeSize.Y + shape.Position.Y));
            vertexes.Add(new Vector2f(0.5f * shapeSize.X + shape.Position.X, shapeSize.Y + shape.Position.Y));
            vertexes.Add(new Vector2f(shape.Position.X, 0.5f * shapeSize.Y + shape.Position.Y));
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
        public Puck(Vector2f shapeSize, float speed, Vector2f dirVector, Vector2f startPos, Color color)
        {
            this.dirVector = dirVector;
            this.startPos = startPos;
            this.speed = speed;
            this.shapeSize = shapeSize;
            
            shape = new CircleShape(shapeSize.X)
            {
                FillColor = color
            };
            shape.Position = startPos;
            InitVertexes();
        }
    }

    public class Player : BaseObject
    {
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

        public Player(Vector2f shapeSize, float speed, Vector2f startPos, List<Keyboard.Key> playerKeys)
        {
            shape = new RectangleShape(shapeSize);
            this.startPos = startPos;
            shape.Position = startPos;

            this.speed = speed;
            this.playerKeys = playerKeys;
            this.shapeSize = shapeSize;
            InitVertexes();
        }
    }

    public class Game
    {
        public RenderWindow window;
        public static List<Keyboard.Key> Player1_Keys = new List<Keyboard.Key>();
        public static List<Keyboard.Key> Player2_Keys = new List<Keyboard.Key>();
        
        public Player Player1 = new Player(new Vector2f(30f, 20f), 5f, new Vector2f(400, 550), Player1_Keys);
        public Player Player2 = new Player(new Vector2f(30f, 20f), 5f, new Vector2f(400, 50), Player2_Keys);
        public Puck puck = new Puck(new Vector2f(10f,10f),0.5f,new Vector2f(1,0), new Vector2f(400,300), Color.Red);
        
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

        public bool DetectCollision(Puck puck, List<BaseObject> figures)
        {
            int count = 0;
            foreach (var figure in figures)
            {
                for(int i = 0; i < puck.vertexes.Count; i++)
                {
                    if(
                        (
                        (puck.vertexes[i].X >= figure.vertexes[0].X || puck.vertexes[i].X >= figure.vertexes[3].X)
                        &&
                        (puck.vertexes[i].X <= figure.vertexes[1].X || puck.vertexes[i].X <= figure.vertexes[2].X)
                        )
                        &&
                        (
                        (puck.vertexes[i].Y >= figure.vertexes[0].Y || puck.vertexes[i].Y >= figure.vertexes[3].Y)
                         &&
                        (puck.vertexes[i].Y <= figure.vertexes[1].Y || puck.vertexes[i].Y <= figure.vertexes[2].Y)
                        )
                        )
                    {
                        window.Close();
                    }
                }   
            }
            return false;
        }
        public void Logic()
        {
            InitKeys();
            InitObstacles();
            puck.MoveObject();
            DetectCollision(puck, BaseObjects);
        }

        public void MakeGraphic()
        {
            foreach (BaseObject figure in BaseObjects)
            {
                window.Draw(figure.shape);
            }
            window.Draw(puck.shape);
        }

        public void Run()
        {
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