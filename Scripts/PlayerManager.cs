using UnityEngine;
using System.Collections;

public class PlayerManager : Photon.MonoBehaviour {
	public string playerPrefabName = "Charprefab"; // Player 생성 정보(prefab) 이름.

	/* 조인 할 때 네트웨크에 플레이어 생성 */
	void OnJoinedRoom()
	{
		Debug.Log ("PlyaerManager, OnJoinedRoom:I", this);

		PhotonNetwork.Instantiate(this.playerPrefabName,	// 플레이어 이름
		                          transform.position,		// 플레이어 초기 위치
		                          Quaternion.identity,		// 플레이어 초기 방향
		                          0,						// 그룹
		                          null);					// 기타 전달인자
	}

	/* 방을 나갈 때, 하는일. 제대로 꺼지길 기다리고 그 전 레벨로 돌아감 */
	IEnumerator OnLeftRoom()
	{
		Debug.Log ("GameManagerBxr, OnLeftRoom:I", this);

		while(PhotonNetwork.room!=null || PhotonNetwork.connected==false)
			yield return 0;
		
		Application.LoadLevel(Application.loadedLevel);
	}

	/* 방에 있을 때만 방을 나갈수 있게 함 */
	void OnGUI()
	{
		if (PhotonNetwork.room == null) return; //Only display this GUI when inside a room
		
		if (GUILayout.Button("Leave Room"))
		{
			PhotonNetwork.LeaveRoom();
		}
	}
	
	void OnDisconnectedFromPhoton()
	{
		Debug.LogWarning("OnDisconnectedFromPhoton");
	}    
}