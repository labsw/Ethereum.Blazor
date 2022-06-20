import { JsonRpcProvider, JsonRpcRequest, IJsonRpcProviderOptions } from './JsonRpcProvider.js'

interface IEip1193ExternalProvider {
    request(request: JsonRpcRequest): Promise<any>;
    on(event: string, listener: any);
    removeListener(event: string, listener: any);
}

export class Eip1193Provider extends JsonRpcProvider {

    private _externalProvider: IEip1193ExternalProvider;
    private _accountChangedHandler: (account:string[]) => void;
    private _chainChangedHandler: (account: string) => void;
    private _dotNetReference?: any;

    constructor(externalProvider: IEip1193ExternalProvider, options: IJsonRpcProviderOptions ) {
        super(options);

        this._externalProvider = externalProvider;
        this._dotNetReference = null;

        this._accountChangedHandler = this.accountChanged.bind(this);
        this._chainChangedHandler = this.chainChanged.bind(this);
    }

    isAvailable() {
        return true;
    }

    requestImp(request: JsonRpcRequest): Promise<any> {
        return this._externalProvider.request(request);
    }

    configureEvents(dotNetReference: any) {
        this._dotNetReference = dotNetReference;
        this._externalProvider.on("accountsChanged", this._accountChangedHandler);
        this._externalProvider.on("chainChanged", this._chainChangedHandler);
    }

    //--

    private accountChanged(accounts: string[]) {
        this._dotNetReference.invokeMethodAsync('AccountChanged', accounts[0]);

    }

    private chainChanged(chainId: string) {
        this._dotNetReference.invokeMethodAsync('ChainChanged', chainId.toString());
    }

    removeEvents() {
        this._externalProvider.removeListener("accountsChanged", this._accountChangedHandler);
        this._externalProvider.removeListener("chainChanged", this._chainChangedHandler);
    }
}
