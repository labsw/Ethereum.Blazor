import { JsonRpcProvider, IJsonRpcProviderOptions, JsonRpcRequest, JsonRpcResponseError } from './JsonRpcProvider.js'

export class NullProvider extends JsonRpcProvider {

    constructor(options: IJsonRpcProviderOptions) {
        super(options);
    }

    requestImp(request: JsonRpcRequest): Promise<any> {
        const error: JsonRpcResponseError = {
            message: "Provider not available",
            code: -32603
        };

        throw error;
    }
}


