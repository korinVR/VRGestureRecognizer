using UnityEngine;

namespace FrameSynthesis.VR
{
    public class DebugGraphRenderer : MonoBehaviour
    {
        [SerializeField]
        Material material;

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

            var poseSamples = VRGestureRecognizer.Current.PoseSamples;

            GL.Color(Color.red);

            var prevGraphPosition = Vector2.zero;
            var i = 0;
            foreach (var poseSample in poseSamples)
            {
                var graphPosition = GetGraphPositionFromPoseSamplePitch(poseSample);
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
                var graphPosition = GetGraphPositionFromPoseSampleYaw(poseSample);
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

        Vector2 GetGraphPositionFromPoseSamplePitch(PoseSample poseSample)
        {
            float x = Time.time - poseSample.timestamp;
            float y = ProjectDegreeTo01(poseSample.orientation.eulerAngles.x);
            return new Vector2(x, y);
        }

        Vector2 GetGraphPositionFromPoseSampleYaw(PoseSample poseSample)
        {
            float x = ProjectDegreeTo01(poseSample.orientation.eulerAngles.y);
            float y = Time.time - poseSample.timestamp;
            return new Vector2(x, y);
        }

        float ProjectDegreeTo01(float degree)
        {
            return MyMath.LinearMap(MyMath.WrapDegree(degree), -180f, 180f, 0f, 1f);
        }
    }
}
