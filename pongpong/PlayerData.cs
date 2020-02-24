using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Net.WebSockets;
using System.Xml.Serialization;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace pongpong
{
    [Serializable]
    public struct PlayerData
    {
        public Vector2f shapeSize;
        public float money;
    }
}