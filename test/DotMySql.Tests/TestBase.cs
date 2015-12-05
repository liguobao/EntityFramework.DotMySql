using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DotMySql.Tests
{
    public class TestBase
    {

        internal MySqlConnection Conn { get; set; }



        public string ConnectionString
        {
            get { return "server=localhost;Database=test;Uid=root;Pwd="; }
        }

    }
}
