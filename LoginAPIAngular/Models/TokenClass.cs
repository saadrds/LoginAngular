using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace LoginAPIAngular.Models
{
    public class TokenClass : ISerializable
    {
        String Token { get; set; }
        public TokenClass() { }
        public TokenClass(string token)
        {
            this.Token = token;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
