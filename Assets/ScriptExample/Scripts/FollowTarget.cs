using UnityEngine;

public class FollowTarget : MonoBehaviour {
	[SerializeField] private Transform m_Target;
	[SerializeField, Range(0.001f, 0.1f)] private float m_Speed = 0.01f;
	[SerializeField] private float m_DistanceFromTarget = 1.5f;

	private Vector3 m_FreePosition;

	private Vector3 TargetPosition { get { return m_Target.position + new Vector3(m_Target.forward.x, 0, m_Target.forward.z) * m_DistanceFromTarget; } }

	private void Start() { m_FreePosition = transform.position = TargetPosition; }

	private void LateUpdate() {
		m_FreePosition = Vector3.Lerp(m_FreePosition, TargetPosition, m_Speed);
		Vector3 direction = m_FreePosition - m_Target.position;
		transform.position = m_Target.position + direction.normalized * m_DistanceFromTarget;
	}
}