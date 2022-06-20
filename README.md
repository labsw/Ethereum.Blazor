# Ethereum.Blazor

This repository contains two libraries

* [Ether.BlazorProvider](Ether.BlazorProvider/README.md) - this library provides a interface to web3 compatible browser plugins like MetaMask.
* [Ether.NetherermProvider](Ether.NethereumProvider/README.md) - this library provides a Nethereum IWeb3 client using using the  Ether.BlazorProvider as the RPC endpint.

## Ether.BlazorProvider

This library has no additional depedencies other that MetaMask browser extension.

## Ether.NetherermProvider

This library depends on the Ether.BlazorProvider and [Netherem](https://nethereum.com)

## Warning

Executing blockchain transactions are irreversible. Ensure sufficient testing has been done on testnet's before any live transactions are attempted.

This software is provided as is wth no warranty of any kind, see the [license](LICENSE)

## Commets / Suggestions

Feel free to suggest any improvements or bug fixes