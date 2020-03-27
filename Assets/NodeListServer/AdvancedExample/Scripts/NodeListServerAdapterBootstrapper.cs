using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace NodeListServer.AdvancedExample {
    public class NodeListServerAdapterBootstrapper : NetworkBehaviour
    {
        private NodeListServerAdapter nlsAdapter;

        private void Awake()
        {
            nlsAdapter = GetComponent<NodeListServerAdapter>();
            if(!nlsAdapter || NetworkManager.singleton == null)
            {
                enabled = false;
                return;
            }
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            UpdateInfo();
            nlsAdapter.RegisterServer();
        }

        private void OnDisable()
        {
            nlsAdapter.DeregisterServer();
        }

        private void UpdateInfo()
        {
            nlsAdapter.CurrentServerInfo.PlayerCount = NetworkServer.connections.Count;
            nlsAdapter.CurrentServerInfo.PlayerCapacity = NetworkManager.singleton.maxConnections;
        }
    }

}
