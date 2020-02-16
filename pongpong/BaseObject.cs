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
    public class BaseObject : IVertexContaining
    {
        protected float speed = 0;
        protected Vector2f startPos;
        protected Vector2f dirVector = new Vector2f(0f, 0f);
        protected Vector2f velocity;
        public Vector2f[] vertexes = new Vector2f[4];
        protected Vector2f shapeSize;
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
}