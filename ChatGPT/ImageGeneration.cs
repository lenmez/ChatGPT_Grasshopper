using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPT
{
    [Serializable]
    public class ImageGeneration : ISerializable
    {
        /// <summary>
        /// Prompts regarding description of image to be generated
        /// </summary>
        public string Prompt { get; set; }

        /// <summary>
        /// Number of images to be generated (between 1 and 10)
        /// </summary>
        public int NumberOfImages { get; set; }

        /// <summary>
        /// Size of image can be 256x256, 512x512, or 1024x1024
        /// </summary>
        public string SizeOfImage { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("prompt", Prompt);
            info.AddValue("n", NumberOfImages);
            info.AddValue("size", SizeOfImage);
        }
    }
}
