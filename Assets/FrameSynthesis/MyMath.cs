using UnityEngine;

namespace FrameSynthesis
{
    public class MyMath
    {
        public static float LinearMap(float value, float s0, float s1, float d0, float d1)
        {
            return d0 + (value - s0) * (d1 - d0) / (s1 - s0);
        }

        public static float WrapDegree(float degree)
        {
            if (degree > 180f)
            {
                return degree - 360f;
            }
            return degree;
        }
    }
}