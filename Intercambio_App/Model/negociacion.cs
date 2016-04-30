using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intercambio_App.Model
{
    class negociacion
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "estado")]
        public string estado { get; set; }

        [JsonProperty(PropertyName = "iduseri")]
        public string iduseri { get; set; }

        [JsonProperty(PropertyName = "iduserc")]
        public string iduserc { get; set; }

        [JsonProperty(PropertyName = "idarticuloi")]
        public string idarticuloi { get; set; }

        [JsonProperty(PropertyName = "idarticuloc")]
        public string idarticuloc { get; set; }
    }
}
