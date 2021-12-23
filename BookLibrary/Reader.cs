using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary
{
    public class Reader
    {
        [JsonProperty("ReaderName")]
        private string ReaderName { get; set; }
        [JsonProperty("RentedBooksList")]
        public List<RentedBook> RentedBooksList { get; set; }
        public Reader(string name)
        {
            ReaderName = name;
            RentedBooksList = new List<RentedBook>();
        }

        public string GetName()
        {
            return ReaderName;
        }

        public void SetName(string value)
        {
            ReaderName = value;
        }
    }
}
