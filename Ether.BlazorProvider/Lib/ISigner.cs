using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.BlazorProvider
{
    public interface ISigner
    {
        /// <summary>
        /// Signs a message using the person_sign RPC spec.
        /// The caller must provide the address the selected address of the wallet
        /// </summary>
        /// <param name="message">The message to be signed</param>
        /// <param name="address">The address of the signer</param>
        ValueTask<string> SignMessage(string message,string address);

        /// <summary>
        /// Signs a message using the person_sign RPC spec.
        /// This method will first get the selected wallet address by executing a RPC call. 
        /// If the caller is tracking the address then the SignMessage(string message,string address) should rather be used
        /// </summary>
        /// <param name="message"></param>
        ValueTask<string> SignMessage(string message);
    }
}
