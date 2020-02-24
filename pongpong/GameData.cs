namespace pongpong
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Eventing.Reader;
    using System.Net.WebSockets;
    using SFML.Window;
    using SFML.Graphics;
    using SFML.System;
    public struct GameData
    {
        public Clock clock;
        public bool gameStopped;
        
        public RenderWindow gameWindow;
        public List<Keyboard.Key> Player1_Keys;
        public List<Keyboard.Key> Player2_Keys;

        public Player Winner;
        public Player Player1;
        public Player Player2;
        public PlayerData PlayerData1;
        public PlayerData PlayerData2;
        
        public ScoreArea ScoreArea1;
        public ScoreArea ScoreArea2;
        
        public Puck puck;
        
        public List<IVertexContaining> VertexContainings;
        public List<BaseObject> BaseObjects;
        public List<Text> Texts;
        public Dictionary<BaseObject, Vector2f> startPositions;
    }
}