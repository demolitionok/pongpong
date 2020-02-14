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
    public interface IVertexContaining
    {
        void InitVertexes();
    }

    public class Collision
    {
        public Vector2f collVector;
        public BaseObject collObject;

        public Collision(Vector2f collVector, BaseObject collObject)
        {
            this.collVector = collVector;
            this.collObject = collObject;
        }
    }

    public class BaseObject : IVertexContaining
    {
        public float speed = 0;
        public Vector2f startPos;
        public Vector2f dirVector = new Vector2f(0f, 0f);
        public Vector2f velocity;
        public Vector2f[] vertexes = new Vector2f[4];
        public Vector2f shapeSize;
        public Shape shape;

        
        public virtual void InitVertexes()
        {
            vertexes[0] = shape.Position;
            vertexes[1] = new Vector2f(shapeSize.X + shape.Position.X, shape.Position.Y);
            vertexes[2] = shapeSize + shape.Position;
            vertexes[3] = new Vector2f(shape.Position.X, shapeSize.Y + shape.Position.Y);
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
            velocity = dirVector * speed;
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
    public class ScoreArea : BaseObject //kak nasledovatsa ot Obstacle perenyav konstuktor
    {
        public Player owner;
        public ScoreArea(Vector2f startPos, Vector2f shapeSize, Player owner)
        {
            this.startPos = startPos;
            this.shapeSize = shapeSize;
            this.owner = owner;
            
            shape = new RectangleShape(shapeSize);
            shape.Position = startPos;
            shape.FillColor = Color.Green;
            InitVertexes();
        }
    }
    public class Puck : BaseObject, IVertexContaining
    {
        public new void InitVertexes()
        {
            vertexes[0] = new Vector2f(/*0.5f * */shapeSize.X + shape.Position.X, shape.Position.Y);
            vertexes[1] = new Vector2f(2*shapeSize.X + shape.Position.X, /*0.5f * */shapeSize.Y + shape.Position.Y);
            vertexes[2] = new Vector2f(/*0.5f * */shapeSize.X + shape.Position.X, 2*shapeSize.Y + shape.Position.Y);
            vertexes[3] = new Vector2f(shape.Position.X, /*0.5f * */shapeSize.Y + shape.Position.Y);
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

        public new void MoveObject(Collision coll)
        {
            velocity = dirVector * speed;
            if (coll.collVector == new Vector2f(0,0))
            {
                
                shape.Position += velocity;
            }
            else
            {
                if (coll.collVector == vertexes[0] || coll.collVector == vertexes[2])
                {
                    dirVector = new Vector2f(dirVector.X, -dirVector.Y);
                }
                else if (coll.collVector == vertexes[1] || coll.collVector == vertexes[3])
                {
                    dirVector = new Vector2f(-dirVector.X, dirVector.Y);
                }

                shape.Position -= 3*velocity;
            }
            InitVertexes();
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
        public string name;

        public void Player_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == playerKeys[0])
            {
                dirVector = new Vector2f(1, 0);
            }

            if (e.Code == playerKeys[1])
            {
                dirVector = new Vector2f(-1, 0);
            }
        }

        public void Player_KeyReleased(object sender, KeyEventArgs e)
        {
            dirVector = new Vector2f(0, 0);
        }

        public Player(Vector2f shapeSize, float speed, Vector2f startPos, List<Keyboard.Key> playerKeys, string name)
        {
            this.name = name;
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
        private Clock clock;
        public bool gameStopped;
        
        public RenderWindow window;
        public static List<Keyboard.Key> Player1_Keys;
        public static List<Keyboard.Key> Player2_Keys;

        Text winnerName;
        public static Player Player1;
        public static Player Player2;
        
        public ScoreArea ScoreArea1;
        public ScoreArea ScoreArea2;
        
        public Puck puck;
        
        public List<IVertexContaining> VertexContainings;
        public List<BaseObject> BaseObjects;

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            var window = (Window) sender;
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
        }

        public void InitVariables()
        {
            clock = new Clock();
            gameStopped = false;
            winnerName = new Text() {FillColor = Color.White};
            Player1 = new Player(new Vector2f(50f, 20f), 1f, new Vector2f(400, 550), Player1_Keys, "Player1"); 
            Player2 = new Player(new Vector2f(50f, 20f), 1f, new Vector2f(400, 50), Player2_Keys, "Player2");
        
            ScoreArea1 = new ScoreArea(new Vector2f(50f, 0f), new Vector2f(700f, 25f), Player1);
            ScoreArea2 = new ScoreArea(new Vector2f(50f, 575f), new Vector2f(700f, 25f), Player2);
        
            puck = new Puck(new Vector2f(10f,10f), 0.1f,new Vector2f(-1f,0.5f), new Vector2f(400,300), Color.Red);
        
            VertexContainings = new List<IVertexContaining>();
            BaseObjects = new List<BaseObject>();
        }

        public void InitText()
        {
            winnerName.Font = new Font("BiolinumV1.ttf");
            winnerName.CharacterSize = 22;
            winnerName.FillColor = Color.White;    
            winnerName.Position = new Vector2f(400,300);          
        }

        public void InitObstacles()
        {
            BaseObjects.Add(Player1);
            BaseObjects.Add(Player2);
            BaseObjects.Add(ScoreArea1);
            BaseObjects.Add(ScoreArea2);
            BaseObjects.Add(new Obstacle(new Vector2f(0f, 0f), new Vector2f(50f, 600f)));
            BaseObjects.Add(new Obstacle(new Vector2f(200f, 275f), new Vector2f(100f, 50f)));
            BaseObjects.Add(new Obstacle(new Vector2f(500f, 275f), new Vector2f(100f, 50f)));
            BaseObjects.Add(new Obstacle(new Vector2f(750f, 0f), new Vector2f(50f, 600f)));
        }

        public void InitEvents()
        {
            window.KeyPressed += Window_KeyPressed;
            window.KeyPressed += Player1.Player_KeyPressed;
            window.KeyReleased += Player1.Player_KeyReleased;
            window.KeyPressed += Player2.Player_KeyPressed;
            window.KeyReleased += Player2.Player_KeyReleased;
        }

        public void InitKeys()
        {
            Player1_Keys = new List<Keyboard.Key>();
            Player2_Keys = new List<Keyboard.Key>();
            
            Player1_Keys.Add(Keyboard.Key.F);
            Player1_Keys.Add(Keyboard.Key.D);
            Player2_Keys.Add(Keyboard.Key.Right);
            Player2_Keys.Add(Keyboard.Key.Left);
        }

        public void Initialization()
        {
            InitKeys();
            InitVariables();
            InitText();
            InitObstacles();
            InitEvents();
        }
        public Collision DetectCollision(BaseObject baseObject, List<BaseObject> figures)
        {
            /*Puck tempBaseObject = baseObject;
            tempBaseObject.shape.Position += tempBaseObject.velocity;
            tempBaseObject.InitVertexes();*/
            foreach (var figure in figures)
            {
                for(int i = 0; i < baseObject.vertexes.Length; i++)
                {
                    if(
                        (
                        (baseObject.vertexes[i].X >= figure.vertexes[0].X || baseObject.vertexes[i].X >= figure.vertexes[3].X)
                        &&
                        (baseObject.vertexes[i].X <= figure.vertexes[1].X || baseObject.vertexes[i].X <= figure.vertexes[2].X)
                        )
                        &&
                        (
                        (baseObject.vertexes[i].Y >= figure.vertexes[0].Y || baseObject.vertexes[i].Y >= figure.vertexes[3].Y)
                         &&
                        (baseObject.vertexes[i].Y <= figure.vertexes[1].Y || baseObject.vertexes[i].Y <= figure.vertexes[2].Y)
                        )
                        )
                    {
                        return new Collision(baseObject.vertexes[i], figure);
                    }
                }   
            }
            return new Collision(new Vector2f(0,0), null);
        }

        public void GetWinner(Collision coll)
        {
            if (coll.collObject?.GetType() == typeof(ScoreArea))
            {
                ScoreArea temp = (ScoreArea)coll.collObject;
                winnerName.DisplayedString = temp.owner.name;
                gameStopped = true;
            }
        }

        public void Logic()
        {
            Collision puckCollision = DetectCollision(puck, BaseObjects);
            puck.MoveObject(puckCollision);
            Player1.MoveObject();
            Player2.MoveObject();
            GetWinner(puckCollision);
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
            Initialization();
            
            while (window.IsOpen)
            {
                if (gameStopped  == false)
                {
                    Logic();
                    MakeGraphic();
                    window.DispatchEvents();
                    window.Display();
                    window.Clear();
                    
                }
                else
                {
                    window.Draw(winnerName);
                    window.DispatchEvents();
                    window.Display();
                    window.Clear();
                    Time elapsed = clock.ElapsedTime;
                    if(elapsed >= Time.FromSeconds(10))
                    {
                        Initialization();
                    }
                }
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