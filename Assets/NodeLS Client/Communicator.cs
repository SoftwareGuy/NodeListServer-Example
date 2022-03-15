using System;
using System.Collections;
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

        public Action<ServerListResponse> OnServerListRetrieved;
        public Action<string> OnServerRegistered;
        public Action OnServerRemoved;

        #endregion

        #region Communication Routines
        /// <summary>
        /// <para>Retrieves the list of servers currently registered with the NodeLS Endpoint.</para>
        /// <para>If something goes wrong, a Debug.LogError call is made.</para>
        /// </summary>
        /// <param name="CommunicationKey">The communication key used to authenicate.</param>
        public IEnumerator RetrieveList(string commKey)
        {
            // Sanity checks: Did the user forget to set the endpoint? Did the user forget to
            // specify the communication key?

            if (string.IsNullOrWhiteSpace(EndPoint) || string.IsNullOrWhiteSpace(commKey))
            {
                Debug.LogError("NodeLS Client: Endpoint or Communicaton key is invalid.");
                yield break;
            }

            WWWForm postRequest = new WWWForm();
            postRequest.AddField("serverKey", commKey.Trim());

            using (UnityWebRequest requestRunner = UnityWebRequest.Post($"{EndPoint}{QueryEndpoint}", postRequest))
            {
                Busy = true;

                yield return requestRunner.SendWebRequest();

                if (requestRunner.result == UnityWebRequest.Result.Success)
                {
                    // Invoke the data received event.
                    OnServerListRetrieved?.Invoke(JsonUtility.FromJson<ServerListResponse>(requestRunner.downloadHandler.text.Trim()));
                }
                else
                {
                    Debug.LogError($"NodeLS Client: Error processing request. Status returned was {requestRunner.result}.");
                }
            }

            Busy = false;
            yield return null;
        }

        /// <summary>
        /// Registers a server with the NodeLS instance
        /// </summary>
        /// <param name="CommunicationKey">The communication key used to authenicate.</param>
        /// <returns>A string containing a GUID (UUID) identifying the server on the NodeLS instance.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IEnumerator RegisterServer(string commKey, ServerInfo newServerInfo)
        {
            // Sanity checks: Did the user forget to set the endpoint? Did the user forget to
            // specify the communication key?

            if (string.IsNullOrWhiteSpace(EndPoint) || string.IsNullOrWhiteSpace(commKey))
            {
                Debug.LogError("NodeLS Client: Endpoint or Communicaton key is invalid.");
                yield break;
            }

            Busy = true;

            // TODO: Server information checks
            WWWForm postRequest = new WWWForm();
            postRequest.AddField("serverKey", commKey.Trim());
            postRequest.AddField("serverName", newServerInfo.Name.Trim());
            // Possibly in the future, doesn't do anything atm.
            // ostRequest.AddField("serverIp", newServerInfo.Ip);
            postRequest.AddField("serverPort", newServerInfo.Port);
            postRequest.AddField("serverPlayers", newServerInfo.Count);
            postRequest.AddField("serverCapacity", newServerInfo.Capacity);
            postRequest.AddField("serverExtras", newServerInfo.ExtraInformation == null ? string.Empty : newServerInfo.ExtraInformation.ToString());

            using (UnityWebRequest requestRunner = UnityWebRequest.Post($"{EndPoint}{RegisterEndpoint}", postRequest))
            {
                yield return requestRunner.SendWebRequest();

                if (requestRunner.result == UnityWebRequest.Result.Success)
                {
                    // It works!
                    OnServerRegistered?.Invoke(requestRunner.downloadHandler.text.Trim());
                }
                else
                {
                    Debug.LogError($"NodeLS Client: Error processing register request. Status returned was {requestRunner.result}.");
                }
            }

            Busy = false;
            yield return null;
        }

        public IEnumerator DeregisterServer(string commKey, string serverUuid)
        {
            if (string.IsNullOrWhiteSpace(EndPoint) || string.IsNullOrWhiteSpace(commKey) || string.IsNullOrWhiteSpace(serverUuid))
            {
                Debug.LogError("NodeLS Client: Endpoint, communicaton key or server UUID is invalid.");
                yield break;
            }

            WWWForm postRequest = new WWWForm();
            postRequest.AddField("serverKey", commKey.Trim());
            postRequest.AddField("serverUuid", serverUuid.Trim());

            using (UnityWebRequest requestRunner = UnityWebRequest.Post($"{EndPoint}{RemoveEndpoint}", postRequest))
            {
                yield return requestRunner.SendWebRequest();

                if (requestRunner.result == UnityWebRequest.Result.Success)
                {
                    // It works!
                    OnServerRemoved.Invoke();
                }
                else
                {
                    Debug.LogError($"NodeLS Client: Error processing deregister request. Status returned was {requestRunner.result}.");
                }
            }
        }

        #endregion

        #region Callback functions

        #endregion
    }
}