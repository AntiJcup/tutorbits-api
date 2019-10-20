using System;
using System.Threading.Tasks;
using TutorBits.Models.Common;

namespace TutorBits
{
    namespace DBDataAccess
    {
        public class DBDataAccessService
        {
            private readonly DBDataLayerInterface dataLayer_;

            public DBDataAccessService(DBDataLayerInterface dataLayer)
            {
                dataLayer_ = dataLayer;
            }

            public async Task CreateTutorial(Tutorial tutorial)
            {
                await dataLayer_.Create(tutorial);
            }

            public async Task<Tutorial> GetTutorial(Guid id)
            {
                return await dataLayer_.Get<Tutorial>(id);
            }

            public async Task UpdateTutorial(Tutorial tutorial)
            {
                await dataLayer_.Update(tutorial);
            }

            public async Task DeleteTutorial(Tutorial tutorial)
            {
                await dataLayer_.Delete(tutorial);
            }
        }
    }
}