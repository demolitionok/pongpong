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
        public Game game;
        List<GameButton> GameButt;
        public Lobby(Game game)
        {   
            this.game = game;
        }
        
        public void InitGame()
        {
            game.Run();
        }

        public void DrawButton(GameButton gb)
        {
            lobbyWindow.Draw(gb.shape);
            lobbyWindow.Draw(gb.Name);
        }

        public void InitUI()
        {
            GameButt = new List<GameButton>();
            
            //GameButt.Add(new GameButton("butt)", new Vector2f(100f,100f), new Vector2f(300f, 200f), game.Winner.UpgradeSelf));
            GameButt.Add(new GameButton("PlayGame", new Vector2f(100f,100f), new Vector2f(300f, 200f), InitGame));
            
        }
        private void GameWindowKeyPressed(object sender, KeyEventArgs e)
        {
            var window = (Window) sender;
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
        }
        private void InitEvents()
        {
            lobbyWindow.KeyPressed += GameWindowKeyPressed;
        }
        public void Initialization()
        {
            InitUI();
            InitEvents();
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
            Initialization();
            while (lobbyWindow.IsOpen)
            {
                Logic();
                MakeGraphic();
                lobbyWindow.DispatchEvents();
                lobbyWindow.Display();
                lobbyWindow.Clear();
            }
        }
    }
}