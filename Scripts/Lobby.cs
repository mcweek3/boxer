using UnityEngine;
using System.Collections;

/**
 * 로비 화면
 */
public class Lobby : MonoBehaviour
{
	// 맨 처음 실행시, 또 방에서 나올 시.
	void Awake() {
		Debug.Log ("Lobby, Awake:I", this);

		// 커넥션 만듬
		if (!PhotonNetwork.connected)
			PhotonNetwork.ConnectUsingSettings("v1.0");
		// 사용자 설정에서 playerName 가져옴. Boxer는 없을 시 default value
		PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "Boxer" + Random.Range(1, 9999));
	}
	
	private string roomName = "My Arena"; // 방 이름
	private Vector2 scrollPos = Vector2.zero; // 방 리스트 스크롤 위치
	
	// 계속해서 실행
	void OnGUI() {
		// 연결 기다리는 중이면 연결 중 화면
		if (!PhotonNetwork.connected) {
			showConnectingDisplay();
			return;
		}
		// 방에 입장해 있으면 로비 띄우지 않기
		if (PhotonNetwork.room != null)
			return;

		/* 레이아웃 */
		// 화면 정중앙에서 가장 왼쪽 어디, 화면 정중앙에서 가장 위 어디, 너비, 길이. 제목
		GUILayout.BeginArea(new Rect((Screen.width - 400)/2, (Screen.height - 300)/2, 400, 300));
		GUILayout.Label("Lobby");
		
		// 플레이어 이름
		GUILayout.BeginHorizontal();
		GUILayout.Label("Boxer Name:", GUILayout.Width(150));
		PhotonNetwork.playerName = GUILayout.TextField(PhotonNetwork.playerName);
		// 해당 창에 타이핑 하면 사용자 설정에 바로 저장
		if (GUI.changed)
			PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
		GUILayout.EndHorizontal();
		
		GUILayout.Space(15);
		
		// 방 입장
		GUILayout.BeginHorizontal();
		GUILayout.Label("Join Arena:", GUILayout.Width(150));
		roomName = GUILayout.TextField(roomName);
		if (GUILayout.Button("GO"))
			PhotonNetwork.JoinRoom(roomName); // 방 입장. *** Photon이 해당 방으로 네트워킹 해줌 ***
		GUILayout.EndHorizontal();
		
		// 방 생성
		GUILayout.BeginHorizontal();
		GUILayout.Label("Create Arena:", GUILayout.Width(150));
		roomName = GUILayout.TextField(roomName);
		if (GUILayout.Button("GO"))
			PhotonNetwork.CreateRoom(roomName, new RoomOptions() {maxPlayers = 8}, TypedLobby.Default);
		GUILayout.EndHorizontal();
		
		// 랜덤 방 입장
		GUILayout.BeginHorizontal();
		GUILayout.Label("Join Random Room:", GUILayout.Width(150));
		if (PhotonNetwork.GetRoomList().Length == 0)
			GUILayout.Label("..no games available...");
		else
			if (GUILayout.Button("GO"))
				PhotonNetwork.JoinRandomRoom();
		GUILayout.EndHorizontal();
		
		GUILayout.Space(30);
		
		// 방 선택 입장
		GUILayout.Label("Arena Listing:");
		if (PhotonNetwork.GetRoomList().Length == 0)
			GUILayout.Label("..no games available..");
		else {
			scrollPos = GUILayout.BeginScrollView(scrollPos);
			foreach (RoomInfo game in PhotonNetwork.GetRoomList()) {
				GUILayout.BeginHorizontal();
				GUILayout.Label(game.name + " " + game.playerCount + "/" + game.maxPlayers);
				if (GUILayout.Button("JOIN"))
					PhotonNetwork.JoinRoom(game.name);
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
		}
		
		GUILayout.EndArea();
	}
	
	
	void showConnectingDisplay() {
		GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
		
		GUILayout.Label("Connecting to Photon server.");
		GUILayout.Label("Hint: This demo uses a settings file and logs the server address to the console.");
		
		GUILayout.EndArea();
	}
}