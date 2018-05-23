using System;

namespace MrRondon.Services.Api.Extensions
{
    public static class DoubleExtension
    {
        public static double ToRadian(this double value)
        {
            const double radiansOverDegrees = Math.PI / 180.0;
            var result = value * radiansOverDegrees;

            return result;
        }
    }
}