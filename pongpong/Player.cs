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
        public float money = 0;
        public Player foe;
        public string name;
        public Text Name = new Text();

        public void UpgradeSelf()
        {
            shapeSize += new Vector2f(10f, 0f);
            shape = new RectangleShape(shapeSize);
        }

        public void DegradeFoe()//downgrade
        {
            foe.shapeSize -= new Vector2f(10f, 0f);
            foe.shape = new RectangleShape(shapeSize);
        }

        public void InitName()
        {
            if (name != null)
            {
                Name.DisplayedString = name;
                Name.Font = new Font("BiolinumV1.ttf");
                Name.CharacterSize = 22;
                Name.FillColor = Color.Red;
            }
        }

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

        public Player(string name, float speed, Vector2f shapeSize, Vector2f startPos, List<Keyboard.Key> playerKeys, Player foe)
        {
            this.name = name;
            this.foe = foe;
            this.shapeSize = shapeSize;
            shape = new RectangleShape(shapeSize);
            this.startPos = startPos;
            shape.Position = startPos;

            this.speed = speed;
            this.playerKeys = playerKeys;
            InitVertexes();
            InitName();
        }
    }
}