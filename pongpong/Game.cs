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

    public class Game
    {
        private Clock clock;
        private bool gameStopped;
        
        private RenderWindow window;
        private List<Keyboard.Key> Player1_Keys;
        private List<Keyboard.Key> Player2_Keys;

        Text winnerName;
        private Player Player1;
        private Player Player2;
        
        private ScoreArea ScoreArea1;
        private ScoreArea ScoreArea2;
        
        private Puck puck;
        
        private List<IVertexContaining> VertexContainings;
        private List<BaseObject> BaseObjects;

        public void FartherstVertex()
        {
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            var window = (Window) sender;
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
        }

        private void InitVariables()
        {
            clock = new Clock();
            gameStopped = false;
            winnerName = new Text() {FillColor = Color.White};
            Player1 = new Player(new Vector2f(50f, 20f), 0.2f, new Vector2f(400, 550), Player1_Keys, "Player1"); 
            Player2 = new Player(new Vector2f(50f, 20f), 0.2f, new Vector2f(400, 50), Player2_Keys, "Player2");
        
            ScoreArea1 = new ScoreArea(new Vector2f(50f, 0f), new Vector2f(700f, 25f), Player1);
            ScoreArea2 = new ScoreArea(new Vector2f(50f, 575f), new Vector2f(700f, 25f), Player2);
        
            puck = new Puck(new Vector2f(10f,10f), 0.1f,new Vector2f(-1f,0.5f), new Vector2f(400,300), Color.Red);
        
            VertexContainings = new List<IVertexContaining>();
            BaseObjects = new List<BaseObject>();
        }

        private void InitText()
        {
            winnerName.Font = new Font("BiolinumV1.ttf");
            winnerName.CharacterSize = 22;
            winnerName.FillColor = Color.White;    
            winnerName.Position = new Vector2f(400,300);          
        }

        private void InitObstacles()
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

        private void InitEvents()
        {
            window.KeyPressed += Window_KeyPressed;
            window.KeyPressed += Player1.Player_KeyPressed;
            window.KeyReleased += Player1.Player_KeyReleased;
            window.KeyPressed += Player2.Player_KeyPressed;
            window.KeyReleased += Player2.Player_KeyReleased;
        }

        private void InitKeys()
        {
            Player1_Keys = new List<Keyboard.Key>();
            Player2_Keys = new List<Keyboard.Key>();
            
            Player1_Keys.Add(Keyboard.Key.F);
            Player1_Keys.Add(Keyboard.Key.D);
            Player2_Keys.Add(Keyboard.Key.Right);
            Player2_Keys.Add(Keyboard.Key.Left);
        }

        private void Initialization()
        {
            InitKeys();
            InitVariables();
            InitText();
            InitObstacles();
            InitEvents();
        }
        private Collision DetectCollision(BaseObject baseObject, List<BaseObject> figures)
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

        private void GetWinner(Collision coll)
        {
            if (coll.collObject?.GetType() == typeof(ScoreArea))
            {
                ScoreArea temp = (ScoreArea)coll.collObject;
                winnerName.DisplayedString = temp.owner.name;
                gameStopped = true;
            }
        }

        private void Logic()
        {
            Collision puckCollision = DetectCollision(puck, BaseObjects);
            puck.MoveObject(puckCollision);
            Player1.MoveObject();
            Player2.MoveObject();
            GetWinner(puckCollision);
        }

        private void MakeGraphic()
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
}