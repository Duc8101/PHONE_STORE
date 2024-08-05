using Common.Base;
using DataAccess.Helper;
using System.Net;

namespace API.Services.Hardware
{
    public class HardwareService : IHardwareService
    {
        public ResponseBase Info()
        {
            try
            {
                string data = HardwareHelper.Generate();
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
