# Ether.BlazorProvider

This library provides an interface to web3 compatible browser plugins like MetaMask for use within Blazor WebAssembly.

## How to use

Register the provider:

```cs
builder.Services.AddEtherProvider();
```

Inject the provider into a Razor page:

```cs
@using Ether.BlazorProvider
@inject IEtherProvider EtherProvider;
```

Initialise the provider:

Parameters

* name: this is user supplied and can be anything
* options:
  * ProviderPath: this is the global javascript object that the browser plugin creates for the provider.
  * SupportsEip1193: indicates if the provider supports EIP 1193, the default is true

Example: if the global javascript is "window.ronin.provider" then ProviderPath will be "ronin.provider". The window part is not required and will be ignored if included.

```cs
var options = new JsonRpcProviderOptions()
    {
        ProviderPath = "ethereum",
        SupportsEip1193 = true
    };

IJsonRpcProvider myProvider = await _etherProvider.InitProvider("my-provider", options );
```

Check if the provider path is available

```cs
bool isAvailable = await myProvider.IsAvailable();
```

Connect to the provider:

```cs
string connectedAccount = await myProvider.Connect();
```

The currently selected account address is returned if the connect is successful, otherwise exceptions can be thrown if access is denied. ```Connect()``` should be called before using any of the other methods.

Connect can take a optional timeout parameter which will results in an exception being thrown if the timeout occurs.


To get the account or chain ID.

```cs
string account = await myProvider.GetAccount();
long chainId = await myProvider.GetChainId();
```


To make general Ethereum RPC calls use ```Request()```. The methods and parameters of the RPC calls must conform to the [Ethereum RPC specification](https://eth.wiki/json-rpc/API#json-rpc-methods).

There are two signatures to the ```Request()``` call. One that takes an object and one that takes a JSON string. The JSON string must conform to the [Ethereum RPC specification](https://eth.wiki/json-rpc/API#json-rpc-methods).

```cs
var request = new RpcRequestMessage(1, "eth_getBalance", Account, "latest");
RpcResponseMessage response = await myProvider.Request(request);
```

## Sample App

See Ether.BlazorProvider.Sample for a simple working example

## Warning

Executing blockchain transactions are irreversible. Ensure sufficient testing has been done on testnet's before any live transactions are attempted.

This software is provided as is wth no warranty of any kind, see the [license](../LICENSE)

## Comments / Suggestions

Feel free to suggest any improvements or bug fixes