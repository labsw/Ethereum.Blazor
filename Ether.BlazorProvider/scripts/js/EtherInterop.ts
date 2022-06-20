import { IJsonRpcProvider, IJsonRpcProviderOptions  } from './JsonRpcProvider.js'
import { JsonRpcProviderInterop } from './JsonRpcProviderInterop.js'
import { Eip1193Provider } from './Eip1193Provider.js'
import { LegacyProvider } from './LegacyProvider.js'
import { NullProvider } from './NullProvider.js'

const globalWindow = (window as any);
const resolveObjFromPath = (object, path) => path.split('.').reduce((o, p) => o?.[p], object)

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

            const provider: IJsonRpcProvider = options.supportsEip1193 ?
                new Eip1193Provider(externalProvider, options) :
                new LegacyProvider(externalProvider, options);

            const providerInterop: JsonRpcProviderInterop = new JsonRpcProviderInterop(provider);
            return providerInterop;

        } 

        return new JsonRpcProviderInterop(new NullProvider(options));
    }
}

export const EtherInteropService: EtherInterop = new EtherInterop();