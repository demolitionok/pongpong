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
}