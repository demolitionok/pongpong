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
}