using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.BlazorProvider.Lib
{
    public interface ISigner
    {
        ValueTask<string> SignMessage(string message);
    }
}
