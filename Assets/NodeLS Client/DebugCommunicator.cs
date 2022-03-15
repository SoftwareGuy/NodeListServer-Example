using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeListServer
{
    public class DebugCommunicator : MonoBehaviour
    {
        private NLSCommunicator communicator = new NLSCommunicator();

        private string ourServerIdentifier = string.Empty;

        private void Start()
        {
            communicator.EndPoint = "http://192.168.88.250:8889";
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
                    Name = "NodeLS Communicator Test Server",
                    Count = 69,
                    Capacity = 420,
                    Port = 31337,
                    ExtraInformation = string.Empty,
                };

                ourServerIdentifier = communicator.RegisterServer("NodeListServerDefaultKey", serverInfo);

            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                // Deregister a server.
                if (string.IsNullOrEmpty(ourServerIdentifier))
                    return;

                communicator.DeregisterServer("NodeListServerDefaultKey", ourServerIdentifier);
            }
        }

        private void OnServerRemoved()
        {
            ourServerIdentifier = string.Empty;
        }
    }

}
