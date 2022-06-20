import { JsonRpcProvider, JsonRpcRequest, IJsonRpcProviderOptions, JsonRpcProviderRcpErrorException } from './JsonRpcProvider.js'

type Web3LegacySendFunc = (request: any, callback: (error: Error, response: any) => void) => void;

type LegacyExternalProvider = {
    sendAsync?: Web3LegacySendFunc
    send?: Web3LegacySendFunc
}

type JsonRpcFetchFunc = (request: JsonRpcRequest) => Promise<any>;

export class LegacyProvider extends JsonRpcProvider {

    private _fetchFunc: JsonRpcFetchFunc;
    private _externalProvider: LegacyExternalProvider;

    constructor(externalProvider: LegacyExternalProvider, options: IJsonRpcProviderOptions) {
        super(options);

        this._fetchFunc = this.buildFetchFunc(externalProvider);
        this._externalProvider = externalProvider;
    }

    isAvailable() {
        return true;
    }

    requestImp(request: JsonRpcRequest): Promise<any> {
        return this._fetchFunc(request);
    }

    configureEvents(dotNetReference: any) {
        // do nothing - legacy providers do not support a standard event interface
    }

    //--

    private buildFetchFunc(externalProvider: LegacyExternalProvider): JsonRpcFetchFunc {

        if (externalProvider.sendAsync) {
            return this.buildLegacyFetchFunc(externalProvider.sendAsync);
        } else if (externalProvider.send) {
            return this.buildLegacyFetchFunc(externalProvider.send);
        } else {
            throw "Unsupported legacy externalProvider";
        }
    }

    private buildLegacyFetchFunc(sendFunc: Web3LegacySendFunc): JsonRpcFetchFunc {
        return function (request: JsonRpcRequest): Promise<any> {

            return new Promise((resolve, reject) => {

                sendFunc(request, (error, response) => {

                    // a non rpc error
                    if (error) {
                        return reject(error);
                    }

                    // a rpc error = build an rpc error object reject (throw) that object
                    if (response.error) {
                        const e = new JsonRpcProviderRcpErrorException(response.error);
                        return reject(e);
                    }

                    resolve(response.result);
                });
            });
        }
    }
}
