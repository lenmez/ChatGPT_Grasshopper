using System;
using System.IO;
using System.Net;
using Grasshopper.Kernel;
using Newtonsoft.Json;

namespace ChatGPT
{
    /// <summary>
    /// Grasshopper component that makes DALL-E image generation API calls 
    /// </summary>
    public class Dall_E : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Dall_E class.
        /// </summary>
        public Dall_E()
          : base("Dall-E Generator", "Nickname",
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
            pManager.AddTextParameter("Prompt", "Prompt", "Prompt for DALL-E", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Image Url", "Image Url", "Url of generated image", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string api_key = "";
            string prompt = "";
            if (!DA.GetData(0, ref api_key))
                return;
            if (!DA.GetData(0, ref prompt))
                return;
            

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 |
                SecurityProtocolType.Tls12 |
                SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls;

            string apiAddress = "https://api.openai.com/v1/images/generations";
            var request = WebRequest.Create(apiAddress);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + api_key);

            //Initialize ImageGeneration object
            ImageGeneration imageGeneration = new ImageGeneration()
            {
                Prompt = prompt,
                NumberOfImages = 1,
                SizeOfImage = "1024x1024"
            };

            //Convert ImageGeneration object to JSON
            string json = JsonConvert.SerializeObject(imageGeneration);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var response = request.GetResponse();
            var streamReader = new StreamReader(response.GetResponseStream());
            string sJson = streamReader.ReadToEnd();

            //Deserialize response to ImageResponseModel
            ImageResponseModel imageResponse = JsonConvert.DeserializeObject<ImageResponseModel>(sJson);

            DA.SetData(0, imageResponse.Responses[0].Url);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("3E0CAB1D-7593-467C-BA10-8151C015B6B7"); }
        }
    }
}