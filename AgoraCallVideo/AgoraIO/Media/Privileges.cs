using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraIO.Media
{
    public enum Privileges
    {
        kJoinChannel = 1,
        kPublishAudioStream = 2,
        kPublishVideoStream = 3,
        kPublishDataStream = 4,
        kRtmLogin = 1000
    }
}
