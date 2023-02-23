using LanguageBooster.Data.Configurations;
using LanguageBooster.Data.IRepositories;
using LanguageBooster.Domain.Commons;
using LanguageBooster.Domain.Entities;
using Newtonsoft.Json;

namespace LanguageBooster.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Auditable
    {
        private string dbFile;
        private List<TEntity> entities = new List<TEntity>();

        // Constructor
        public Repository()
        {
            if (typeof(TEntity) == typeof(User))
            {
                dbFile = Constants.USER_DB;
            }
            else if (typeof(TEntity) == typeof(Language))
            {
                dbFile = Constants.LANGUAGE_DB;
            }
        }

        public Repository(Word word)
        {
            if (word.ChosenLanguageId == 0)
            {
                dbFile = Constants.LANGUAGE_PATH + "\\Uzbek\\Words.json";
            }
            else if (word.ChosenLanguageId == 1)
            {
                dbFile = Constants.LANGUAGE_PATH + "\\English\\Words.json";
            }
        }

        public Repository(Podcast podcast) 
        {
            if (podcast.LanguageId == 0)
            {
                dbFile = Constants.LANGUAGE_PATH + "\\Uzbek\\Podcasts.json";
            }
            else if (podcast.LanguageId == 1)
            {
                dbFile = Constants.LANGUAGE_PATH + "\\English\\Podcasts.json";
            }
        }
        #region create
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            // in this place : you missed give list information as a entities variable
            await SelectAllAsync();
            if (entities.Count == 0)
            {
                entity.Id = 1;
            }
            else
            {
                entity.Id = entities[entities.Count - 1].Id + 1;
            }
            entity.CreatedAt = DateTime.UtcNow;
            entities.Add(entity);
            string jsonEdition = JsonConvert.SerializeObject(entities, Formatting.Indented);
            File.WriteAllText(dbFile, jsonEdition);

            return entity;
        }
        #endregion

        #region generic delete
        public async Task<bool> DeleteAsync(Predicate<TEntity> predicate)
        {
            TEntity entity = await SelectAsync(x => predicate(x));

            if (entity is null)
            {
                return false;
            }

            entities.Remove(entity);

            string jsonEdition = JsonConvert.SerializeObject(entities, Formatting.Indented);
            File.WriteAllText(dbFile, jsonEdition);

            return true;
        }
        #endregion

        #region generic selectAll
        public async Task<List<TEntity>> SelectAllAsync(Predicate<TEntity> predicate = null)
        {
            string content = await File.ReadAllTextAsync(dbFile);
            if (content.Length == 0)
            {
                await File.WriteAllTextAsync(dbFile, "[]");
                content = "[]";
            }

            entities = JsonConvert.DeserializeObject<List<TEntity>>(content);
            if (predicate is not null)
            {
                var result = entities.FindAll(x => predicate(x));
                return result;
            }

            return entities;
        }
        #endregion

        #region generic select
        public async Task<TEntity> SelectAsync(Predicate<TEntity> predicate)
        {
            // in this place too, you forgot to intialize new "entities" variable. Ok
            await SelectAllAsync();
            // And last "Find" method don't return null . But "FirstOrDefault" null return. Ok
            return entities.FirstOrDefault(x => predicate(x));
        }
        #endregion

        #region update
        public async Task<TEntity> UpdateAsync(long id, TEntity entity)
        {
            TEntity objectToUpdate = await SelectAsync(x => x.Id == id);

            int index = entities.IndexOf(objectToUpdate);
            objectToUpdate.LastUpdatedAt = DateTime.UtcNow;

            if (index == -1)
            {
                return null;
            }
            entities.RemoveAt(index);
            entities.Insert(index, entity);

            string jsonEdition = JsonConvert.SerializeObject(entities, Formatting.Indented);
            File.WriteAllText(dbFile, jsonEdition);

            return entity;
        }
        #endregion
    }
}
