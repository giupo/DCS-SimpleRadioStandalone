﻿namespace Ciribob.DCS.SimpleRadio.Standalone.Common
{
    public struct DcsPosition
    {
        public double x;
        public double y;
        public double z;

        public override string ToString()
        {
            return $"Pos:[{x},{y},{z}]";
        }
    }
}