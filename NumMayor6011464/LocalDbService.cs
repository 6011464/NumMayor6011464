using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;
namespace NumMayor6011464
{
    public class LocalDbService
    {
        private const string DB_NAME = "demo_suma.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<Resul>().Wait(); // Asegúrate de crear la tabla
        }

        public async Task<List<Resul>> GetResultado()
        {
            return await _connection.Table<Resul>().ToListAsync();
        }

        public async Task<Resul> GetById(int id)
        {
            return await _connection.Table<Resul>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Create(Resul resultado)
        {
            await _connection.InsertAsync(resultado);
        }

        public async Task Update(Resul resultado)
        {
            await _connection.UpdateAsync(resultado);
        }

        public async Task Delete(Resul resultado)
        {
            await _connection.DeleteAsync(resultado);
        }
    }
}
