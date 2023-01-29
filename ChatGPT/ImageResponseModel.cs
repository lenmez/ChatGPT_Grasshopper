using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPT
{
    /// <summary>
    /// This object, on conversion from JSON, will have the response for the Image generation or Edit prompt
    /// </summary>
    [Serializable]
    public class ImageResponseModel : ISerializable
    {
        /// <summary>
        /// Array of responses received
        /// </summary>
        public Response[] Responses { get; set; }

        public ImageResponseModel(SerializationInfo info, StreamingContext context)
        {
            foreach (SerializationEntry s in info)
            {
                switch (s.Name)
                {
                    case "data":
                        Responses = info.GetValue(s.Name, typeof(Response[])) as Response[];
                        break;
                }
            }
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
