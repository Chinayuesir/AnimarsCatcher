using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        private TextMeshProUGUI mScore;

        public static int PlayerScore = 0;

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

            mScore = transform.Find("PlayerScore/Count").GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            mScore.text = PlayerScore.ToString();
        }
    }
}

