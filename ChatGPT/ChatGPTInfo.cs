using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace ChatGPT
{
    public class ChatGPTInfo : GH_AssemblyInfo
    {
        public override string Name => "ChatGPT";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("22a52895-7982-4d9e-9972-39cbdbf26809");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}