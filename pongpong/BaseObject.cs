using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Net.WebSockets;
using SFML.Window;
using System.Linq;
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
        public float FartherstVertex()
        {
            
            if (vertexes != null)
            {
                return vertexes

                    .Select(x =>
                        (float)Math.Sqrt(Math.Pow((x.X - shape.Position.X), 2) + Math.Pow((x.Y - shape.Position.Y), 2))
                    ).Max();
            }

            return 0;
        }

        public List<BaseObject> Objects_possible_to_collide(List<BaseObject> baseObjects)
        {
            return baseObjects
                .Where(figure =>
                    Math.Sqrt(Math.Pow((figure.shape.Position.X - shape.Position.X), 2) +
                              Math.Pow((figure.shape.Position.Y - shape.Position.Y), 2))
                    < FartherstVertex() + figure.FartherstVertex()).ToList();
        }
        
        public Collision DetectCollision(List<BaseObject> baseObjects)
        {
            bool intersecting = false;
            List<BaseObject> possibleToCollide = Objects_possible_to_collide(baseObjects);
            foreach (var figure in possibleToCollide)
            {
                for (int i = 0; i < vertexes.Length; i++)
                {
                    var my_vertex = vertexes[i];
                    var my_next_vertex = i + 1 < vertexes.Length ? vertexes[i + 1] : vertexes[0];
                    for (int j = 0; j < figure.vertexes.Length; j++)
                    {
                        
                        var vertex = figure.vertexes[j];
                        var next_vertex = j + 1 < figure.vertexes.Length ? figure.vertexes[j+1] : figure.vertexes[0];

                        var v1 = (next_vertex.X - vertex.X) * (my_vertex.Y - vertex.Y) -
                                 (next_vertex.Y - vertex.Y) * (my_vertex.X - vertex.X);
                        var v2 = (next_vertex.X - vertex.X) * (my_next_vertex.Y - vertex.Y) -
                                 (next_vertex.Y - vertex.Y) * (my_next_vertex.X - vertex.X);
                        var v3 = (my_next_vertex.X - my_vertex.X) * (vertex.Y - my_vertex.Y) -
                                 (my_next_vertex.Y - my_vertex.Y) * (vertex.X - my_vertex.X);
                        var v4 = (my_next_vertex.X - my_vertex.X) * (next_vertex.Y - my_vertex.Y) -
                                 (my_next_vertex.Y - my_vertex.Y) * (next_vertex.X - my_vertex.X);

                        intersecting = (v1 * v2 < 0) && (v3 * v4 < 0);
                        if (intersecting)
                        {
                            return new Collision(my_vertex,my_next_vertex, figure);
                        }
                    }
                }   
            }
            return new Collision(new Vector2f(0,0), new Vector2f(0,0), null);
        }
        public virtual void MoveObject()
        {
            velocity = dirVector * speed;
            shape.Position += velocity;
            InitVertexes();
        }
    }
}