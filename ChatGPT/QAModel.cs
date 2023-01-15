using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace ChatGPT
{
    [Serializable]
    public class QAModel:ISerializable
    {
        public string Model { get; set; }
        public string Prompt { get; set; }
        public double Temperature { get; set; }
        public int Max_Tokens { get; set; }
        public int Top_p { get; set; }
        public double Frequency_Penalty { get; set; }
        public double Presence_Penalty { get; set; }
        public string[] Stop { get; set; }

        public QAModel()
        {

        }

        public QAModel(SerializationInfo info, StreamingContext context)
        {

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("model", Model);
            info.AddValue("prompt", Prompt);
            info.AddValue("temperature", Temperature);
            info.AddValue("max_tokens", Max_Tokens);
            info.AddValue("top_p", Top_p);
            info.AddValue("frequency_penalty", Frequency_Penalty);
            info.AddValue("presence_penalty", Presence_Penalty);
            info.AddValue("stop", Stop);
        }
    }
}
