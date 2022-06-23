# Ether.BlazorProvider

This library provides an interface to web3 compatible browser plugins like MetaMask for use within Blazor WebAssembly.

## Usage

### Configuration

Basic MetaMask configuration

```cs
builder.Services.AddEtherProviderRegistry(config => config.AddMetaMaskProvider());
```

Custom configuration example

```cs
builder.Services.AddEtherProviderRegistry( config =>
    {
        // quick metamask configuration with custom name
        config.AddMetaMaskProvider("my-metamask");

        // example of disabling events
        //options.AddMetaMaskProvider("my-metamask").Configure(x => x.EnableEvents = false);

        // custom  configure
        config.AddProvider("ronin")
            .Configure(x =>
            {
                x.ProviderPath = "ronin.provider";
                x.SupportsEip1193 = false;
                x.SupportsEip1102 = false;
            });
    }
);
```

The __ProviderPath__ property is the global javascript object path to the provider. For MetaMask the global object is "window.ethereum", when configuring the __ProviderPath__ the "window" part should be removed. So for MetaMask is would be ```x.ProviderPath = "ethereum";```

### Using the provider

__Inject the registry into a Razor page:__

```cs
@using Ether.BlazorProvider
@inject IEtherProviderRegistry _registry;
```

__Get a single provider__ (this only applies if one provider has been configured)

```cs
var myProvider = await _registry.GetSingleProvider();
```

__Get a provider by name__ (acquire the provider using the configured name)

```cs
var myProvider = await _registry.GetProvider("my-metamask");
```

__Check if the provider is available:__

```cs
bool isAvailable = await myProvider.IsAvailable();
```

This checks if the underlying javascript object supplied by the browser extension exists.

__Connect to a provider:__

```cs
string connectedAccount = await myProvider.Connect();
```

The currently selected account address is returned if the connect is successful, otherwise exceptions can be thrown if access is denied. ```Connect()``` should be called before using any of the other methods.

For providers which have been configured with ```SupportsEip1102 = true;``` (the default) a "eth_requestAccounts" RPC call is made.

For providers which have been configured with ```SupportsEip1102 = false;``` a "eth_accounts" RPC is made.

Connect can take a optional timeout parameter which will results in an exception being thrown if the timeout occurs.

__To get the account or chain ID:__

```cs
string account = await myProvider.GetAccount();
long chainId = await myProvider.GetChainId();
```

__General RPC Calls:__

To make general Ethereum RPC calls use ```Request()```. The methods and parameters of the RPC calls must conform to the [Ethereum RPC specification](https://eth.wiki/json-rpc/API#json-rpc-methods).

There are two signatures to the ```Request()``` call. One that takes an object and one that takes a JSON string. The JSON string must conform to the [Ethereum RPC specification](https://eth.wiki/json-rpc/API#json-rpc-methods).

```cs
var request = new RpcRequestMessage(1, "eth_getBalance", Account, "latest");
RpcResponseMessage response = await myProvider.Request(request);
```

__Events__

The following events are available:

* AccountChanged - this is fired if the user changes the current account
* ChainIdChanged - this is fired of the user changes the current network

These events are only available if the configured provider supports EIP 1193, MetaMask does support EIP 1193

```cs
// add an event handler
myProvider.AccountChanged += OnAccountChanged;

// remove a event handler (it is good practice to remove handlers when they are no longer required)
myProvider.AccountChanged -= OnAccountChanged;


private void OnAccountChanged(string account)
{
    // todo
}

```

## Sample App

See [Ether.BlazorProvider.Sample](https://github.com/labsw/Ethereum.Blazor/tree/master/Ether.BlazorProvider.Sample) for a simple working example

## Warning

Executing blockchain transactions are irreversible. Ensure sufficient testing has been done using testnet's before any live transactions are attempted.

This software is provided as is with no warranty of any kind, see the [license](../LICENSE)

## Comments / Suggestions

Feel free to suggest any improvements or bug fixes.
