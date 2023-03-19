using Microsoft.AspNetCore.Mvc;

namespace NewsWebProject.Model.ViewModel
{
    public class VMBuilResponse
    {
        public VMBuilResponse() { }

        public JsonResult BuilResponse(string Code,string Message,object result)
        {
            try
            {
                ResponseFormat responseFormat = new ResponseFormat
                {
                    ReturnCode = Code,
                    ReturnMessage = Message,
                    ResponseId = Guid.NewGuid(),
                    Data = new JsonResult(result)
                };

                return new JsonResult(responseFormat);

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
