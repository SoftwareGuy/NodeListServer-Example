using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;

namespace NodeListServer
{
    /// <summary>
    /// This class represents a Node List Server communicator, which acts as a interface
    /// between the Node List Server web application running on a server and your game.
    /// </summary>
    public class NLSCommunicator
    {
        #region Constants
        const string RegisterEndpoint = "/add";
        const string RemoveEndpoint = "/remove";
        const string QueryEndpoint = "/list";
        #endregion

        #region Public properties
        /// <summary>
        /// The URI to the NodeLS Endpoint. For example, "https://127.0.0.1:9000".
        /// </summary>
        public string EndPoint { get; set; }
        #endregion

        public bool Busy { get; private set; }
        #region Private properties
        /// <summary>
        /// Convience property. No need to worry about this.
        /// </summary>
        private Uri EndPointAsUri { get { return new Uri(EndPoint); } }

        private HttpClient httpClient = new HttpClient();
        private List<KeyValuePair<string, string>> postDataPairs = new List<KeyValuePair<string, string>>();

        public Action<ServerListResponse> OnServerListRetrieved;
        public Action<string> OnServerRegistered;
        public Action OnServerRemoved;

        #endregion

        #region Communication Routines
        /// <summary>
        /// Retrieves the list of servers currently registered with the NodeLS Endpoint.
        /// </summary>
        /// <param name="CommunicationKey">The communication key used to authenicate.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void RetrieveServerList(string CommunicationKey)
        {
            // Did the user forget to set the endpoint?
            if (string.IsNullOrWhiteSpace(EndPoint))
                throw new ArgumentNullException(EndPoint);

            // Did the user forget to specify the communication key?
            if (string.IsNullOrWhiteSpace(CommunicationKey))
                throw new ArgumentNullException(CommunicationKey);

            // Sanity checks passed, let's attempt communication.
            httpClient.BaseAddress = EndPointAsUri;

            // Create 
            postDataPairs.Clear();
            postDataPairs.Add(new KeyValuePair<string, string>("serverKey", CommunicationKey.Trim()));

            var postContent = new FormUrlEncodedContent(postDataPairs);
            HttpResponseMessage theResult = null;

            try
            {
                theResult = httpClient.PostAsync(QueryEndpoint, postContent).Result;
            }
            catch
            {
                // Swallow it
            }

            // Was this successful?
            if (theResult != null && theResult.IsSuccessStatusCode)
            {
                var contentAsString = theResult.Content.ReadAsStringAsync().Result.Trim();
                UnityEngine.Debug.Log(contentAsString);
                OnServerListRetrieved?.Invoke(JsonUtility.FromJson<ServerListResponse>(contentAsString));
                Busy = false;
            }
            else
            {
                // Throw an exception.
                Busy = false;
                throw new Exception($"Failed retrieving Server List.");
            }
        }

        /// <summary>
        /// Registers a server with the NodeLS instance
        /// </summary>
        /// <param name="CommunicationKey">The communication key used to authenicate.</param>
        /// <returns>A string containing a GUID (UUID) identifying the server on the NodeLS instance.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public string RegisterServer(string CommunicationKey, ServerInfo newServerInfo)
        {
            // Did the user forget to set the endpoint?
            if (string.IsNullOrWhiteSpace(EndPoint))
                throw new ArgumentNullException(EndPoint);

            // Did the user forget to specify the communication key?
            if (string.IsNullOrWhiteSpace(CommunicationKey))
                throw new ArgumentNullException(CommunicationKey);

            // TODO: Server information checks

            // Sanity checks passed, let's attempt communication.
            httpClient.BaseAddress = EndPointAsUri;

            postDataPairs.Clear();
            postDataPairs.Add(new KeyValuePair<string, string>("serverKey", CommunicationKey.Trim()));
            postDataPairs.Add(new KeyValuePair<string, string>("serverName", newServerInfo.Name));
            // Possibly in the future. Does nothing atm.
            // postDataPairs.Add("serverIp", newServerInfo.Ip);
            postDataPairs.Add(new KeyValuePair<string, string>("serverPort", newServerInfo.Port.ToString()));
            postDataPairs.Add(new KeyValuePair<string, string>("serverPlayers", newServerInfo.Count.ToString()));
            postDataPairs.Add(new KeyValuePair<string, string>("serverCapacity", newServerInfo.Capacity.ToString()));
            postDataPairs.Add(new KeyValuePair<string, string>("serverExtras", newServerInfo.ExtraInformation == null ? string.Empty : newServerInfo.ExtraInformation.ToString()));

            var postContent = new FormUrlEncodedContent(postDataPairs);
            HttpResponseMessage theResult = null;
            try
            {
                theResult = httpClient.PostAsync(RegisterEndpoint, postContent).Result;
            }
            catch
            {
                // Swallow it
            }


            // Was this successful?
            if (theResult != null && theResult.IsSuccessStatusCode)
            {
                var contentAsString = theResult.Content.ReadAsStringAsync().Result.Trim();
                OnServerRegistered?.Invoke(contentAsString);
                return contentAsString;
            }
            else
            {
                // Throw an exception.
                throw new Exception($"Failed registering server. More information available on the NodeLS instance console.");
            }
        }

        public void DeregisterServer(string CommunicationKey, string ServerUuid)
        {
            // Did the user forget to set the endpoint?
            if (string.IsNullOrWhiteSpace(EndPoint))
                throw new ArgumentNullException(EndPoint);

            // Did the user forget to specify the communication key?
            if (string.IsNullOrWhiteSpace(CommunicationKey))
                throw new ArgumentNullException(CommunicationKey);

            // Gotta have a server uuid to use this!
            if (string.IsNullOrWhiteSpace(ServerUuid))
                throw new ArgumentNullException(ServerUuid);

            // Sanity check passed, attempt contact.
            httpClient.BaseAddress = EndPointAsUri;

            List<KeyValuePair<string, string>> postDataPairs = new List<KeyValuePair<string, string>>
            {
                // Server Auth Key
                new KeyValuePair<string, string>("serverKey", CommunicationKey.Trim()),
                // Server UUID to remove
                new KeyValuePair<string, string>("serverUuid", ServerUuid.Trim())
            };

            var postContent = new FormUrlEncodedContent(postDataPairs);
            HttpResponseMessage theResult = null;
            try
            {
                theResult = httpClient.PostAsync(RemoveEndpoint, postContent).Result;
            }
            catch
            {
                // Swallow it
            }
            
            // Was this successful?
            if (theResult != null && theResult.IsSuccessStatusCode)
            {
                // Fire it off.
                OnServerRemoved?.Invoke();
            }
            else
            {
                // Fire it off anyway.
                OnServerRemoved?.Invoke();

                // Throw an exception.
                throw new Exception($"Failed deregistering server. This servers' listing may have expired or the NodeLS server was restarted. " +
                    $"HTTP status code {theResult.StatusCode}.");
            }
        }

        #endregion

        #region Callback functions

        #endregion
    }
}