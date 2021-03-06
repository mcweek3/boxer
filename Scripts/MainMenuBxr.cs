﻿using UnityEngine;
using System.Collections;

public class MainMenuBxr : MonoBehaviour
{
	
	void Awake()
	{
		Debug.Log ("MainMenuBxr, Awake:I", this);
		
		//Connect to the main photon server. This is the only IP and port we ever need to set(!)
		if (!PhotonNetwork.connected)
			PhotonNetwork.ConnectUsingSettings("v1.0"); // version of the game/demo. used to separate older clients from newer ones (e.g. if incompatible)
		
		//Load name from PlayerPrefs
		PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "Boxer" + Random.Range(1, 9999));
		
		//Set camera clipping for nicer "main menu" background
		//		Camera.main.farClipPlane = Camera.main.nearClipPlane + 0.1f;
	}
	
	private string roomName = "My Arena";
	private Vector2 scrollPos = Vector2.zero;
	
	void OnGUI()
	{
		if (!PhotonNetwork.connected)
		{
			ShowConnectingGUI();
			return;   //Wait for a connection
		}
		if (PhotonNetwork.room != null)
			return; //Only when we're not in a Room
		
		GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
		GUILayout.Label("Main Menu");
		
		//Player name
		GUILayout.BeginHorizontal();
		GUILayout.Label("Choose Your Name:", GUILayout.Width(100));
		PhotonNetwork.playerName = GUILayout.TextField(PhotonNetwork.playerName);
		if (GUI.changed)//Save name
			PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
		GUILayout.EndHorizontal();
		
		GUILayout.Space(15);
		
		//Join room by title
		GUILayout.BeginHorizontal();
		GUILayout.Label("Join the Arena:", GUILayout.Width(100));
		roomName = GUILayout.TextField(roomName);
		if (GUILayout.Button("GO!"))
		{
			PhotonNetwork.JoinRoom(roomName);////////////////////////////////////////////////////////////////////////////////
		}
		GUILayout.EndHorizontal();
		
		//Create a room (fails if exist!)
		GUILayout.BeginHorizontal();
		GUILayout.Label("Create Arena:", GUILayout.Width(100));
		roomName = GUILayout.TextField(roomName);
		if (GUILayout.Button("GO!!"))
		{
			// using null as TypedLobby parameter will also use the default lobby
			PhotonNetwork.CreateRoom(roomName, new RoomOptions() { maxPlayers = 8 }, TypedLobby.Default);////////////////////////////////////////////////////////////////////////////////
		}
		GUILayout.EndHorizontal();
		
		//Join random room
		GUILayout.BeginHorizontal();
		GUILayout.Label("Join Random Arena:", GUILayout.Width(100));
		if (PhotonNetwork.GetRoomList().Length == 0)
		{
			GUILayout.Label("..no games available...");
		}
		else
		{
			if (GUILayout.Button("GO!!!"))
			{
				PhotonNetwork.JoinRandomRoom();////////////////////////////////////////////////////////////////////////////////
			}
		}
		GUILayout.EndHorizontal();
		
		GUILayout.Space(30);
		GUILayout.Label("Arena Listing:");
		if (PhotonNetwork.GetRoomList().Length == 0)
		{
			GUILayout.Label("..no games available..");
		}
		else
		{
			//Room listing: simply call GetRoomList: no need to fetch/poll whatever!
			scrollPos = GUILayout.BeginScrollView(scrollPos);
			foreach (RoomInfo game in PhotonNetwork.GetRoomList())
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(game.name + " " + game.playerCount + "/" + game.maxPlayers);
				if (GUILayout.Button("Join in!!"))
				{
					PhotonNetwork.JoinRoom(game.name);////////////////////////////////////////////////////////////////////////////////
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
		}
		
		GUILayout.EndArea();
	}
	
	
	void ShowConnectingGUI()
	{
		GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
		
		GUILayout.Label("Connecting to Photon server.");
		GUILayout.Label("Hint: This demo uses a settings file and logs the server address to the console.");
		
		GUILayout.EndArea();
	}
}