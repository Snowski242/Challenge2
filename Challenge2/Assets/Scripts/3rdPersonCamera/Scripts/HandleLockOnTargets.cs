using ThirdPersonCamera;
using UnityEngine;

public class HandleLockOnTargets : MonoBehaviour
{
	public LockOnTarget LockOnTarget;

	private void OnTriggerEnter(Collider other)
	{
		var tmp = other.gameObject.GetComponent<Targetable>();
		if (tmp != null)
		{
			LockOnTarget.AddTarget(tmp);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		var tmp = other.gameObject.GetComponent<Targetable>();
		if (tmp != null)
		{
			LockOnTarget.RemoveTarget(tmp);
		}
	}
}
