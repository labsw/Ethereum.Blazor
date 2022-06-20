
// put everything in one file for now - i was having problems getting the imports to work with the blazor dynamic loading

const globalWindow = (window as any);
const resolveObjFromPath = (object, path) => path.split('.').reduce((o, p) => o?.[p], object)

function promiseTimeout<T>(prom: Promise<T>, timeInMs: number, timeoutException: any) {
    let timer: any;
    return Promise.race([
        prom,
        new Promise((_r, rej) => timer = setTimeout(rej, timeInMs, timeoutException))
    ]).finally(() => clearTimeout(timer));
}

//const promiseTimeout = (prom: Promise<T>, time: number) => {
//    let timer;
//    return Promise.race([
//        prom,
//        new Promise((_r, rej) => timer = setTimeout(rej, time))
//    ]).finally(() => clearTimeout(timer));
//}

//-- JsonRpcProvider ----------------------------------------------------------

interface JsonRpcRequest {
    method: string;
    params?: unknown[] | object;
}

interface JsonRpcResponse {
    id: any,
    jsonrpc: string,
    result: any
    error: any
}

interface JsonRpcResponseError {
    code: number,
    message?: string,
    date?: any
}

export interface IJsonRpcProviderOptions {
    providerPath: string;
    supportsEip1193: boolean;
}

interface IJsonRpcProvider {
    isAvailable(): boolean;
    request(request: JsonRpcRequest, timeoutInMs?: number): Promise<any>;
    getOptions(): IJsonRpcProviderOptions;
}

//-- 

abstract class JsonRpcProvider implements IJsonRpcProvider {

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

    getOptions(): IJsonRpcProviderOptions {
        return this._options;
    }

    abstract requestImp(request: JsonRpcRequest): Promise<any>;
}

//--

type ExternalProvider = {
    sendAsync?: (request: JsonRpcRequest, callback: (error: any, response: any) => void) => void
    send?: (request: JsonRpcRequest, callback: (error: any, response: any) => void) => void
    request?: (request: JsonRpcRequest) => Promise<any>
}


class JsonRpcProviderRcpError extends Error {
    private readonly _rpcError: any;
    constructor(rpcError:any) {
        super();
        this._rpcError = rpcError;
        Object.setPrototypeOf(this, JsonRpcProviderRcpError.prototype);
    }

    get rpcError(): any {
        return this._rpcError;
    }
}

class Web3Provider extends JsonRpcProvider {

    private _fetchFunc: JsonRpcFetchFunc;
    private _externalProvider: ExternalProvider;

    constructor(externalProvider: ExternalProvider, options: IJsonRpcProviderOptions) {
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

    //--

    private buildFetchFunc(externalProvider: ExternalProvider): JsonRpcFetchFunc {

        if (externalProvider.request) {
            return this.buildEip1193FetchFunc(externalProvider);
        } else if (externalProvider.sendAsync) {
            return this.buildLegacyFetchFunc(externalProvider.sendAsync);
        } else if (externalProvider.send) {
            return this.buildLegacyFetchFunc(externalProvider.send);
        } else {
            throw "Unsupported externalProvider";
        }
    }

    private buildEip1193FetchFunc(externalProvider: ExternalProvider): JsonRpcFetchFunc {
        return function (request: JsonRpcRequest): Promise<any> {
            return externalProvider.request(request);
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
                        const e = new JsonRpcProviderRcpError(response.error);
                        return reject(e);
                    }

                    resolve(response.result);
                });
            });
        }
    }
}



class NullProvider extends JsonRpcProvider {

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

//-- 

type JsonRpcFetchFunc = (request: JsonRpcRequest) => Promise<any>;
type Web3LegacySendFunc = (request: any, callback: (error: Error, response: any) => void) => void;


//-- JsonRpcProviderInterop ---------------------------------------------------

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

    get provider(): IJsonRpcProvider {
        return this._provider;
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

            if (e instanceof JsonRpcProviderRcpError) {
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

//-- EtherInterop -------------------------------------------------------------


export class EtherInterop {

    constructor() {
    }

    initProvider(name: string, options: IJsonRpcProviderOptions): JsonRpcProviderInterop {
        const provider: JsonRpcProviderInterop = this.createProvider(options);
        return provider;
    }

    //--

    createProvider(options: IJsonRpcProviderOptions): JsonRpcProviderInterop {

        // resolve the external provider from the path 
        const externalProvider: any = resolveObjFromPath(globalWindow, options.providerPath);

        if (externalProvider !== undefined) {
            const provider: IJsonRpcProvider = new Web3Provider(externalProvider, options);
            const providerInterop: JsonRpcProviderInterop = new JsonRpcProviderInterop(provider);
            return providerInterop;

        } 

        return new JsonRpcProviderInterop(new NullProvider(options));
    }
}

export const EtherInteropService: EtherInterop = new EtherInterop();