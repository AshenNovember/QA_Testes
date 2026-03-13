using Microsoft.EntityFrameworkCore;
using MinhasFinancas.Application.DTOs;
using MinhasFinancas.Application.Services;
using MinhasFinancas.Domain.Entities;
using MinhasFinancas.Infrastructure;
using MinhasFinancas.Infrastructure.Data;
using Xunit;

namespace MinhasFinancas.Tests.Integration;

public class IntegracaoTeste
{
    private MinhasFinancasDbContext CriarBancoTeste()
    {
        var options = new DbContextOptionsBuilder<MinhasFinancasDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new MinhasFinancasDbContext(options);
    }

    [Fact]
    public async Task TesteCategoriaErrada()
    {
        using var context = CriarBancoTeste();
        var uow = new UnitOfWork(context);
        var service = new TransacaoService(uow);

        var p = new Pessoa
        {
            Nome = "Rodrigo Teste",
            DataNascimento = DateTime.Today.AddYears(-20),
        };
        context.Pessoas.Add(p);

        var cat = new Categoria { Descricao = "Luz", Finalidade = Categoria.EFinalidade.Despesa };
        context.Categorias.Add(cat);
        await context.SaveChangesAsync();

        var dto = new CreateTransacaoDto
        {
            Descricao = "Teste Errado",
            Valor = 100,
            Tipo = Transacao.ETipo.Receita,
            PessoaId = p.Id,
            CategoriaId = cat.Id,
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(dto));
    }

    [Fact]
    public async Task TesteMenorDeIdadeReceita()
    {
        using var context = CriarBancoTeste();
        var uow = new UnitOfWork(context);
        var service = new TransacaoService(uow);

        var menor = new Pessoa { Nome = "Menor", DataNascimento = DateTime.Today.AddYears(-15) };
        context.Pessoas.Add(menor);

        var cat = new Categoria
        {
            Descricao = "Mesada",
            Finalidade = Categoria.EFinalidade.Receita,
        };
        context.Categorias.Add(cat);
        await context.SaveChangesAsync();

        var dto = new CreateTransacaoDto
        {
            Descricao = "Salario",
            Valor = 500,
            Tipo = Transacao.ETipo.Receita,
            PessoaId = menor.Id,
            CategoriaId = cat.Id,
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(dto));
    }

    [Fact]
    public async Task TesteExcluirPessoaLimparTransacao()
    {
        using var context = CriarBancoTeste();
        var uow = new UnitOfWork(context);
        var service = new TransacaoService(uow); // Precisei do service por causa do internal set

        var pessoa = new Pessoa { Nome = "Joao", DataNascimento = DateTime.Today.AddYears(-20) };
        var cat = new Categoria { Descricao = "Geral", Finalidade = Categoria.EFinalidade.Ambas };

        context.Pessoas.Add(pessoa);
        context.Categorias.Add(cat);
        await context.SaveChangesAsync();

        // Usando o service porque a Transacao.Pessoa é 'internal set' (BUG-004)
        await service.CreateAsync(
            new CreateTransacaoDto
            {
                Descricao = "Teste",
                Valor = 50,
                Tipo = Transacao.ETipo.Despesa,
                PessoaId = pessoa.Id,
                CategoriaId = cat.Id,
            }
        );

        // Apagando a pessoa para ver se a transação some junto
        context.Pessoas.Remove(pessoa);
        await context.SaveChangesAsync();

        var total = await context.Transacoes.CountAsync();
        Assert.Equal(0, total);
    }

    [Fact]
    public async Task TesteSomaTotal()
    {
        using var context = CriarBancoTeste();
        var uow = new UnitOfWork(context);
        var service = new TransacaoService(uow);

        var p = new Pessoa { Nome = "Carlos", DataNascimento = DateTime.Today.AddYears(-20) };
        context.Pessoas.Add(p);
        var cat = new Categoria { Descricao = "Geral", Finalidade = Categoria.EFinalidade.Ambas };
        context.Categorias.Add(cat);
        await context.SaveChangesAsync();

        await service.CreateAsync(
            new CreateTransacaoDto
            {
                Valor = 1000,
                Tipo = Transacao.ETipo.Receita,
                PessoaId = p.Id,
                CategoriaId = cat.Id,
                Descricao = "R",
            }
        );
        await service.CreateAsync(
            new CreateTransacaoDto
            {
                Valor = 200,
                Tipo = Transacao.ETipo.Despesa,
                PessoaId = p.Id,
                CategoriaId = cat.Id,
                Descricao = "D",
            }
        );

        var pNoBanco = await context
            .Pessoas.Include(x => x.Transacoes)
            .FirstOrDefaultAsync(x => x.Id == p.Id);

        // Verificação para o compilador não reclamar de nulo
        Assert.NotNull(pNoBanco);

        // Vai dar 0 por causa do erro de mapeamento do WithMany
        var saldo =
            pNoBanco.Transacoes.Where(x => x.Tipo == Transacao.ETipo.Receita).Sum(x => x.Valor)
            - pNoBanco.Transacoes.Where(x => x.Tipo == Transacao.ETipo.Despesa).Sum(x => x.Valor);

        Assert.Equal(800m, saldo);
    }
}
