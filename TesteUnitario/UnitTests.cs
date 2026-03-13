using MinhasFinancas.Domain.Entities;
using Xunit;

namespace QA;

public class UnitTests
{
    [Fact]
    public void TesteIdadeMenor()
    {
        // Rodrigo tem 15 anos
        var dataNasc = DateTime.Today.AddYears(-15);
        var p = new Pessoa { Nome = "Rodrigo", DataNascimento = dataNasc };

        // Ele não pode ser maior de idade
        Assert.False(p.EhMaiorDeIdade());
        Assert.Equal(15, p.Idade);
    }

    [Fact]
    public void TesteNomeBugado() 
    {
        // BUG-001: Aceita qualquer coisa no nome
        var nomeComErro = "Rodrigo C#$!@2000";
        var p = new Pessoa { Nome = nomeComErro, DataNascimento = DateTime.Today.AddYears(-20) };

        Assert.Equal(nomeComErro, p.Nome);
    }

    [Fact]
    public void TesteDataMuitoAntiga()
    {
        // BUG-002: Aceita data de 1215 (Idade vai dar mais de 800)
        var dataVelha = new DateTime(1215, 6, 15);
        var p = new Pessoa { Nome = "Joao", DataNascimento = dataVelha };

        Assert.True(p.Idade > 800);
    }

    [Fact]
    public void TesteErroValorMuitoGrande()
    {
        // BUG-003: Tentar colocar o maior double dentro de um decimal
        double valorGigante = double.MaxValue;

        // O C# deve estourar um erro de Overflow na conversão
        Assert.Throws<OverflowException>(() =>
        {
            decimal valorConvertido = (decimal)valorGigante;
            var t = new Transacao { Valor = valorConvertido };
        });
    }

    [Fact]
    public void TesteCategoriaAmbas()
    {
        // Categoria que aceita tudo
        var cat = new Categoria
        {
            Descricao = "Geral",
            Finalidade = Categoria.EFinalidade.Ambas,
        };

        // Tem que deixar passar os dois tipos
        Assert.True(cat.PermiteTipo(Transacao.ETipo.Receita));
        Assert.True(cat.PermiteTipo(Transacao.ETipo.Despesa));
    }
}