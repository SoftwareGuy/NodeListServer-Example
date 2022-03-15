using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeListServer
{
#if UNITY_EDITOR
    /// <summary>
    /// This is for debug purposes only.
    /// NOT RECOMMENDED FOR USE IN A PRODUCTION ENVIRONMENT.
    /// YOU HAVE BEEN WARNED.
    /// </summary>
    public class DebugCommunicator : MonoBehaviour
    {
        private NLSCommunicator communicator = new NLSCommunicator();

        private string ourServerIdentifier = string.Empty;

        private void Start()
        {
            communicator.EndPoint = "http://192.168.88.250:8889";
            communicator.OnServerRegistered += OnServerRegistered;
            communicator.OnServerRemoved += OnServerRemoved;
        }

        // Update is called once per frame
        private void Update()
        {
            // Query.
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(communicator.RetrieveList("NodeListServerDefaultKey"));
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                // Register a server.
                if (!string.IsNullOrEmpty(ourServerIdentifier))
                    return;

                ServerInfo serverInfo = new ServerInfo()
                {
                    Name = $"NodeLS Dummy Server {Random.Range(100, 1000)}",
                    Count = Random.Range(0, 69),
                    Capacity = 999,
                    Port = 31337,
                    ExtraInformation = string.Empty,
                };

                StartCoroutine(communicator.RegisterServer("NodeListServerDefaultKey", serverInfo));
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                // Deregister a server.
                if (string.IsNullOrEmpty(ourServerIdentifier))
                    return;

                StartCoroutine(communicator.DeregisterServer("NodeListServerDefaultKey", ourServerIdentifier));
            }
        }

        private void OnServerRegistered(string serverUuid)
        {
            print("Server registered successfully");
            ourServerIdentifier = serverUuid;
        }

        private void OnServerRemoved()
        {
            print("Server removed successfully");
            ourServerIdentifier = string.Empty;
        }
    }
#endif
}
