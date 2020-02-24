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
    public class Lobby
    {
        public RenderWindow lobbyWindow = new RenderWindow(new VideoMode(800, 600), "Lobby");
        public GameData gameData = new GameData();
        public Game game;
        public bool initOnce = true;
        List<GameButton> GameButt;
        public void Upgrade()
        {
            gameData.Winner.playerData.shapeSize += new Vector2f(10f, 0f);
        }

        public void Degrade()//downgrade
        {
            gameData.Winner.foe.playerData.shapeSize -= new Vector2f(10f, 0f);
            
        }
        public Lobby()
        {   
        }
        
        public void InitVariables()
        {
            gameData.clock = new Clock();
            gameData.gameStopped = false;
            gameData.Player1 = new Player("Player1", 0.2f, gameData.PlayerData1,new Vector2f(400, 550), gameData.Player1_Keys, gameData.Player2); 
            gameData.Player2 = new Player("Player2", 0.2f, gameData.PlayerData2,new Vector2f(400, 50), gameData.Player2_Keys, gameData.Player1);
            gameData.Player1.foe = gameData.Player2;
        
            gameData.ScoreArea1 = new ScoreArea(new Vector2f(50f, 0f), new Vector2f(700f, 25f), gameData.Player1);
            gameData.ScoreArea2 = new ScoreArea(new Vector2f(50f, 575f), new Vector2f(700f, 25f), gameData.Player2);
        
            gameData.puck = new Puck(new Vector2f(10f,10f), 0.1f,new Vector2f(-1f,0.5f), new Vector2f(400,300), Color.Red);
        
            gameData.VertexContainings = new List<IVertexContaining>();
            gameData.Texts = new List<Text>();
            gameData.BaseObjects = new List<BaseObject>();
            gameData.startPositions = new Dictionary<BaseObject, Vector2f>();
        }

        public void InitStartVariables()
        {
            foreach (var baseObject in gameData.BaseObjects)
            {
                baseObject.shape.Position = baseObject.startPos;
            }
            gameData.puck.shape.Position = gameData.puck.startPos;
            gameData.puck.dirVector = gameData.puck.startDirVector;
        }

        private void InitGameEvents()
        {
            gameData.gameWindow.KeyPressed += gameData.Player1.Player_KeyPressed;
            gameData.gameWindow.KeyReleased += gameData.Player1.Player_KeyReleased;
            gameData.gameWindow.KeyPressed += gameData.Player2.Player_KeyPressed;
            gameData.gameWindow.KeyReleased += gameData.Player2.Player_KeyReleased;
        }

        private void InitGameKeys()
        {
            gameData.Player1_Keys = new List<Keyboard.Key>();
            gameData.Player2_Keys = new List<Keyboard.Key>();
            
            gameData.Player1_Keys.Add(Keyboard.Key.F);
            gameData.Player1_Keys.Add(Keyboard.Key.D);
            gameData.Player2_Keys.Add(Keyboard.Key.Right);
            gameData.Player2_Keys.Add(Keyboard.Key.Left);
        }
        private void InitObstacles()
        {
            gameData.BaseObjects.Add(gameData.Player1);
            gameData.BaseObjects.Add(gameData.Player2);
            gameData.BaseObjects.Add(gameData.ScoreArea1);
            gameData.BaseObjects.Add(gameData.ScoreArea2);
            gameData.BaseObjects.Add(new Obstacle(new Vector2f(0f, 0f), new Vector2f(50f, 600f)));
            gameData.BaseObjects.Add(new Obstacle(new Vector2f(200f, 275f), new Vector2f(100f, 50f)));
            gameData.BaseObjects.Add(new Obstacle(new Vector2f(500f, 275f), new Vector2f(100f, 50f)));
            gameData.BaseObjects.Add(new Obstacle(new Vector2f(750f, 0f), new Vector2f(50f, 600f)));
        }
        
        private void GameInitialization()
        {
            InitGameKeys();
            InitVariables();
            InitObstacles();
        }
        
        public void RunGame()
        {
            gameData.gameWindow = new RenderWindow(new VideoMode(800, 600), "dingdong");
            InitGameEvents();
            InitStartVariables();
            game.gameData = gameData;
            game.Run();
            gameData = game.gameData;
            gameData.gameStopped = false;
        }

        public void DrawButton(GameButton gb)
        {
            lobbyWindow.Draw(gb.shape);
            lobbyWindow.Draw(gb.Name);
        }

        public void InitPlayerDatas()
        {
            gameData.PlayerData1 = new PlayerData();
            gameData.PlayerData2 = new PlayerData();
            
            gameData.PlayerData1.shapeSize = new Vector2f(50f, 20f);
            gameData.PlayerData2.shapeSize = new Vector2f(50f, 20f);

            gameData.PlayerData1.money = 0f;
            gameData.PlayerData2.money = 0f;
        }

        public void InitUI()
        {
            GameButt = new List<GameButton>();
            
            //GameButt.Add(new GameButton("butt)", new Vector2f(100f,100f), new Vector2f(300f, 200f), game.Winner.UpgradeSelf));
            GameButt.Add(new GameButton("PlayGame", new Vector2f(100f,100f), new Vector2f(300f, 200f), RunGame));
            
            if(gameData.Winner != null)
            {
                GameButt.Add(new GameButton("Upgrade self", 200, gameData.Winner, new Vector2f(100f,100f), new Vector2f(100f, 450f), Upgrade));
                GameButt.Add(new GameButton("Degrade opponent", 200, gameData.Winner, new Vector2f(100f,100f), new Vector2f(300f, 450f), Degrade));
            }
            
        }
        private void GameWindowKeyPressed(object sender, KeyEventArgs e)
        {
            var window = (Window) sender;
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
        }
        private void InitLobbyEvents()
        {
            lobbyWindow.KeyPressed += GameWindowKeyPressed;
        }
        public void LobbyInitialization()
        {
            InitPlayerDatas();
            GameInitialization();
            InitUI();
            InitLobbyEvents();
            game = new Game(gameData);
        }

        public void Logic()
        {
            foreach (GameButton gameButton in GameButt)
            {
                gameButton.DetectClick(lobbyWindow);   
            }
        }

        public void MakeGraphic()
        {
            foreach (var gameButton in GameButt)
            {
                DrawButton(gameButton);
            }
        }

        public void InitLobby()
        {
            LobbyInitialization();
            while (lobbyWindow.IsOpen)
            {
                if (gameData.gameWindow == null || gameData.gameWindow.IsOpen == false)
                {
                    if (gameData.Winner != null && initOnce == true)
                    {
                        InitUI();
                        initOnce = false;
                    }

                    Logic();
                    MakeGraphic();
                    lobbyWindow.DispatchEvents();
                    lobbyWindow.Display();
                    lobbyWindow.Clear();
                }

                else if (gameData.gameWindow.IsOpen)
                {
                    initOnce = true;
                }
            }
        }
    }
}