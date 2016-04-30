using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intercambio_App.Model
{
    public class UserData
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Username")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "Direccion")]
        public string Direccion { get; set; }

        [JsonProperty(PropertyName = "UPSEmail")]
         public string UPSEmail { get; set; }
    }
}
