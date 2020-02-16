using SFML.System;

namespace pongpong
{
    public struct Collision
    {
        public Vector2f collVector;
        public BaseObject collObject;

        public Collision(Vector2f collVector, BaseObject collObject)
        {
            this.collVector = collVector;
            this.collObject = collObject;
        }
    }
}