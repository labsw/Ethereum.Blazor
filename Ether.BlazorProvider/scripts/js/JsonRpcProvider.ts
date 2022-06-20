import { promiseTimeout } from './utils.js'

export interface IJsonRpcProviderOptions {
    providerPath: string;
    supportsEip1193: boolean;
}

export interface IJsonRpcProvider {
    isAvailable(): boolean;
    request(request: JsonRpcRequest, timeoutInMs?: number): Promise<any>;
    configureEvents(dotNetReference: any);
}

export interface JsonRpcRequest {
    method: string;
    params?: unknown[] | object;
}

export interface JsonRpcResponse {
    id: any,
    jsonrpc: string,
    result: any
    error: any
}

export interface JsonRpcResponseError {
    code: number,
    message?: string,
    date?: any
}

//-- 

export abstract class JsonRpcProvider implements IJsonRpcProvider {

    private _options: IJsonRpcProviderOptions;

    constructor(options: IJsonRpcProviderOptions) {
        this._options = options;
    }

    isAvailable() {
        return false;
    }

    request(request: JsonRpcRequest, timeoutInMs?: number): Promise<any> {

        if (timeoutInMs != null) {
            return promiseTimeout(this.requestImp(request), timeoutInMs, new Error("Timeout"));
        }

        return this.requestImp(request);
    }

    configureEvents(dotNetReference: any) {
        // default is do nothing
    }

    abstract requestImp(request: JsonRpcRequest): Promise<any>;
}

export class JsonRpcProviderRcpErrorException extends Error {
    private readonly _rpcError: any;
    constructor(rpcError: any) {
        super();
        this._rpcError = rpcError;
        Object.setPrototypeOf(this, JsonRpcProviderRcpErrorException.prototype);
    }

    get rpcError(): any {
        return this._rpcError;
    }
}
