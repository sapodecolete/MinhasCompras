using MinhasCompras.Models;
using SQLite;

namespace MinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";

            return _conn.QueryAsync<Produto>(
                sql, p.Descricao, p.Quantidade, p.Preco, p.Id
            );
        }

        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM Produto WHERE descricao LIKE '%" + q + "%'";

            return _conn.QueryAsync<Produto>(sql);
        }

        public Task<List<Produto>> GetByDateRange(DateTime dataInicio, DateTime dataFim)
        {
            // Adiciona 1 dia para incluir a data final completa
            DateTime dataFimAjustada = dataFim.Date.AddDays(1);

            return _conn.Table<Produto>()
                       .Where(p => p.DataCadastro >= dataInicio.Date &&
                                 p.DataCadastro < dataFimAjustada)
                       .OrderByDescending(p => p.DataCadastro)
                       .ToListAsync();
        }

        // MÉTODO AUXILIAR PARA ATUALIZAR O BANCO (OPCIONAL)
        public async Task UpdateDatabase()
        {
            // Adiciona a coluna DataCadastro se não existir
            await _conn.ExecuteAsync(
                "ALTER TABLE Produto ADD COLUMN DataCadastro DATETIME DEFAULT CURRENT_TIMESTAMP");
        }
    }
}
