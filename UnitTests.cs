using MinhasFinancas.Domain.Entities;
using Xunit;

namespace QA;

public class UnitTests
{
    [Fact]
    public void Teste_Menor_de_Idade()
    {
        var dataNasc = DateTime.Today.AddYears(-15);
        var pessoa = new Pessoa { Nome = "Rodrigo", DataNascimento = dataNasc };

        Assert.False(pessoa.EhMaiorDeIdade());
        Assert.Equal(15, pessoa.Idade);
    }

    //Teste do BUG-001//
    [Fact]
    public void Nome_Com_Caracteres_Especiais()
    {
        var nomeModificado = "Rodrigo C#$!@2000";
        var pessoa = new Pessoa
        {
            Nome = nomeModificado,
            DataNascimento = DateTime.Today.AddYears(-20),
        };

        Assert.Equal(nomeModificado, pessoa.Nome);
    }

    //Teste do BUG-002//
    [Fact]
    public void Data_Nascimento_Extrema()
    {
        var dataMG = new DateTime(1215, 6, 15);
        var pessoa = new Pessoa { Nome = "Joao Sem Terra", DataNascimento = dataMG };

        Assert.True(pessoa.Idade > 800);
    }

    //Teste do BUG-003//
    [Fact]
    public void Overflow_No_Decimal()
    {
        double valorPermitidoPeloAtributo = double.MaxValue;

        Assert.Throws<OverflowException>(() =>
        {
            decimal valorConvertido = (decimal)valorPermitidoPeloAtributo;

            var transacao = new Transacao { Valor = valorConvertido };
        });
    }

    [Fact]
    public void Categoria_Tipo_Ambas_Deve_Permitir_Receita_E_Despesa()
    {
        var categoria = new Categoria
        {
            Descricao = "Outros",
            Finalidade = Categoria.EFinalidade.Ambas,
        };

        Assert.True(categoria.PermiteTipo(Transacao.ETipo.Receita));
        Assert.True(categoria.PermiteTipo(Transacao.ETipo.Despesa));
    }
}
