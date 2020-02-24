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
        public GameData gameData;

        public Game(GameData gameData)
        {
            this.gameData = gameData;
        }

        public void FartherstVertex()
        {
        }


        public void InitPlayerName(Player player,Vector2f pos)
        {
            player.Name.Position = pos;
        }

        private void EndGameText()
        {
            InitPlayerName(gameData.Winner, new Vector2f(400, 300));
            gameData.gameWindow.Draw(gameData.Winner.Name);
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
            gameData.Winner.playerData.money += 200;
        }

        private void GetWinner(Collision puckCollision)
        {
            if (puckCollision.collObject?.GetType() == typeof(ScoreArea))
            {
                ScoreArea temp = (ScoreArea) puckCollision.collObject;
                gameData.Winner = temp.owner;
                OnWin();
                
                gameData.gameStopped = true;
            }
        }

        private void Logic()
        {
            Collision puckCollision = DetectCollision(gameData.puck, gameData.BaseObjects);
            gameData.puck.MoveObject(puckCollision);
            gameData.Player1.MoveObject();
            gameData.Player2.MoveObject();
            GetWinner(puckCollision);
        }

        private void MakeGraphic()
        {
            foreach (var text in gameData.Texts)
            {
                gameData.gameWindow.Draw(text);
            }
            foreach (BaseObject figure in gameData.BaseObjects)
            {
                gameData.gameWindow.Draw(figure.shape);
            }
            gameData.gameWindow.Draw(gameData.puck.shape);
        }

        public void Run()
        {
            while (gameData.gameWindow.IsOpen)
            {
                if (gameData.gameStopped  == false)
                {
                    Logic();
                    MakeGraphic();
                    gameData.gameWindow.DispatchEvents();
                    gameData.gameWindow.Display();
                    gameData.gameWindow.Clear();
                    
                }
                else
                {
                    EndGameText();
                    gameData.gameWindow.DispatchEvents();
                    gameData.gameWindow.Display();
                    gameData.gameWindow.Clear();
                    Time elapsed = gameData.clock.ElapsedTime;
                    if(elapsed >= Time.FromSeconds(10))
                    {
                        gameData.gameWindow.Close();
                    }
                }
            }
        }
    }
}