using System;

namespace WebNode.ApiModels.Nodes
{
    public class AddPeerModel
    {
        public string Name { get; set; }

        public Uri NetworkAddress { get; set; }
    }
}
