using System;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace pongpong
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var window = new RenderWindow(new VideoMode(800, 600), "Test");
            var shape = new RectangleShape(new Vector2f(50f, 50f))
            {
                FillColor = Color.Red
            };
            window.Draw(shape);
            window.Display();
        }
    }
}