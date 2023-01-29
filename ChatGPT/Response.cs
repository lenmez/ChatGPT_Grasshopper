using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPT
{
    [Serializable]
    public class Response : ISerializable
    {
        /// <summary>
        /// If text is received as a response (in case of completions)
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// If URL is received as a response (in case of images)
        /// </summary>
        public string Url { get; set; }
        public Response(SerializationInfo info, StreamingContext context)
        {
            foreach (var entry in info)
            {
                switch (entry.Name)
                {
                    case ("text"):
                        Text = info.GetString(entry.Name);
                        break;
                    case ("url"):
                        Url = info.GetString(entry.Name);
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
