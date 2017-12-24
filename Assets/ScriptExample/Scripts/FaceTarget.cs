using UnityEngine;

public class FaceTarget : MonoBehaviour {
	[SerializeField] private Transform m_Target;
	private void LateUpdate() { transform.rotation = Quaternion.LookRotation(transform.position - m_Target.position); }
}