using SFML.System;

namespace pongpong
{
    public struct Collision
    {
        public Vector2f collVertex1;
        public Vector2f collVertex2;
        public BaseObject collObject;

        public Collision(Vector2f collVertex1, Vector2f collVertex2, BaseObject collObject)
        {
            this.collVertex1 = collVertex1;
            this.collVertex2 = collVertex2;
            this.collObject = collObject;
        }
    }
}