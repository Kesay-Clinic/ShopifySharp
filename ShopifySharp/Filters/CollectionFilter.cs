using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifySharp.Filters;

public class CollectionFilter: Parameterizable
{
        /// <summary>
        /// Restrict results to those with the given title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

}
