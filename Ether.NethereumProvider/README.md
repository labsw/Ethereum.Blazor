# Ether.NethereumProvider

This library provides a [Netherem](https://nethereum.com) IWeb3 client interface for use within Blazor WebAssembly.

## Usage

### Configuration

Register the components:

```cs

// configure EtherProvider - NethereumProvider
builder.Services.AddEtherProviderRegistry(config => config.AddMetaMaskProvider());


builder.Services.AddNethereumProviderRegistry();
```
### Using the provider

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

## Sample App

See [Ether.NethereumProvider.Sample](https://github.com/labsw/Ethereum.Blazor/tree/master/Ether.NethereumProvider.Sample) for a simple working example

## Warning

Executing blockchain transactions are irreversible. Ensure sufficient testing has been done using testnet's before any live transactions are attempted.

This software is provided as is with no warranty of any kind, see the [license](../LICENSE)

## Comments / Suggestions

Feel free to suggest any improvements or bug fixes.
