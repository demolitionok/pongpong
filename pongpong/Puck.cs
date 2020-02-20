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
    public class Puck : BaseObject, IVertexContaining
    {
        public new void InitVertexes()
        {
            vertexes[0] = new Vector2f(shapeSize.X + shape.Position.X, shape.Position.Y);
            vertexes[1] = new Vector2f(2*shapeSize.X + shape.Position.X, shapeSize.Y + shape.Position.Y);
            vertexes[2] = new Vector2f(shapeSize.X + shape.Position.X, 2*shapeSize.Y + shape.Position.Y);
            vertexes[3] = new Vector2f(shape.Position.X, shapeSize.Y + shape.Position.Y);
            /*
             *    ./----[0]----\
             *    |            |
             *  [6]    Puck    [2]
             *    |            |
             *     \---[4]---/
             *
             *     It should look like a circle.
             */
        }

        public new void MoveObject(Collision coll)
        {
            velocity = dirVector * speed;
            if (coll.collObject == null)
            {
                shape.Position += velocity;
            }
            else
            {
                if (coll.collVertex2 == vertexes[0] || coll.collVertex2 == vertexes[2])
                {
                    dirVector = new Vector2f(dirVector.X, -dirVector.Y);
                }
                else if (coll.collVertex2 == vertexes[1] || coll.collVertex2 == vertexes[3])
                {
                    dirVector = new Vector2f(-dirVector.X, dirVector.Y);
                }
                shape.Position -= 10*velocity;
                shape.Position -= 3*velocity;
            }
            InitVertexes();
        }

        public Puck(Vector2f shapeSize, float speed, Vector2f dirVector, Vector2f startPos, Color color)
        {
            this.dirVector = dirVector;
            this.startPos = startPos;
            this.speed = speed;
            this.shapeSize = shapeSize;
            
            shape = new CircleShape(shapeSize.X)
            {
                FillColor = color
            };
            shape.Position = startPos;
            InitVertexes();
        }
    }
}