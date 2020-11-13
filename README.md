# NodeListServer Example Project

[![Ko-Fi](https://img.shields.io/badge/Donate-Ko--Fi-red)](https://ko-fi.com/coburn) 
[![PayPal](https://img.shields.io/badge/Donate-PayPal-blue)](https://paypal.me/coburn64)
![MIT Licensed](https://img.shields.io/badge/license-MIT-green.svg)

_**Please consider a donation (see the Ko-Fi button above) if this project is useful to you.**_

This repository contains a project that shows off NodeListServer in action. Included is both server and client functionality and is easy to demostrate. The server and client logic are both set up to communicate with Furutaka, the Oiran Studio in-house development server.

_**It is advised that this project is only used as a reference and not a "complete" project. Think of it like a reference manual, not something you'd slap into a project and expect it to make you a million dollars overnight.**_

You are strongly advised NOT to use Furutaka's NodeLS instance in production as Oiran Studio cannot promise that it will be running forever. Instead, learn how to run your own instance by checking out the [NodeListServer repository](https://github.com/SoftwareGuy/NodeListServer). **Check your editor logs for any errors before you file a Issue Ticket.** There is a fair chance that the demo NodeListServer instance may be shut down for various reasons.

## Setup
1.  Open the List Example scene, click Logic gameobject, and change the masterServerUrl and communicationKey, the key and port you will have set in Node List Server.config file on your vps.  ( example masterServerUrl:  http://MyVPS:8008/list )
2.	Locate and open the script called: NodeListServerCommunicationManager, and set the two variables, Server and AuthKey, same as above.

## Server Mode

1.  Open the project in your version of Unity. Remember to use a version supported with Mirror, which is 2018.3 - 2019.x. **Alpha/Beta versions are not supported.**
2.	Go to File -> Build Settings -> Make sure both scenes are listed in the Build Scenes. 
3.	Tick "Server Build", then click `Build and Run`.
4.	If prompted, point it to a empty folder and wait for it to build.
5.	Once it completes, it should automatically pop up a new window (on Windows, maybe Mac/Linux too) and go through the cycles of starting up a server instance. You should also see "Successfully registered server instance!" in the console.

## Client Mode

1.	Open the project in your version of Unity. Remember to use a version supported with Mirror, which is 2018.3 - 2019.x. **Alpha/Beta versions are not supported.**
2.	Go to File -> Build Settings -> Make sure both scenes are listed in the Build Scenes. 
3.	Make sure "Server Build" is not ticked, then click `Build and Run`.
4.	If prompted, point it to a empty folder and wait for it to build.
5. 	It should auto-run when complete. When the Unity runtime finishes initializing, it should load into the List Server Client user interface. If you have a server running, you should see it in the list.

## Scripts

The main logic you want to take a look at is the `NodeListServerNetMan.cs` file. That is pretty much a barebones Network Manager with NodeListServer Adapter calls added where appropriate. See the Example scene's objects in its Hierarchy. If you're using custom code, just add the Node List Server Adapter component somewhere, and then you can call it's public functions where needed.

## Wait, why I cannot connect to a listed server?

-   Unless your NodeLS instance is inside your local network, NodeLS will detect your **public** IP address. Most routers do not support loopback connections, which is basically a inside connection calling the external outside IP address.
-   Some ISPs block game server traffic. Comcast, Verizon and Vodafone are some that do traffic blocking, including servers on non-business grade connections. You're screwed if your ISP does that to you. **Use a VPN to workaround this, at the expense of extra latency.**
-	Mobile connections will not work for server hosting. Due to the nature of how mobile internet works, even if it *does* work partially, if you drop signal then you're in the dark.
-   The server port is closed or needs Port Forwarding. If the server is on your ISP connection, open that port in your router - read the manual to figure out how to do that. **Keep in mind some ISPs can disallow port forwarding for stupid reasons, or they'll penalize you for doing so.** If it's on a Windows/Linux cloud instance, open the firewall using Windows Firewall or iptables, although if you're using Google Cloud/Amazon AWS/Microsoft Azure then that's a clusterfuck of a firewall system to deal with.
-   The server died. It happens, as it's Unity Engine after all.

## I need help!

-	Open a issue ticket, although I expect you to know the absolute basics of C# and Unity Engine first.
-	Don't be shy asking for help, but please give details as to what you tried, and what went wrong.

## Credits

- 	AnthonyE: First NodeLS instance in production
-	JesusLuvsYooh: Some say he's still pestering me with NodeLS bugfixes, but for what we know he's a great comrade
-	Mirror Team & Discord Members: Warm and friendly community based around a great network stack

_**Thanks for using Australian Open Source Software!**_
