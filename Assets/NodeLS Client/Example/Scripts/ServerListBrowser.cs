// This file is part of the NodeListServer Example package.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NodeListServer
{
    public class ServerListBrowser : MonoBehaviour
    {
        [Header("API Configuration")]
        [Tooltip("The URL to connect to the NodeListServer. For example, http://127.0.0.1:8889. Omit the last slash.")]
        [SerializeField] private string serverEndpoint = "http://127.0.0.1:8889";
        [Tooltip("The key required to talk to the server. It must match. Default is NodeListServerDefaultKey.")]
        [SerializeField] private string communicationKey = "NodeListServerDefaultKey";

        [Header("Refresh Settings")]
        [Tooltip("Refresh the server list on startup?")]
        [SerializeField] private bool refreshOnStart = true;
        [Tooltip("Should we automatically refresh the server list? Set to 0 to disable. Default is 10 seconds.")]
        [SerializeField] private int refreshInterval = 10;

        [Header("Cosmetics")]
        public GameObject popup;
        public Text mainStatusText;
        public Text popupStatusText;
        public GameObject ListElementPrefab;
        public GameObject ListElementContainer;

        public Button refreshButton;
        public Button supportButton;

        // Stop editing from this point onwards //
        private bool isBusy => nlsCommunicator.Busy;

        private NLSCommunicator nlsCommunicator = new NLSCommunicator();
        private List<ServerListEntry> listServerListEntries = new List<ServerListEntry>();

        private void Awake()
        {
            // Sanity Checks
            if (string.IsNullOrEmpty(communicationKey))
            {
                Debug.LogError("The communication Key cannot be null or empty!");
                enabled = false;
                return;
            }

            nlsCommunicator.EndPoint = serverEndpoint;
            nlsCommunicator.OnServerListRetrieved += OnServerListRetrieved;

            if (mainStatusText != null)
            {
                mainStatusText.text = "Initializing";
            }

            if (refreshButton)
                refreshButton.onClick.AddListener(RefreshList);

            // For experts only
            if (supportButton)
                supportButton.onClick.AddListener(SupportButton);
        }

        private void Start()
        {
            if (refreshOnStart)
            {
                RefreshList();
            }

            if (refreshInterval > 0)
            {
                InvokeRepeating(nameof(RefreshList), Time.realtimeSinceStartup + refreshInterval, refreshInterval);
            }
        }

        private void LateUpdate()
        {
            if (popup != null)
            {
                popup.SetActive(isBusy);
            }
        }

        // Apparently invoke repeating doesn't work for IEnumerators
        // So I guess the workaround is to make a bootstrapper.
        private void RefreshList()
        {
            // Don't refresh again if we're busy.
            if (isBusy) return;

            StartCoroutine(nlsCommunicator.RetrieveList(communicationKey));
        }

        // -- UI Elements -- //
        // instantiate/remove enough prefabs to match amount (thanks vis2k)
        public void BalancePrefabs(int amount, Transform parent)
        {
            // instantiate until amount
            for (int i = parent.childCount; i < amount; ++i)
            {
                if (ListElementPrefab != null) Instantiate(ListElementPrefab, parent, false);
            }

            // delete everything that's too much
            // (backwards loop because Destroy changes childCount)
            for (int i = parent.childCount - 1; i >= amount; --i)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
        }

        private void UpdateListElements()
        {
            for (int i = 0; i < listServerListEntries.Count; i++)
            {
                if (i >= ListElementContainer.transform.childCount || ListElementContainer.transform.GetChild(i) == null)
                {
                    print("That's null");
                    continue;
                }

                ListEntryController entryController = ListElementContainer.transform.GetChild(i).GetComponent<ListEntryController>();

                string modifiedAddress = string.Empty;
                if (listServerListEntries[i].ip.StartsWith("::ffff:"))
                {
                    modifiedAddress = listServerListEntries[i].ip.Replace("::ffff:", string.Empty);
                }
                else
                {
                    modifiedAddress = listServerListEntries[i].ip;
                }

                entryController.titleText.text = listServerListEntries[i].name;
                entryController.addressText.text = modifiedAddress;
                entryController.playersText.text = $"{listServerListEntries[i].players} {(listServerListEntries[i].capacity > 0 ? $"/ {listServerListEntries[i].capacity}" : string.Empty)}";
                // It is up to you to figure out how to do the latency text.
                entryController.latencyText.text = "-";
            }

            // Done here.
        }

        private void SupportButton()
        {
            Application.OpenURL("http://github.com/SoftwareGuy/NodeListServer-Example");
        }

        #region Callbacks for events from NLS Communicator
        private void OnServerListRetrieved(ServerListResponse response)
        {
            listServerListEntries = response.servers;
            
            // For debug purposes only.
            // print(listServerListEntries.Count);

            if (ListElementContainer != null)
            {
                BalancePrefabs(listServerListEntries.Count, ListElementContainer.transform);
                UpdateListElements();
            }
        }
        #endregion
    }
}
