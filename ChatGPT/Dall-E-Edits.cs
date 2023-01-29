using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Newtonsoft.Json;


namespace ChatGPT
{
    /// <summary>
    /// Grasshopper component that makes DALL-E image edit API calls 
    /// </summary>
    public class Dall_E_Edits : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Dall_E_Edits class.
        /// </summary>
        public Dall_E_Edits()
          : base("Dall-E Editor", "DE",
              "Component for editing image using DALL-E",
              "ChatGPT", "ChatGPT")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Open API Key", "Open API Key", "Get it from https://beta.openai.com/account/api-keys", GH_ParamAccess.item);
            pManager.AddTextParameter("Image to Edit", "Image to Edit", "Image to Edit. Must be a valid PNG file, less than 4MB, and square", GH_ParamAccess.item);
            pManager.AddTextParameter("Mask", "Mask", "An additional image whose fully transparent areas indicate where image should be edited", GH_ParamAccess.item);
            pManager.AddTextParameter("Prompt", "Prompt", "Prompt for DALL-E", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Answer", "Answer", "Response based on the prompt", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string api_key = "";
            string imagePath = "";
            string maskPath = "";
            string prompt = "";

            //Get Input values
            if (!DA.GetData(0, ref api_key))
                return;
            if (!DA.GetData(1, ref imagePath))
                return;
            if (!DA.GetData(2, ref maskPath))
                return;
            if (!DA.GetData(3, ref prompt))
                return;

            if (!File.Exists(imagePath))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Could not find image file at path " + imagePath);
                return;
            }

            if (!File.Exists(maskPath))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Could not find mask file at path " + maskPath);
                return;
            }

            var imageBytes = File.ReadAllBytes(imagePath);
            var maskBytes = File.ReadAllBytes(maskPath);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 |
                SecurityProtocolType.Tls12 |
                SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls;

            var result = Task.Run(() => ResponseMessage(api_key, imagePath, maskPath, prompt, 1, "1024x1024")).Result;
            var content = Task.Run(() => result.Content.ReadAsStringAsync()).Result;

            if (result.IsSuccessStatusCode)
            {
                ImageResponseModel responseModel = JsonConvert.DeserializeObject<ImageResponseModel>(content);

                if (responseModel.Responses.Length == 1)
                    DA.SetData(0, responseModel.Responses[0].Url);
            }

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
        /// Get response message based on given input values
        /// </summary>
        /// <param name="key"></param>
        /// <param name="imagePath"></param>
        /// <param name="maskPath"></param>
        /// <param name="prompt"></param>
        /// <param name="number"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        async Task<HttpResponseMessage> ResponseMessage(string key, string imagePath, string maskPath, string prompt, int number, string size)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.openai.com/v1/images/edits"))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + key);

                    var multipartContent = new MultipartFormDataContent
                    {
                        { new ByteArrayContent(File.ReadAllBytes(imagePath)), "image", Path.GetFileName(imagePath) },
                        { new ByteArrayContent(File.ReadAllBytes(maskPath)), "mask", Path.GetFileName(maskPath) },
                        { new StringContent(prompt), "prompt" },
                        { new StringContent(number.ToString()), "n" },
                        { new StringContent(size), "size" }
                    };
                    request.Content = multipartContent;

                    return await httpClient.SendAsync(request);
                }
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("478090DA-E528-4966-BF6E-7636084E5C0D"); }
        }
    }
}