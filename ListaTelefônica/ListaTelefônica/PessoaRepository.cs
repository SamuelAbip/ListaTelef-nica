using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
            throw new NotImplementedException();
        }

        public Pessoa Obter(int id)
        {
            using (var conexao = new SqlConnection(ConnectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand(
                    "select tb_pessoas.id_pessoa, nome, id_telefone, numero, tipo_telefone from tb_pessoas join tb_telefones on tb_pessoas.id_pessoa = tb_telefones.id_pessoa;", conexao))
                {
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        var telefone = new Telefone();
                        telefone.Id = reader.GetInt32("id_telefone");
                        telefone.Tipo = (TipoTelefone)reader.GetInt32("tipo_telefone");
                        telefone.Numero = reader.GetString("numero");

                        var pessoa = new Pessoa();
                        pessoa.Id = reader.GetInt32("id_pessoa");
                        pessoa.Nome = reader.GetString("nome_pessoa");
                        pessoa.Telefones = new List<Telefone>();
                        pessoa.Telefones.Add(telefone);
                        return pessoa;
                    }
                }
            }

            throw new Exception("ID não encontrado");
        }

        public void Adicionar(Pessoa pessoa)
        {
            throw new NotImplementedException();
        }
    }
}
