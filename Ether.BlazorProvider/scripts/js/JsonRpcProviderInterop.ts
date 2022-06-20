import { IJsonRpcProvider, JsonRpcRequest, JsonRpcResponse, JsonRpcProviderRcpErrorException} from './JsonRpcProvider.js'

export class JsonRpcProviderInterop {
    private _provider: IJsonRpcProvider;

    constructor(provider: IJsonRpcProvider) {
        this._provider = provider;
    }

    isAvailable() {
        return this._provider.isAvailable();
    }

    async request(requestJson: string, timeoutInMs?: number): Promise<string> {
        const request: JsonRpcRequest = JSON.parse(requestJson);
        const response = await this.requestWithResponse(request, timeoutInMs);

        return JSON.stringify(response);
    }

    configureEvents(dotNetReference: any) {
        this._provider.configureEvents(dotNetReference);
    }

    //--

    private async requestWithResponse(request: any, timeoutInMs?: number): Promise<JsonRpcResponse> {
        try {

            const response = await this._provider.request(request, timeoutInMs);

            const rpcResponse = {
                jsonrpc: "2.0",
                result: response,
                id: request.id,
                error: null
            }
            return rpcResponse;

        } catch (e) {
            // rpc errors are throw so we catch all errors

            if (e instanceof JsonRpcProviderRcpErrorException) {
                // if our provider implementation throw an rpc error - handle it

                const rpcResponseError = {
                    jsonrpc: "2.0",
                    id: request.id,
                    result: null,
                    error: e.rpcError
                }

                return rpcResponseError;

            } else if ('code' in e) {
                // other external errors could be rpc errors - so if the error has a "code" property then assume its an rpc error
                const rpcResponseError = {
                    jsonrpc: "2.0",
                    id: request.id,
                    result: null,
                    error: e
                }

                return rpcResponseError;
            }

            // this is probably some unknown error
            throw e;
        }
    }

}

