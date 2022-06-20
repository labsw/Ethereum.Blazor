# MetaMask.Provider

This repository contains two libraries which provider MetaMask support within Blazor WebAssembly.

* [MetaMask.BlazorProvider](MetaMask.BlazorProvider/README.md) - this library provides a simple interface to MetaMask.
* [MetaMask.NetherermProvider](MetaMask.NethereumProvider/README.md) - this library provides a Nethereum IWeb3 client using MetaMask as the RPC endpint.

## MetaMask.BlazorProvider

This library has no additional depedencies other that MetaMask browser extension.

## MetaMask.NetherermProvider

This library depends on the MetaMask.BlazorProvider and [Netherem](https://nethereum.com)

## Warning

Executing blockchain transactions are irreversible. Ensure sufficient testing has been done on testnet's before any live transactions are attempted.

This software is provided as is wth no warranty of any kind, see the [license](LICENSE)

## Commets / Suggestions

Feel free to suggest any improvements or bug fixes