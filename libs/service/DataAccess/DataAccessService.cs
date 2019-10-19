using System;
using System.Threading.Tasks;
using TutorBits.Models.Common;

namespace TutorBits
{
    namespace DataAccess
    {
        public class DataAccessService
        {
            private readonly DataLayerInterface dataLayer_;

            public DataAccessService(DataLayerInterface dataLayer)
            {
                dataLayer_ = dataLayer;
            }

            public async Task<Tutorial> GetTutorial(Guid id)
            {
                return await dataLayer_.Get<Tutorial>(id);
            }
        }
    }
}