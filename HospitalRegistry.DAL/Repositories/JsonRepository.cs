using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.IO;
using HospitalRegistry.BLL.Interfaces;
using HospitalRegistry.Core;

namespace HospitalRegistry.DAL.Repositories
{
    public class JsonRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly string _filePath;
        protected List<T> _items;

        public JsonRepository(string fileName)
        {
            _filePath = fileName;
            _items = LoadData();
        }

        private List<T> LoadData()
        {
            if (!File.Exists(_filePath)) return new List<T>();
            return JsonSerializer.Deserialize<List<T>>(File.ReadAllText(_filePath));
        }

        protected void SaveData()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(_items, options));
        }


        public IEnumerable<T> GetAll() => _items;

        public T GetById(int id) => _items.FirstOrDefault(i => i.Id == id);

        public void Create(T item)
        {
            int newId = (_items.Count == 0) ? 1 : _items.Max(i => i.Id) + 1;
            item.Id = newId;
            _items.Add(item);
            SaveData();
        }

        public void Update(T item)
        {
            var index = _items.FindIndex(i => i.Id == item.Id);
            if (index != -1)
            {
                _items[index] = item;
                SaveData();
            }
        }

        public void Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                _items.Remove(item);
                SaveData();
            }
        }
    }
}
