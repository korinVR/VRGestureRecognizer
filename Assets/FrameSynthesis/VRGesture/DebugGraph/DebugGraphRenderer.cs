using UnityEngine;

namespace FrameSynthesis.VR
{
    public class DebugGraphRenderer : MonoBehaviour
    {
        [SerializeField]
        Material material;
        [SerializeField]
        VRGesture vrGesture;

        float project(float angle)
        {
            const float scale = 180f;
            return MyMath.LinearMap(MyMath.WrapAngle(angle), -scale, scale, 0f, 1f);
        }

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

            float[] timestamps;
            Quaternion[] orientations;
            vrGesture.GetGraphEntries(out timestamps, out orientations);

            GL.Color(Color.red);
            for (int i = 0; i < timestamps.Length - 1; i++)
            {
                GL.Vertex3(Time.time - timestamps[i], project(orientations[i].eulerAngles.x), 0f);
                GL.Vertex3(Time.time - timestamps[i + 1], project(orientations[i + 1].eulerAngles.x), 0f);
            }

            GL.Color(Color.green);
            for (int i = 0; i < timestamps.Length - 1; i++)
            {
                GL.Vertex3(project(orientations[i].eulerAngles.y), Time.time - timestamps[i], 0f);
                GL.Vertex3(project(orientations[i + 1].eulerAngles.y), Time.time - timestamps[i + 1], 0f);
            }

            GL.End();
        }
    }
}
