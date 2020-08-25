using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ListaTelefônica
{
    public class PessoaRepository : IPessoaRepository
    {
        private string ConnectionString { get; }

        public PessoaRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<Pessoa> Listar()
        {
            List<Pessoa> pessoas = new List<Pessoa>();
            using (var conexao = new SqlConnection(ConnectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand("select tb_pessoas.id_pessoa, nome, id_telefone, numero, tipo_telefone " +
                    $"from tb_pessoas join tb_telefones on tb_pessoas.id_pessoa = tb_telefones.id_pessoa;", conexao))
                {
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        var pessoa = ObterPessoa(reader);
                        pessoas.Add(pessoa);
                    }
                }
            }
            return pessoas;
        }

        public Pessoa Obter(int id)
        {
            using (var conexao = new SqlConnection(ConnectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand(
                    "select tb_pessoas.id_pessoa, nome, id_telefone, numero, tipo_telefone " +
                    $"from tb_pessoas join tb_telefones on tb_pessoas.id_pessoa = tb_telefones.id_pessoa where tb_pessoas.id_pessoa={id};", conexao))
                {
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        return ObterPessoa(reader);
                    }
                }
            }

            throw new Exception("ID não encontrado");
        }

        public Pessoa Obter(string nome)
        {
            using (var conexao = new SqlConnection(ConnectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand(
                    "select tb_pessoas.id_pessoa, nome, id_telefone, numero, tipo_telefone " +
                    $"from tb_pessoas join tb_telefones on tb_pessoas.id_pessoa = tb_telefones.id_pessoa where tb_pessoas.nome='{nome}';", conexao))
                {
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        return ObterPessoa(reader);
                    }
                }
            }

            return null;
        }

        public void AtualizarTelefone(Pessoa pessoa)
        {
            var telefone = pessoa.Telefones.First();
            using var conexao = new SqlConnection(ConnectionString);
            conexao.Open();
            using var comando = new SqlCommand(
                $"update tb_telefones set tipo_telefone = {(int)telefone.Tipo}, numero = {telefone.Numero} where id_pessoa = {pessoa.Id}; ", conexao);
            comando.ExecuteNonQuery();
        }

        private Pessoa ObterPessoa(SqlDataReader reader)
        {
            var telefone = new Telefone();
            telefone.Id = reader.GetInt32("id_telefone");
            telefone.Tipo = (TipoTelefone)reader.GetByte("tipo_telefone");
            telefone.Numero = reader.GetString("numero");

            var pessoa = new Pessoa();
            pessoa.Id = reader.GetInt32("id_pessoa");
            pessoa.Nome = reader.GetString("nome");
            pessoa.Telefones = new List<Telefone>();
            pessoa.Telefones.Add(telefone);
            return pessoa;
        }

        public void Adicionar(Pessoa pessoa)
        {
            if (pessoa.Id != 0)
            {
                throw new Exception("Já existe essa pessoa no banco");
            }
            using (var conexao = new SqlConnection(ConnectionString))
            {
                conexao.Open();
                using (var transacao = conexao.BeginTransaction())
                {
                    try
                    {
                        int ultimoIdPessoa = 0;
                        using (var comandoCriarPessoa = new SqlCommand(ObterComandoCriarPessoa(pessoa), conexao, transacao))
                        {
                            comandoCriarPessoa.ExecuteNonQuery();
                        }
                        using (var comandoUltimoId = new SqlCommand("select max(id_pessoa) from tb_pessoas", conexao, transacao))
                        {
                            ultimoIdPessoa = (int)comandoUltimoId.ExecuteScalar();
                        }
                        using (var comandoCriarTelefone = new SqlCommand(ObterComandoCriarTelefones(ultimoIdPessoa, pessoa.Telefones), conexao, transacao))
                        {
                            comandoCriarTelefone.ExecuteNonQuery();
                        }
                        pessoa.Id = ultimoIdPessoa;
                        transacao.Commit();
                    }
                    catch (Exception)
                    {
                        transacao.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Remover(int id)
        {
            using var conexao = new SqlConnection(ConnectionString);
            conexao.Open();
            using var comando = new SqlCommand($"delete from tb_telefones where id_pessoa = {id}; delete from tb_pessoas where id_pessoa = {id}; ", conexao);
            comando.ExecuteNonQuery();
        }

        private string ObterComandoCriarPessoa(Pessoa pessoa)
        {
            return $"insert into tb_pessoas (nome) values ('{pessoa.Nome}');";
        }

        private string ObterComandoCriarTelefones(int ultimoIdPessoa, List<Telefone> pessoaTelefones)
        {
            string query = $"insert into tb_telefones (id_pessoa, tipo_telefone, numero) values\n";
            foreach (var telefone in pessoaTelefones)
            {
                string linhaTelefone = $"({ultimoIdPessoa}, {(int)telefone.Tipo}, {telefone.Numero}),\n";
                query += linhaTelefone;
            }
            query = query.TrimEnd(',', '\n');
            return query;
        }
    }
}
