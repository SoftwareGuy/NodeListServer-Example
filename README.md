# NodeListServer Example Project

[![Ko-Fi](https://img.shields.io/badge/Donate-Ko--Fi-red)](https://ko-fi.com/coburn) 
[![PayPal](https://img.shields.io/badge/Donate-PayPal-blue)](https://paypal.me/coburn64)
![MIT Licensed](https://img.shields.io/badge/license-MIT-green.svg)

_**Please consider a donation (see the Ko-Fi button above) if this project is useful to you.**_

This repository contains a Unity 2020.3 LTS project that demostrates a implementation of a classic Server Browser (one you'd see in classic games such as Battlefield 1942, old Call of Duty games, etc). 

It is advised that this project is only used as a reference, don't expect it to be feature rich.

While the Server Browser logic is network stack agnostic (as in, it doesn't care if you're using Mirror, Mirage, a fishy-named network library or something paid), the Mirror and Mirage integrations are 
dependent on the respective network stacks and require further steps. To help keep dependencies to a minimum the mentioned network stacks are **not** included in the repository. 

## Requirements

- You must have installed and configured Node List Server. You can get it here from the [NodeListServer repository](https://github.com/SoftwareGuy/NodeListServer).

## Setup

1. 	You can either clone this repository and open it in your version of Unity, please note that any versions below Unity 2020 LTS are unsupported and I cannot have 50 different Unity versions on my workstation. Alternatively, download a snapshot 
archive of this repository and copy the **NodeLS Client** directory into your project's Assets directory.
2. 	If you use Unity Assembly Definitions, add a reference to the `NodeLS.Client` Definition. This will allow you to access the `NodeListServer` namespace.
3. 	You're good to go! You can now create class instances of the NodeLS Client. An example would be `NLSCommunicator myCommunicator = new NLSCommunicator();`. 

Review the `Communicator.cs` as it's commented and does explain what it's trying to do.

### Testing with DebugCommunicator

-	There is a `DebugCommunicator` script that I used during development to ensure things are operating correctly. It uses three keys which are **Q**, **W** and **E**.
-	**Q** sends a request to retrieve the server list, **W** fakes a server registration request and **E** removes the fake server that it created (if it was successful).
-	You'll need to modify the Endpoint in which it uses for it to work on your setup. This script has a compile define to only be available inside the Unity Editor. It will not be compiled in standalone builds.
-	Using this script outside of a testing environment is a bad idea. You've been warned.

### Integration Packages

-   Some integrations exist for various network stacks as mentioned earlier. 
-   Look inside the `NodeLS Client Integrations` folder for a UnityPackage file that contains scripts for use with the network stack the integration is for.
-   Note that some integrations may be cause a compile error or warning about things being obsolete over time. If this happens, please open a issue ticket.

## Wait, why I cannot connect to a listed server?

**The easy answers...**

-   The server died. It happens, as it's Unity Engine after all.
-   The server port is closed or needs Port Forwarding. If the server is on your ISP connection, open that port in your router: read the manual to figure out how to do that. See the footnote for additional info.

**The more technicial answers...**

*Are you trying to connect a server hosted on your network using the Server Browser?*

-   Unless your NodeLS instance is inside your local network, NodeLS will detect your **public** IP address. Your private IP might be `10.0.0.2`, but your public IP might be `42.69.4.20`.
-	Most routers may not support loop-back connections, which is basically a LAN connection calling the external outside IP address.
-   Some ISPs block game server traffic. Comcast, Verizon and Vodafone are some that do traffic blocking, including servers on non-business grade connections. You're screwed if your ISP does that to you.
-	Hosting servers behind a VPN connection is a bad idea.
-	Mobile connections will not work for server hosting. Due to the nature of how mobile internet works, even if it *does* work partially, if you drop signal then you're in the dark.

Footnote: Keep in mind some ISPs can disallow port forwarding for stupid reasons, or they'll penalize you for doing so. If it's on a Windows/Linux cloud instance, open the firewall using Windows Firewall or 
iptables, although if you're using Google Cloud/Amazon AWS/Microsoft Azure then it can be a clusterfuck.

## I need help!

-	Open a issue ticket, although I expect you to know the absolute basics of C# and Unity Engine first.
-	Don't be shy asking for help, but please give details as to what you tried, and what went wrong.

## Credits

- 	AnthonyE
-	JesusLuvsYooh
-	NodeLS users & contributors
-	Coffee donators
-	Mirror discord

_**Thanks for using Australian Open Source Software!**_
