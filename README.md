# citrix-csharp-adapter
C# demo for connecting apps on Citrix server to OpenFin apps on clients.  This demo should run as a virual app on a Citrix server.

This demo accepts two arguments:  --proxy-url and --ws-url

* --proxy-url:  Manifest URL of an OpenFin app (proxy app) that proxies messages to and from this demo app via WebSocket.  The OpenFin app is launched on the client machine via Citrix Virtual channel.
* --ws-url: WebSocket server URL this demo listens to.  This URL is passed to the proxy app on client machine for connecting to this demo.

## Prerequirites
* Citrix virual channel is properly configured on the client machine
* Citrix adapter libraries, provider by OpenFin