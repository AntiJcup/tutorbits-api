using System;
using System.Collections.Generic;
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

            public async Task<Tutorial> CreateTutorial(Tutorial tutorial)
            {
                return await dataLayer_.Create(tutorial);
            }

            public async Task<Tutorial> GetTutorial(Guid id)
            {
                return await dataLayer_.Get<Tutorial>(id);
            }

            public async Task<ICollection<Tutorial>> GetAllTutorialsForUser(Guid userId)
            {
                return await dataLayer_.GetAll<Tutorial>((t => t.UserId == userId), null, null);
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