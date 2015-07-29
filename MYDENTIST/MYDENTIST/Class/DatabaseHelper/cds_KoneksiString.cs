using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYDENTIST.Class.DatabaseHelper
{
    public class cds_KoneksiString
    {
        public string server;
        public string username;
        public string password;
        public uint port = 3306;
        public bool pooling = true;
        public bool compress = false;
        public uint defaultCommandTimeout = 5000;
        public uint connectionTimeout = 5000;
        public bool convertZeroDateTime = true;
        public bool allowBatch = true;
        public bool allowUserVariables = true;
        public bool allowZeroDatetime = false;

        public cds_KoneksiString(string server, string uid, string pwd, uint port = 3306)
        {
            this.server = server;
            this.username = uid;
            this.password = pwd;
            this.port = port;
        }

        public override string ToString()
        {
            return
            "Server=" + server +
            ";Port=" + port.ToString() +
            ";Uid=" + username.ToString() +
            ";Pwd=" + password.ToString() +
            ";AllowUserVariables=" + allowUserVariables.ToString() +
            ";ConnectionTimeout=" + connectionTimeout.ToString() +
            ";DefaultCommandTimeout=" + defaultCommandTimeout.ToString() +
            ";ConvertZeroDateTime=" + convertZeroDateTime.ToString() +
            ";Pooling=" + pooling.ToString() +
            ";Compress=" + compress.ToString() +
            ";AllowBatch=" + allowBatch.ToString() +
            ";AllowZeroDateTime=" + allowZeroDatetime.ToString();
        }
    }
}
