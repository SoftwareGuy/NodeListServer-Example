using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace NodeListServer.AdvancedExample {
    // TODO: Remove this class.    
    public class NodeListServerAdapterBootstrapper : NetworkBehaviour
    {
/*        
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

            nlsAdapter.RegisterServer();
        }

        private void LateUpdate()
        {
            UpdateInfo();
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
*/
    }

}
