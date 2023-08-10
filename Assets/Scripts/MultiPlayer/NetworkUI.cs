using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


namespace AnimarsCatcher
{
    public class NetworkUI : MonoBehaviour
    {
        public Button StartServerBtn;
        public Button StartHostBtn;
        public Button StartClientBtn;
        public Button MoveBtn;

        private void Awake()
        {
            StartServerBtn.onClick.AddListener(()=>
            {
                NetworkManager.Singleton.StartServer();
            });
            
            StartHostBtn.onClick.AddListener(()=>
            {
                NetworkManager.Singleton.StartHost();
            });
            
            StartClientBtn.onClick.AddListener(()=>
            {
                NetworkManager.Singleton.StartClient();
            });
            
            MoveBtn.onClick.AddListener(() =>
            {
                if (NetworkManager.Singleton.IsClient)
                {
                    var ani = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                    ani.GetComponent<AniController>().Move();
                }
            });
        }
    }
}

