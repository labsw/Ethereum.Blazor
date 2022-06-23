# Ethereum.Blazor

This repository contains two libraries

* [Ether.BlazorProvider](Ether.BlazorProvider/README.md) - this library provides a interface to web3 compatible browser plugins like MetaMask.
* [Ether.NethereumProvider](Ether.NethereumProvider/README.md) - this library provides a Nethereum IWeb3 client using using the  Ether.BlazorProvider as the RPC endpoint.

## Ether.BlazorProvider

This library has no additional dependencies other that MetaMask browser extension.

## Ether.NethereumProvider

This library depends on the Ether.BlazorProvider and [Nethereum](https://nethereum.com)

## Warning

Executing blockchain transactions are irreversible. Ensure sufficient testing has been done on testnet's before any live transactions are attempted.

This software is provided as is with no warranty of any kind, see the [license](LICENSE)

## Comments / Suggestions

Feel free to suggest any improvements or bug fixes