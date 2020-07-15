using Mirror;
using NodeListServer;
using System;
using System.Collections;
using UnityEngine;

/*
	Documentation: https://mirror-networking.com/docs/Components/NetworkManager.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

public class NodeListServerNetManager : NetworkManager
{
    #region Unity Callbacks
    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// </summary>
    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    #endregion

    #region Start & Stop

    /// <summary>
    /// Set the frame rate for a headless server.
    /// <para>Override if you wish to disable the behavior or set your own tick rate.</para>
    /// </summary>
    public override void ConfigureServerFrameRate()
    {
        base.ConfigureServerFrameRate();
    }

    /// <summary>
    /// called when quitting the application by closing the window / pressing stop in the editor
    /// </summary>
    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }

    #endregion

    #region Scene Management

    /// <summary>
    /// This causes the server to switch scenes and sets the networkSceneName.
    /// <para>Clients that connect to this server will automatically switch to this scene. This is called autmatically if onlineScene or offlineScene are set, but it can be called from user code to switch scenes again while the game is in progress. This automatically sets clients to be not-ready. The clients must call NetworkClient.Ready() again to participate in the new scene.</para>
    /// </summary>
    /// <param name="newSceneName"></param>
    public override void ServerChangeScene(string newSceneName)
    {
        base.ServerChangeScene(newSceneName);
    }

    /// <summary>
    /// Called from ServerChangeScene immediately before SceneManager.LoadSceneAsync is executed
    /// <para>This allows server to do work / cleanup / prep before the scene changes.</para>
    /// </summary>
    /// <param name="newSceneName">Name of the scene that's about to be loaded</param>
    public override void OnServerChangeScene(string newSceneName) { }

    /// <summary>
    /// Called on the server when a scene is completed loaded, when the scene load was initiated by the server with ServerChangeScene().
    /// </summary>
    /// <param name="sceneName">The name of the new scene.</param>
    public override void OnServerSceneChanged(string sceneName) { }

    /// <summary>
    /// Called from ClientChangeScene immediately before SceneManager.LoadSceneAsync is executed
    /// <para>This allows client to do work / cleanup / prep before the scene changes.</para>
    /// </summary>
    /// <param name="newSceneName">Name of the scene that's about to be loaded</param>
    /// <param name="sceneOperation">Scene operation that's about to happen</param>
    /// <param name="customHandling">true to indicate that scene loading will be handled through overrides</param>
    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling) { }

    /// <summary>
    /// Called on clients when a scene has completed loaded, when the scene load was initiated by the server.
    /// <para>Scene changes can cause player objects to be destroyed. The default implementation of OnClientSceneChanged in the NetworkManager is to add a player object for the connection if no player object exists.</para>
    /// </summary>
    /// <param name="conn">The network connection that the scene change message arrived on.</param>
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
    }

    #endregion

    #region Server System Callbacks

    /// <summary>
    /// Called on the server when a new client connects.
    /// <para>Unity calls this on the Server when a Client connects to the Server. Use an override to tell the NetworkManager what to do when a client connects to the server.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerConnect(NetworkConnection conn) { 
    }

    /// <summary>
    /// Called on the server when a client is ready.
    /// <para>The default implementation of this function calls NetworkServer.SetClientReady() to continue the network setup process.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        UpdateServerInListServer();
    }

    /// <summary>
    /// Called on the server when a client adds a new player with ClientScene.AddPlayer.
    /// <para>The default implementation for this function creates a new player object from the playerPrefab.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
    }

    /// <summary>
    /// Called on the server when a client disconnects.
    /// <para>This is called on the Server when a Client disconnects from the Server. Use an override to decide what should happen when a disconnection is detected.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        UpdateServerInListServer();
    }

    /// <summary>
    /// Called on the server when a network error occurs for a client connection.
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    /// <param name="errorCode">Error code.</param>
    public override void OnServerError(NetworkConnection conn, int errorCode) { }

    #endregion

    #region Client System Callbacks

    /// <summary>
    /// Called on the client when connected to a server.
    /// <para>The default implementation of this function sets the client as ready and adds a player. Override the function to dictate what happens when the client connects.</para>
    /// </summary>
    /// <param name="conn">Connection to the server.</param>
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
    }

    /// <summary>
    /// Called on clients when disconnected from a server.
    /// <para>This is called on the client when it disconnects from the server. Override this function to decide what happens when the client disconnects.</para>
    /// </summary>
    /// <param name="conn">Connection to the server.</param>
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
    }

    /// <summary>
    /// Called on clients when a network error occurs.
    /// </summary>
    /// <param name="conn">Connection to a server.</param>
    /// <param name="errorCode">Error code.</param>
    public override void OnClientError(NetworkConnection conn, int errorCode) { }

    /// <summary>
    /// Called on clients when a servers tells the client it is no longer ready.
    /// <para>This is commonly used when switching scenes.</para>
    /// </summary>
    /// <param name="conn">Connection to the server.</param>
    public override void OnClientNotReady(NetworkConnection conn) { }

    #endregion

    #region Start & Stop Callbacks

    // Since there are multiple versions of StartServer, StartClient and StartHost, to reliably customize
    // their functionality, users would need override all the versions. Instead these callbacks are invoked
    // from all versions, so users only need to implement this one case.

    /// <summary>
    /// This is invoked when a host is started.
    /// <para>StartHost has multiple signatures, but they all cause this hook to be called.</para>
    /// </summary>
    public override void OnStartHost() { }

    /// <summary>
    /// This is invoked when a server is started - including when a host is started.
    /// <para>StartServer has multiple signatures, but they all cause this hook to be called.</para>
    /// </summary>
    public override void OnStartServer() {
        AddServerToListServer();
    }

    /// <summary>
    /// This is invoked when the client is started.
    /// </summary>
    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    /// <summary>
    /// This is called when a host is stopped.
    /// </summary>
    public override void OnStopHost() { }

    /// <summary>
    /// This is called when a server is stopped - including when a host is stopped.
    /// </summary>
    public override void OnStopServer() {
        RemoveServerFromListServer();
    }

    /// <summary>
    /// This is called when a client is stopped.
    /// </summary>
    public override void OnStopClient() { }

    #endregion

    #region NodeListServer specifics
    [Header("NodeListServer Specific Settings")]
    [SerializeField] private string ListserverConnectUrl = "http://127.0.0.1:8889";
    [SerializeField] private string ListserverAuthKey = "NodeListServerDefaultKey";
    [SerializeField] private string ServerGuid = string.Empty;
    [SerializeField] private string ServerName = "My Server";
    [Tooltip("NOT the Mirror server port. This is what will be advertised to the list server.")]
    [SerializeField] private int ServerPublicPort = 7777;

    [Tooltip("Any extra data you want to send along with the requests. Could be CSV data, etc.")]
    [SerializeField] private string ServerExtraData = string.Empty;

    [Tooltip("If a registration attempt fails (ie. bad network conditions), should it get a retry as an update?")]
    [SerializeField] private bool RetryRegistrationAsUpdateOnFail = true;
    [Tooltip("If a update attempt fails (ie. list server rebooted), should it get a retry as an registration?")]
    [SerializeField] private bool RetryUpdateAsRegistrationOnFail = false;

    [Tooltip("Should we auto-update the List Server periodically?")]
    [SerializeField] private bool UpdateServerPeriodically = true;
    [Tooltip("Value in minutes. How often the server should phone home to update it's status.")]
    [SerializeField] private int UpdateServerPeriod = 5;

    private ServerInfo CachedServerInfo = new ServerInfo();
    private bool listServerIsBusy = false;
    private bool listServerRegistered = false;

    private void PopulateCachedServerInfo()
    {
        CachedServerInfo.Name = ServerName;
        CachedServerInfo.Port = ServerPublicPort;
        CachedServerInfo.ExtraInformation = ServerExtraData;
        CachedServerInfo.PlayerCount = NetworkServer.connections.Count;
        CachedServerInfo.PlayerCapacity = maxConnections;
    }

    private void AddServerToListServer()
    {
        if (listServerIsBusy)
        {
            Debug.LogWarning("NodeLS: We're trying to register the server while we're already busy...");
            return;
        }
       
        PopulateCachedServerInfo();
        StartCoroutine(nameof(RegisterServerInternal));
    }

    private void RemoveServerFromListServer()
    {
        if (listServerIsBusy)
        {
            Debug.LogWarning("NodeLS: We're trying to remove the server while we're already busy...");
            return;
        }

        StartCoroutine(nameof(DeregisterServerInternal));
    }

    private void UpdateServerInListServer()
    {
        if (listServerIsBusy)
        {
            Debug.LogWarning("NodeLS: We're trying to update the server while we're already busy...");
            return;
        }

        PopulateCachedServerInfo();

        StartCoroutine(nameof(UpdateServerInternal), false);
    }

    // The coroutines.
    private IEnumerator RegisterServerInternal()
    {
        bool hasFailed = false;
        WWWForm registerServerRequest = new WWWForm();

        listServerIsBusy = true;

        // Assign all the fields required.
        registerServerRequest.AddField("serverKey", ListserverAuthKey);

        registerServerRequest.AddField("serverUuid", ServerGuid);
        registerServerRequest.AddField("serverName", CachedServerInfo.Name);
        registerServerRequest.AddField("serverPort", CachedServerInfo.Port);
        registerServerRequest.AddField("serverPlayers", CachedServerInfo.PlayerCount);
        registerServerRequest.AddField("serverCapacity", CachedServerInfo.PlayerCapacity);
        registerServerRequest.AddField("serverExtras", CachedServerInfo.ExtraInformation);

        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Post($"{ListserverConnectUrl}/add", registerServerRequest))
        {
            yield return www.SendWebRequest();

            if (www.responseCode == 200)
            {
                print("NodeLS: Successfully registered server with NodeListServer instance!");
                listServerRegistered = true;
            }
            else
            {
                Debug.LogError("NodeLS: Mission failed. We'll get them next time.\n" +
                    "An error occurred while registering the server. One or more required fields, like the server GUID, " +
                    $"name and port might be missing. Check the values on {gameObject.name} and try again.");
                Debug.LogError(www.error);
                hasFailed = true;
            }
        }

        listServerIsBusy = false;

        // It may have failed registration because the server came back online
        // (ie. bad connection). Try an update if desired.
        if (hasFailed && RetryRegistrationAsUpdateOnFail)
        {
            print("NodeLS: But it's not over yet. Get ready for the next round: retrying as an update as specified.");
            yield return UpdateServerInternal(true);
        }

        if (UpdateServerPeriodically)
        {
            // Schedule it to be invoked automatically.
            InvokeRepeating(nameof(UpdateServerInListServer), Time.realtimeSinceStartup + (UpdateServerPeriod * 60), UpdateServerPeriod * 60);
        }

        yield break;
    }

    private IEnumerator DeregisterServerInternal()
    {
        WWWForm deregisterServerRequest = new WWWForm();
        listServerIsBusy = true;

        // Assign all the fields required.
        deregisterServerRequest.AddField("serverKey", ListserverAuthKey);
        deregisterServerRequest.AddField("serverUuid", ServerGuid);

        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Post($"{ListserverConnectUrl}/delete", deregisterServerRequest))
        {
            yield return www.SendWebRequest();

            if (www.responseCode == 200)
            {
                print("NodeLS: Successfully Deregistered Server!");
                listServerRegistered = false;
                // Clear out any remaining invoke calls.
                CancelInvoke();
            }
            else
            {
                Debug.LogError("NodeLS: Mission failed. We'll get them next time.\n" +
                    $"An error occurred while deregistering the server. Check the values on {gameObject.name} and try again. " +
                    "Do note that there is a chance that this server instance did not update before the configured NodeListServer " +
                    "update deadline, therefore the server list entry has expired.");
            }
        }

        listServerIsBusy = false;
        yield break;
    }

    private IEnumerator UpdateServerInternal(bool overrideRegisteredCheck = false)
    {
        if (!overrideRegisteredCheck && !listServerRegistered) yield break;

        WWWForm updateServerRequest = new WWWForm();
        listServerIsBusy = true;

        // Assign all the fields required.
        updateServerRequest.AddField("serverKey", ListserverAuthKey);

        // Can't update the IP address or port while updating the server - might
        // be implemented at a later date
        updateServerRequest.AddField("serverUuid", ServerGuid);
        updateServerRequest.AddField("serverName", CachedServerInfo.Name);

        updateServerRequest.AddField("serverPlayers", CachedServerInfo.PlayerCount);
        updateServerRequest.AddField("serverCapacity", CachedServerInfo.PlayerCapacity);
        updateServerRequest.AddField("serverExtras", CachedServerInfo.ExtraInformation);

        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Post($"{ListserverConnectUrl}/update", updateServerRequest))
        {
            yield return www.SendWebRequest();

            if (www.responseCode == 200)
            {
                print("NodeLS: Successfully updated server information!");
                if (overrideRegisteredCheck) listServerRegistered = true;
            }
            else
            {
                Debug.LogError("NodeLS: Mission failed. We'll get them next time.\n" +
                    "An error occurred while updating the server information. The communication key or the server GUID might be wrong, or some" +
                    " other information is bogus. Or it could be you are experiencing connection problems.");

                if (RetryUpdateAsRegistrationOnFail)
                {                    
                    print("NodeLS: But it's not over yet, get ready for the next round. Retrying update as registration.");
                    listServerRegistered = false;
                    Invoke(nameof(RegisterServerInternal), 0f);
                }
            }
        }

        listServerIsBusy = false;
        yield break;
    }

    #endregion

#if UNITY_EDITOR
    public override void OnValidate()
    {
        base.OnValidate();

        // Make sure our GUID is valid.
        if (string.IsNullOrEmpty(ServerGuid))
        {
            // Generate a new one.
            Guid randomId = Guid.NewGuid();
            ServerGuid = randomId.ToString();

            print($"NodeListServerNetManager: I automatically assigned a GUID to the Server Adapter: {ServerGuid}. Good to go!");
        }
    }
#endif
}
