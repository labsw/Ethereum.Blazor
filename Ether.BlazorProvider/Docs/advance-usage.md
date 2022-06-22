# Advance Usage

## Advanced Configuration

Initialise the provider:

Parameters

* name: this is user supplied and can be anything
* options:
  * ProviderPath: this is the global javascript object that the browser plugin creates for the provider.
  * SupportsEip1193: indicates if the provider supports EIP 1193, the default is true

Example: if the global javascript is "window.ronin.provider" then ProviderPath will be "ronin.provider". The window part is not required and will be ignored if included.
