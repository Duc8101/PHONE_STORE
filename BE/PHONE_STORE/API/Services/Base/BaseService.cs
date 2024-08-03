using AutoMapper;
using DataAccess.DBContext;

namespace API.Services.Base
{
    public class BaseService
    {
        private protected readonly IMapper _mapper;
        private protected readonly PHONE_STOREContext _context;
        public BaseService(IMapper mapper, PHONE_STOREContext context)
        {
            _mapper = mapper;
            _context = context;
        }
    }
}
