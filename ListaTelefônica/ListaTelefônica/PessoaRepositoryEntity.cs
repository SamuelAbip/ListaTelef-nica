using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace ListaTelefônica
{
    public class PessoaRepositoryEntity : IPessoaRepository
    {
        private ListaTelefonicaContext Db { get; }

        public PessoaRepositoryEntity(ListaTelefonicaContext db)
        {
            Db = db;
        }

        public void Adicionar(Pessoa pessoa)
        {
            Db.Pessoas.Add(pessoa);
            Db.SaveChanges();
        }

        public void Atualizar(Pessoa pessoa)
        {
            Db.Update(pessoa);
            Db.SaveChanges();
        }

        public List<Pessoa> Listar()
        {
            return Db.Pessoas.Include(x => x.Telefones).ToList();
        }

        public Pessoa Obter(int id)
        {
            return Db.Pessoas.Include(x => x.Telefones).FirstOrDefault(x => x.Id == id);
        }

        public Pessoa Obter(string nome)
        {
            return Db.Pessoas.Include(x => x.Telefones).FirstOrDefault(x => x.Nome == nome);
        }

        public void Remover(Pessoa pessoa)
        {
            Db.Pessoas.Remove(pessoa);
            Db.SaveChanges();
        }
    }
}
