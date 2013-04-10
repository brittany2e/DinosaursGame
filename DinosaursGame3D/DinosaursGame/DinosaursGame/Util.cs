using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace DinosaursGame
{
    static class Util
    {
        public static Vector2 vec2sum(Vector2 first, Vector2 second)
        {
            return new Vector2( first.X + second.X, 
                                first.Y + second.Y);
        }

        public static Vector3 vec3sum(Vector3 first, Vector3 second)
        {
            return new Vector3( first.X + second.X, 
                                first.Y + second.Y,
                                first.Z + second.Z);
        }

    }
}
