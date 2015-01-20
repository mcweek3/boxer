using UnityEngine;
using System.Collections;

public class FollowingCamera : MonoBehaviour {
	public Transform cam; // 초기 카메라 위치
	public float smoothing = 5f; // 카메라가 따라가는 속도
	Vector3 offset; // 초기 카메라와 위치 차이
	
	void Start () {
		// 초기 offset 계산
		cam = Camera.main.transform;
		offset = cam.position - transform.position;
	}
	
	void FixedUpdate ()
	{
		// 카메라가 어디로 가야하는지
		Vector3 destCamPos = transform.position + offset;
		// 카메라 이동. Lerp 부드럽게.
		cam.position = Vector3.Lerp (destCamPos, transform.position, smoothing * Time.deltaTime);
	}
}