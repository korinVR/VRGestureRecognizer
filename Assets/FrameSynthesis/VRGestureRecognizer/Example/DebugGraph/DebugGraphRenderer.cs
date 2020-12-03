using UnityEngine;

namespace FrameSynthesis.VR
{
    public class DebugGraphRenderer : MonoBehaviour
    {
        [SerializeField] VRGestureRecognizer vrGestureRecognizer;
        [SerializeField] Material material;

        void OnPostRender()
        {
            material.SetPass(0);

            GL.LoadOrtho();
            GL.Begin(GL.LINES);

            GL.Color(new Color(1f, 1f, 1f, 0.5f));
            GL.Vertex3(0f, 0.5f, 0f);
            GL.Vertex3(1f, 0.5f, 0f);
            GL.Vertex3(0.5f, 0f, 0f);
            GL.Vertex3(0.5f, 1f, 0f);

            var poseSamples = vrGestureRecognizer.PoseSamples;

            GL.Color(Color.red);

            var prevGraphPosition = Vector2.zero;
            var i = 0;
            foreach (var poseSample in poseSamples)
            {
                var graphPosition = CalculateGraphPositionFromPitch(poseSample);
                if (i > 0)
                {
                    GL.Vertex3(prevGraphPosition.x, prevGraphPosition.y, 0f);
                    GL.Vertex3(graphPosition.x, graphPosition.y, 0f);
                }
                prevGraphPosition = graphPosition;
                i++;
            }

            GL.Color(Color.green);

            i = 0;
            foreach (var poseSample in poseSamples)
            {
                var graphPosition = CalculateGraphPositionFromYaw(poseSample);
                if (i > 0)
                {
                    GL.Vertex3(prevGraphPosition.x, prevGraphPosition.y, 0f);
                    GL.Vertex3(graphPosition.x, graphPosition.y, 0f);
                }
                prevGraphPosition = graphPosition;
                i++;
            }

            GL.End();
        }

        static Vector2 CalculateGraphPositionFromPitch(PoseSample poseSample)
        {
            var x = Time.time - poseSample.Timestamp;
            var y = DegreeTo01(poseSample.Orientation.eulerAngles.x);
            return new Vector2(x, y);
        }

        static Vector2 CalculateGraphPositionFromYaw(PoseSample poseSample)
        {
            var x = DegreeTo01(poseSample.Orientation.eulerAngles.y);
            var y = Time.time - poseSample.Timestamp;
            return new Vector2(x, y);
        }

        static float DegreeTo01(float degree) =>
            MathHelper.LinearMap(MathHelper.WrapDegree(degree), -180f, 180f, 0f, 1f);
    }
}
