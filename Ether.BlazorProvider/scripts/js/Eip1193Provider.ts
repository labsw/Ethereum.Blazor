import { JsonRpcProvider, JsonRpcRequest, IJsonRpcProviderOptions } from './JsonRpcProvider.js'

interface IEip1193ExternalProvider {
    request(request: JsonRpcRequest): Promise<any>;
    on(event: string, listener: any);
}

export class Eip1193Provider extends JsonRpcProvider {

    private _externalProvider: IEip1193ExternalProvider;

    constructor(externalProvider: IEip1193ExternalProvider, options: IJsonRpcProviderOptions) {
        super(options);
        this._externalProvider = externalProvider;
    }

    isAvailable() {
        return true;
    }

    requestImp(request: JsonRpcRequest): Promise<any> {
        return this._externalProvider.request(request);
    }

    configureEvents(dotNetReference: any) {

        this._externalProvider.on("accountsChanged",
            function (accounts) {
                dotNetReference.invokeMethodAsync('AccountChanged', accounts[0]);
            });

        this._externalProvider.on("chainChanged",
            function (chainId) {
                dotNetReference.invokeMethodAsync('ChainChanged', chainId.toString());
            });
    }
}
