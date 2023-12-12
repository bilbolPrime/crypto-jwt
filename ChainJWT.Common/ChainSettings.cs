using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilbolStack.ChainJWT.Common
{
    public class ChainSettings
    {
        public const string ConfigKey = "ChainInfo";
        public string AccountPrivateKey { get; set; }
        public long ChainId { get; set; }
        public string RpcUrl { get; set; }
        public string NFTContractAddress { get; set; }
    }
}
