using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
{
    public class HotellTokenDataAcess : IHotellTokenDataAcess
    {
        private readonly IMapper _mapper;
        private readonly ServerDbContext _dbContext;
        public HotellTokenDataAcess(IMapper mapper, ServerDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public int? GetMyId(string myToken)
        {
            // Exceptions:
            //   T:System.ArgumentNullException:
            //     source or predicate is null.
            //
            //   T:System.InvalidOperationException:
            //     No element satisfies the condition in predicate. -or- The source sequence is
            //     empty.
            int id;
            try
            {
                id = _dbContext.HotelInfos.First(x => x.AccessToken == myToken).HotelID;
            }
            catch(Exception e)
            {
                return null;
            }
            return id;
        }
    }
}
