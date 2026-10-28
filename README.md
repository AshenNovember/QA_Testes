# 🧪 Relatório de Testes

**👤 Responsável:** Rodrigo Correa
**📅 Data:** 16 de março de 2026

## 📖 Sobre o Projeto

Este repositório reúne os **testes automatizados** e o **registro de bugs** do desafio técnico de QA, aplicado a um sistema financeiro com **API em .NET** e **front-end em Vite**.

A estratégia seguiu a **pirâmide de testes** (unitários, integração e E2E), e cada camada tem a sua própria pasta, com seus programas.

### ✨ O que você encontra aqui

- 🔬 **Testes unitários:** métodos isolados, como cálculo de saldo, sanitização de texto e limites de tipos numéricos.
- 🔗 **Testes de integração:** relacionamento entre entidades, mapeamento do ORM e regras de negócio na API.
- 🖥️ **Testes E2E:** simulação do usuário na interface com Playwright, com análise do tráfego de rede.
- 🐞 **Registro de bugs:** 10 bugs e 2 alertas, classificados por prioridade, em [BUGS.md](BUGS.md).

---

## 🗂️ Estrutura do Repositório

| Pasta / Arquivo | Conteúdo |
|---|---|
| `TesteUnitario/` | Testes unitários (.NET) |
| `TesteIntegracao/` | Testes de integração (.NET) |
| `TesteE2E/` | Testes de ponta a ponta (Playwright) |
| `BUGS.md` | Lista completa de bugs e alertas |

---

## 🚀 Como executar os testes

### 🔬 A. Testes unitários e de integração

Dentro de cada pasta (`TesteUnitario` e `TesteIntegracao`), execute:

```bash
dotnet test
```

Alguns cenários foram verificados manualmente pelo **Swagger**, a documentação interativa da API, que permite fazer as chamadas direto pelo navegador. Para isso, suba a API com:

```bash
dotnet run --urls "http://localhost:5135"
```

> ⚠️ **Importante:** a API deve estar em `localhost:5135` e o front-end (Vite) em `localhost:5000`.

### 🖥️ B. Testes E2E

```bash
cd TesteE2E
bunx playwright test
```

Para abrir o relatório visual (vídeos e traces):

```bash
bunx playwright show-report
```

> 📝 **Nota:** os testes de interface falham por um problema de configuração do sistema, e não por falha nos testes. O front-end chama `api/v1.0/`, mas a API responde apenas em `api/v1/`, o que gera erro **404**. Há ainda um conflito de portas (front-end em `:5000` e API em `:5135`). Detalhes em **BUG-007** e **BUG-008**.

---

## 🔺 Pirâmide de Testes

A pirâmide permite boa cobertura com baixo custo de manutenção.

| Nível | Foco | Exemplo |
|---|---|---|
| 🔬 **Unitário** (base) | Métodos isolados | Cálculo de saldo, sanitização de strings, overflow de `double.MaxValue` para `decimal` |
| 🔗 **Integração** (meio) | Comunicação entre componentes | Relacionamento `WithMany` configurado, evitando propriedades de sombra e carregamento nulo |
| 🖥️ **E2E** (topo) | Fluxo completo do usuário | Rotas da API e regras de segurança, como o bloqueio de receita para menores de idade |

---

## 🎯 Por que escolhi esses testes

- 💰 **Precisão numérica:** em sistemas financeiros, erros de tipo e de limite são críticos. O teste de overflow (`double` para `decimal`) busca evitar interrupções de serviço com grandes volumes de dados.
- 🕵️ **Falhas silenciosas do ORM:** os testes de integração foram escolhidos para diagnosticar problemas do Entity Framework, como o mapeamento incompleto de coleções, que resultava em saldos zerados.
- 🛡️ **Segurança preventiva:** testar a sanitização de entradas evita que caracteres especiais corrompam a base de dados ou facilitem ataques de script.
- 🌐 **E2E com foco em rede:** como a interface estava indisponível, configurei o Playwright para monitorar o tráfego de rede. Isso mostrou que o erro 404 vinha de uma **configuração de rota (v1.0)** e não de um bug de código.

---

## 🐞 Principais achados

| # | Bug | Prioridade |
|---|---|---|
| BUG-005 | Relacionamento `Pessoa.Transacoes` mapeado de forma incompleta. O teste `TesteSomaTotal` esperava **800,00** e obteve **0,00**. | 🔴 Alta |
| BUG-007 | Rota `v1.0` no front-end e `v1` na API, o que gera 404 e **bloqueia** o CRUD pela interface. | 🔴 🚩 Alta e bloqueante |
| BUG-009 | Regra de "menor de idade não pode ter receita" retorna **HTTP 500** em vez de 400. | ⭕ Média |

Para a lista completa, veja o [BUGS.md](BUGS.md).

### 📊 Resumo por prioridade

| Prioridade | Quantidade |
|---|---|
| 🚩 Bloqueante | 1 |
| 🔴 Alta | 7 (incluindo o bloqueante) |
| ⭕ Média | 3 |
| ⚠️ Alertas | 2 |

---

## 🚧 Limitações

Como a comunicação entre front-end e API estava quebrada (**BUG-007**), o CRUD pela interface não pôde ser validado e parte do escopo ficou sem cobertura E2E. Para compensar, usei testes de integração e o Swagger para verificar as regras de negócio diretamente na API.

---

## 🛠️ Tecnologias

- 🟣 **.NET** e **Entity Framework**
- 🎭 **Playwright**
- ⚡ **Bun** e **Vite**
- 📘 **Swagger**
