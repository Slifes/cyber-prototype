using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.scripts
{
    internal class Peer
    {
        private long _id;

        public long Id
        {
            get { return _id; }
        }

        public Peer(long id)
        {
            _id = id;
        }
    }
}
