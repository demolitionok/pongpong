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

    public delegate void OnClick();
    public class GameButton
    {
        public OnClick onClick;
        public Player owner;
        public int cost;
        public string text = "button";
        public Text Name = new Text();
        protected Vector2f startPos;
        public Vector2f shapeSize;
        public Vector2f[] vertexes = new Vector2f[4];
        public RectangleShape shape;
        
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
        public void InitName()
        {
            Name.DisplayedString = text;
            Name.Font = new Font("BiolinumV1.ttf");
            Name.CharacterSize = 22;
            Name.FillColor = Color.Red;
            Name.Position = shape.Position;
        }

        public void DetectClick(RenderWindow rw)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left) == true)
            {
                Vector2i mousePos =  Mouse.GetPosition(rw);
                if (
                    (
                        (mousePos.X >= vertexes[0].X || mousePos.X >= vertexes[3].X)
                        &&
                        (mousePos.X <= vertexes[1].X || mousePos.X <= vertexes[2].X)
                    )
                    &&
                    (
                        (mousePos.Y >= vertexes[0].Y || mousePos.Y >= vertexes[3].Y)
                        &&
                        (mousePos.Y <= vertexes[1].Y || mousePos.Y <= vertexes[2].Y)
                    )
                )
                {
                    Console.WriteLine("Clicked");
                    if(owner == null)
                        onClick();
                    else if (owner.playerData.money >= cost)
                    {
                        onClick();
                        owner.playerData.money -= cost;
                    }
                }
            }
            Console.WriteLine("Not Clicked");
        }

        public GameButton(string text, Vector2f shapeSize, Vector2f startPos, Action action)
        {
            onClick = new OnClick(action);
            this.text = text;
            this.shapeSize = shapeSize;
            shape = new RectangleShape(shapeSize);
            shape.Position = startPos;
            
            InitVertexes();
            InitName();
        }
        public GameButton(string text, int cost, Player owner, Vector2f shapeSize, Vector2f startPos, Action action)
        {
            onClick = new OnClick(action);
            this.cost = cost;
            this.owner = owner;
            this.text = text;
            this.shapeSize = shapeSize;
            shape = new RectangleShape(shapeSize);
            shape.Position = startPos;
            
            InitVertexes();
            InitName();
        }
    }
}