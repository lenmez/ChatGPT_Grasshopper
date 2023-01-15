using Grasshopper;
using Grasshopper.Kernel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;

namespace ChatGPT
{
    public class ChatGPTComponent : GH_Component
    {
        string API_KEY { get; set; }
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ChatGPTComponent()
          : base("ChatGPT", "Nickname",
            "Description",
            "ChatGPT", "ChatGPT")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Open API Key", "Open API Key", "Get it from https://beta.openai.com/account/api-keys", GH_ParamAccess.item);
            pManager.AddTextParameter("Question", "Question", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Answer", "Answer", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string key = "";
            string ques = "";
            if (!DA.GetData(0, ref key))
                return;
            if (!DA.GetData(1, ref ques))
                return;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 |
                SecurityProtocolType.Tls12 |
                SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls;

            string apiAddress = "https://api.openai.com/v1/completions";
            var request = WebRequest.Create(apiAddress);
            request.Method = "POST";
            request.ContentType= "application/json";
            request.Headers.Add("Authorization", "Bearer " + key);

            string model = "text-davinci-003";

            QAModel qAModel = new QAModel()
            {
                Model = model,
                Prompt = ques,
                Max_Tokens = 100,
                Temperature = 0,
                Frequency_Penalty = 0.0,
                Presence_Penalty = 0.0,
                Stop = new string[] { "\"n" }
            };

            string json = JsonConvert.SerializeObject(qAModel);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var response = request.GetResponse();
            var streamReader = new StreamReader(response.GetResponseStream());
            string sJson = streamReader.ReadToEnd();

            AnswerModel answerModel = JsonConvert.DeserializeObject<AnswerModel>(sJson);

            if(answerModel!=null && answerModel.Choices.Length>0)
            {
                DA.SetData(0, answerModel.Choices[0].Text);
            }
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("7eb13b9a-9dca-40c5-b5b9-728f509cfb2b");
    }

    [Serializable]
    public class AnswerModel:ISerializable
    {
        public Choice[] Choices { get; set; }

        public AnswerModel(SerializationInfo info, StreamingContext context)
        {
            foreach(SerializationEntry s in info)
            {
                switch(s.Name)
                {
                    case "choices":
                        Choices = info.GetValue("choices", typeof(Choice[])) as Choice[];
                        break;
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class Choice:ISerializable
    {
        public string Text { get; set; }

        public Choice(SerializationInfo info, StreamingContext context)
        {
            Text = info.GetString("text");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}