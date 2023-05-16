using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Services.ApiModels
{
    public class ISBN
    {
        // do not fully understand, what the differet ISBNs are. In Google API it seems, that the ISBN we feed in comes
        // back as this type.
        private readonly string _ISBNTypePublicISBN = "ISBN_13";


        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("identifier")]
        public string ISBNNumber { get; set; }

        /** 
         * Returns true, if the ISBN record is the one, that is the one we are most familiar with.
         * Dont understand the different types of ISBNs yet, though.
         * 
         * @return true, if this ISBN is the most familiar one.
         */
        public Boolean IsPublicISBN ()
        {
            if (Type.Equals (_ISBNTypePublicISBN))
            {
                return true;
            }

            return false;
        }
    }
}
