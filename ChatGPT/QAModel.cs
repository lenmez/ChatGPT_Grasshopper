using System;
using System.Runtime.Serialization;



namespace ChatGPT
{
    /// <summary>
    /// This object, on conversion to JSON, can be posted to OpenAI completions API
    /// https://api.openai.com/v1/completions
    /// </summary>
    [Serializable]
    public class QAModel : ISerializable
    {
        /// <summary>
        /// Model to use, for example "text-davinci-003"
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Prompts /Questions to generate completions for
        /// </summary>
        public string Prompt { get; set; }

        /// <summary>
        /// Sampling temterature (Lower values result in well defined answers)
        /// Higher values will take risks and get more creative
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// May number of tokens to generate
        /// </summary>
        public int Max_Tokens { get; set; }

        /// <summary>
        /// Nucleus sampling (better keep default as 1, unless you know what you are doing)
        /// </summary>
        public int Top_p { get; set; }
        public double Frequency_Penalty { get; set; } = 0.0;
        public double Presence_Penalty { get; set; } = 0.0;
        
        /// <summary>
        /// Stop sequence
        /// </summary>
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
