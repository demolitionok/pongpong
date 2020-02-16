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

    public class Player : BaseObject
    {
        private List<Keyboard.Key> playerKeys;
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
}