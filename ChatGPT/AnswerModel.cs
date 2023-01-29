using System;
using System.Runtime.Serialization;

namespace ChatGPT
{
    /// <summary>
    /// This object, on conversion from JSON, will have the response for the completion prompt
    /// </summary>
    [Serializable]
    public class AnswerModel : ISerializable
    {
        /// <summary>
        /// Array of responses received
        /// </summary>
        public Response[] Choices { get; set; }

        public AnswerModel(SerializationInfo info, StreamingContext context)
        {
            foreach (SerializationEntry s in info)
            {
                switch (s.Name)
                {
                    case "choices":
                        Choices = info.GetValue("choices", typeof(Response[])) as Response[];
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
