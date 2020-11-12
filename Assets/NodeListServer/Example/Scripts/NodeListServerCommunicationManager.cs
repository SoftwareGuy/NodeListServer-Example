using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeListServer
{
    public class NodeListServerCommunicationManager : MonoBehaviour
    {
        public static NodeListServerCommunicationManager Instance;

        // You can modify this data.
        public ServerInfo CurrentServerInfo = new ServerInfo()
        {
            Name = "Untitled Server",
            Port = 7777,
            PlayerCount = 0,
            PlayerCapacity = 0,
            ExtraInformation = string.Empty
        };

        private const string AuthKey = "NodeListServerDefaultKey";

        // Change this to your NodeLS Server instance URL.
        private const string Server = "http://furutaka.oiran.studio:8889";
        private const string AddEndpoint = "add";
        private const string RemoveEndpoint = "remove";

        // Don't modify, this is randomly generated.
        private string InstanceServerId = string.Empty;

        private void Awake()
        {
            // If singleton was somehow loaded twice...
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Duplicate NodeLS Communication Manager detected in scene. This one will be destroyed.");
                Destroy(this);
                return;
            }

            Instance = this;

            // Generate a new identification string
            Guid randomGuid = Guid.NewGuid();
            InstanceServerId = randomGuid.ToString();

            print("NodeLS Communication Manager Initialized.");
        }


        public void AddUpdateServerEntry()
        {
            StartCoroutine(nameof(AddUpdateInternal));
        }

        public void RemoveServerEntry()
        {
            StartCoroutine(nameof(RemoveServerInternal));
        }

        // Internal things
        private IEnumerator AddUpdateInternal()
        {
            WWWForm serverData = new WWWForm();
            print("NodeLS Communication Manager: Adding/Updating Server Entry");

            serverData.AddField("serverKey", AuthKey);

            serverData.AddField("serverUuid", InstanceServerId);
            serverData.AddField("serverName", CurrentServerInfo.Name);
            serverData.AddField("serverPort", CurrentServerInfo.Port);
            serverData.AddField("serverPlayers", CurrentServerInfo.PlayerCount);
            serverData.AddField("serverCapacity", CurrentServerInfo.PlayerCapacity);
            serverData.AddField("serverExtras", CurrentServerInfo.ExtraInformation);

            using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Post($"{Server}/{AddEndpoint}", serverData))
            {
                yield return www.SendWebRequest();

                if (www.responseCode == 200)
                {
                    print("Successfully registered server with the NodeListServer instance!");
                }
                else
                {
                    Debug.LogError($"Failed to register the server with the NodeListServer instance: {www.error}");                    
                }
            }

            yield break;
        }

        private IEnumerator RemoveServerInternal()
        {
            WWWForm serverData = new WWWForm();
            print("NodeLS Communication Manager: Removing Server Entry");

            // Assign all the fields required.
            serverData.AddField("serverKey", AuthKey);
            serverData.AddField("serverUuid", InstanceServerId);

            using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Post($"{Server}/{RemoveEndpoint}", serverData))
            {
                yield return www.SendWebRequest();

                if (www.responseCode == 200)
                {
                    print("Successfully deregistered server with the NodeListServer instance!");
                }
                else
                {
                    Debug.LogError($"Failed to deregister the server with the NodeListServer instance: {www.error}");
                }
            }

            yield break;
        }
    }

}
