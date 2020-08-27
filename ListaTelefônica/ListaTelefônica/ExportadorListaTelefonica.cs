using System.Collections.Generic;
using System.Data;
using System.IO;
using OfficeOpenXml;

namespace ListaTelefônica
{
    class ExportadorListaTelefonica
    {
        public void ExportarLista(List<Pessoa> listaTelefonica, string localArquivo)
        {
            var tabela = ConverterPraTabela(listaTelefonica);
            SalvarExcel(tabela, localArquivo);
        }

        private DataTable ConverterPraTabela(List<Pessoa> listaTelefonica)
        {
            DataTable tabela = new DataTable();
            tabela.Columns.Add("Nome");
            tabela.Columns.Add("Tipo");
            tabela.Columns.Add("Telefone");
            foreach (var pessoa in listaTelefonica)
            {
                var linha = tabela.NewRow();
                linha["Nome"] = pessoa.Nome;
                linha["Tipo"] = pessoa.Telefones[0].Tipo.ToString();
                linha["Telefone"] = pessoa.Telefones[0].Numero;
                tabela.Rows.Add(linha);
            }
            return tabela;
        }

        private void SalvarExcel(DataTable tabela, string localArquivo)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Lista Telefônica");
            worksheet.Cells[1, 1].Value = "Nome";
            worksheet.Cells[1, 2].Value = "Tipo";
            worksheet.Cells[1, 3].Value = "Telefone";
            worksheet.Cells["A2"].LoadFromDataTable(tabela);
            package.SaveAs(new FileInfo(localArquivo));
        }
    }
}
