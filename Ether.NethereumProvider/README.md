# Ether.NethereumProvider

This library provides a [Netherem](https://nethereum.com) IWeb3 client interface for use within Blazor WebAssembly.

## How to use

Register the provider:

```cs
builder.Services.AddNethereumProvider();
```

Inject the provider into a Razor page:

```cs
@using MetaMask.NethereumProvider
@inject INethereumMetaMaskProvider Provider;
```

Using the provider:

Check if MetaMask has been installed

```cs
bool isAvailable = await Provider.IsMetaMaskAvailable();
```

Connect to MetaMask:

```cs
await Provider.Connect();
```

Create a IWeb3 client

```cs
Nethereum.Web3.IWeb3 web3 = await Provider.CreateWeb3();
```