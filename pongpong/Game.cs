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
        
        public RenderWindow gameWindow;
        private List<Keyboard.Key> Player1_Keys;
        private List<Keyboard.Key> Player2_Keys;

        public Player Winner;
        private Player Player1;
        public Vector2f shapeSize1 = new Vector2f(50f, 20f);
        private Player Player2;
        public Vector2f shapeSize2 = new Vector2f(50f, 20f);
        
        private ScoreArea ScoreArea1;
        private ScoreArea ScoreArea2;
        
        private Puck puck;
        
        private List<IVertexContaining> VertexContainings;
        private List<BaseObject> BaseObjects;
        private List<Text> Texts;

        public void FartherstVertex()
        {
        }

        private void GameWindowKeyPressed(object sender, KeyEventArgs e)
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
            Winner = null;
            Player1 = new Player("Player1", 0.2f, shapeSize1, new Vector2f(400, 550), Player1_Keys, Player2); 
            Player2 = new Player("Player2", 0.2f,  shapeSize2,new Vector2f(400, 50), Player2_Keys, Player1);
        
            ScoreArea1 = new ScoreArea(new Vector2f(50f, 0f), new Vector2f(700f, 25f), Player1);
            ScoreArea2 = new ScoreArea(new Vector2f(50f, 575f), new Vector2f(700f, 25f), Player2);
        
            puck = new Puck(new Vector2f(10f,10f), 0.1f,new Vector2f(-1f,0.5f), new Vector2f(400,300), Color.Red);
        
            VertexContainings = new List<IVertexContaining>();
            Texts = new List<Text>();
            BaseObjects = new List<BaseObject>();
        }

        public void InitPlayerName(Player player,Vector2f pos)
        {
            player.Name.Position = pos;
        }

        private void EndGameText()
        {
            InitPlayerName(Winner, new Vector2f(400, 300));
            gameWindow.Draw(Winner.Name);
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
            gameWindow.KeyPressed += GameWindowKeyPressed;
            gameWindow.KeyPressed += Player1.Player_KeyPressed;
            gameWindow.KeyReleased += Player1.Player_KeyReleased;
            gameWindow.KeyPressed += Player2.Player_KeyPressed;
            gameWindow.KeyReleased += Player2.Player_KeyReleased;
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

        private void OnWin()
        {
            Winner.money += 200;
        }

        private void GetWinner(Collision puckCollision)
        {
            if (puckCollision.collObject?.GetType() == typeof(ScoreArea))
            {
                ScoreArea temp = (ScoreArea)puckCollision.collObject;
                Winner = temp.owner;
                OnWin();
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
            foreach (var text in Texts)
            {
                gameWindow.Draw(text);
            }
            foreach (BaseObject figure in BaseObjects)
            {
                gameWindow.Draw(figure.shape);
            }
            gameWindow.Draw(puck.shape);
        }

        public void Run()
        {
            gameWindow = new RenderWindow(new VideoMode(800, 600), "dingdong");
            Initialization();
            
            while (gameWindow.IsOpen)
            {
                if (gameStopped  == false)
                {
                    Logic();
                    MakeGraphic();
                    gameWindow.DispatchEvents();
                    gameWindow.Display();
                    gameWindow.Clear();
                    
                }
                else
                {
                    EndGameText();
                    gameWindow.DispatchEvents();
                    gameWindow.Display();
                    gameWindow.Clear();
                    Time elapsed = clock.ElapsedTime;
                    if(elapsed >= Time.FromSeconds(10))
                    {
                        Initialization();
                    }
                    gameWindow.Close();
                }
            }
        }
    }
}