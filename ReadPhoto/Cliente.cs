using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadPhoto
{
    public class Cliente
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public Cliente () { }

        public Cliente(string name, string phone, string email)
        {
            Name = name;
            Phone = phone;
            Email = email;
        }
    }
}
