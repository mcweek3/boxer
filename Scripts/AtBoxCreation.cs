using UnityEngine;
using System.Collections;

public class AtBoxCreation : Photon.MonoBehaviour
{
    private bool appliedInitialUpdate = false;
	private FollowingCamera cameraComponent;
	private PlayerController controllerComponent;

    void Awake()
    {
		Debug.Log ("AtBoxCreation, Awake:I");
		cameraComponent = GetComponent<FollowingCamera> ();
		controllerComponent = GetComponent<PlayerController> ();
    }

    void Start()
    {
		Debug.Log ("AtBoxCreation, Start:I");

        if (photonView.isMine) {
			/*
            Camera.main.transform.parent = transform;
            Camera.main.transform.localPosition = new Vector3(0, 5, -10);
            Camera.main.transform.localEulerAngles = new Vector3(10, 0, 0);
            */
			cameraComponent.enabled = true;
			controllerComponent.enabled = true;
		}
		else {
			cameraComponent.enabled = false;
			controllerComponent.enabled = false;
		}

        controllerComponent.SetIsRemotePlayer(!photonView.isMine);
		// ID 붙여줌.
        gameObject.name = gameObject.name + photonView.viewID;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting) {
			// 우리 플레이어. 다른 사람들한테 정보를 뿌림
            // stream.SendNext((int)controllerScript._characterState);
//            stream.SendNext(transform.position);
//            stream.SendNext(transform.rotation);
//            stream.SendNext(rigidbody.velocity); 
        }
        else {
			// 네트워크 플레이어. 정보를 받음
            // controllerScript._characterState = (CharacterState)(int)stream.ReceiveNext();
//            destPos = (Vector3)stream.ReceiveNext();
//            destRot = (Quaternion)stream.ReceiveNext();
//            rigidbody.velocity = (Vector3)stream.ReceiveNext();

            if (!appliedInitialUpdate) {
                appliedInitialUpdate = true;
                transform.position = destPos;
                transform.rotation = destRot;
                rigidbody.velocity = Vector3.zero;
            }
        }
    }

    private Vector3 destPos = Vector3.zero; // 도착해야 하는 위치. 네트워크로 계속 받음.
    private Quaternion destRot = Quaternion.identity; // 도착해야 하는 방향. 네트워크 계속 받음.

    void Update() {
		// 네트워크 플레이어 들을 해당 위치로 화면에서 움직여줌. Lerp는 이동을 부드럽게.
		if (!photonView.isMine) {
//            transform.position = Vector3.Lerp(transform.position, destPos, Time.deltaTime * 5);
//            transform.rotation = Quaternion.Lerp(transform.rotation, destRot, Time.deltaTime * 5);
        }
    }

/*
    void OnPhotonInstantiate(PhotonMessageInfo info) {
		Debug.Log ("AtBoxCreation, OnPhotonInstantiate:I");
    }
*/
}