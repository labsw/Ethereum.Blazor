# MetaMask.Blazor.Provider

This library provides a simple interface to MetaMask for use within Blazor WebAssembly.

## How to use

Register the provider:

```cs
builder.Services.AddMetaMaskProvider();
```

Inject the provider into a Razor page:

```cs
@using MetaMask.BlazorProvider
@inject IMetaMaskProvider MetaMaskProvider;
```

Using the provider:

Check if MetaMask has been installed

```cs
bool isAvailable = await MetaMaskProvider.IsMetaMaskAvailable();
```

Connect to MetaMask:

```cs
string connectedAccount = await MetaMaskProvider.Connect();
```

The currently selected account address is returned if the connect is successful, otherwise exceptions can be thrown if access is denied. ```Connect()``` should be called before using any of the other methods.

Handle update events:

```cs
await MetaMaskProvider.ListenForChanges();
```

This enables the provider to listen for account or chain changes. Add event handles to  ```AccountChanged``` or ```ChainIdChanged``` to get notified of the changes. ```ListenForChanges``` should just be called once. The event handlers should be removed once they are no longer required.

The event handlers can be added before ```Connect()``` or ```ListenForChanges``` have been called.

```cs
// add events handlers
MetaMaskProvider.AccountChanged += OnAccountChanged;    
MetaMaskProvider.ChainIdChanged += OnChainIdChanged;    

// remove event handlers
MetaMaskProvider.AccountChanged -= OnAccountChanged;    
MetaMaskProvider.ChainIdChanged -= OnChainIdChanged;    
```

To get the account or chain ID.

```cs
string account = await MetaMaskProvider.GetAccount();
long chainId = await MetaMaskProvider.GetChainId();
```

To sign a message use ```SignMessage()```

```cs
string signedMessage = await MetaMaskProvider.SignMessage("hello, world");
```

To make general Ethereum RPC calls use ```RpcRequest()```. The methods and parameters of the RPC calles must conform to the [Ethereum RPC specification](https://eth.wiki/json-rpc/API#json-rpc-methods).

There are two signatures to the ```RpcRequest()``` call. One that takes an object and one that takes a JSON string. The JSON string must conform to the [Ethereum RPC specification](https://eth.wiki/json-rpc/API#json-rpc-methods).

```cs
var request = new RpcRequestMessage(1, "eth_getBalance", Account, "latest");
RpcResponseMessage response = await MetaMaskProvider.RpcRequest(request);
```

## Warning

Executing blockchain transactions are irreversible. Ensure sufficient testing has been done on testnet's before any live transactions are attempted.

This software is provided as is wth no warranty of any kind, see the [license](../LICENSE)

## Commets / Suggestions

Feel free to suggest any improvements or bug fixes