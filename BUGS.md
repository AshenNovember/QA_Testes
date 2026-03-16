:triangular_flag_on_post: - Bloqueante \
:red_circle: - Prioridade: Alta \
:o: - Prioridade: Média \
:warning: Prioridade: baixa  

---
## Testes unitários
:red_circle: **BUG-001** - Campos Nome e Descrição aceitam caracteres inválidos, como # e $.

:o: **BUG-002** - Campo DataNascimento aceita datas muito antigas e/ou futuras.

:red_circle: **BUG-003** - O uso de double.MaxValue em uma variável decimal pode criar um erro de Overflow, já que decimal é mais preciso que double ou float.

---
## Testes de integração
:red_circle: **BUG-004** - Teste inicial apresentou 2 erros devido ao uso de internal set referente a Pessoa em Transacao.cs.

:red_circle: **BUG-005** - Mapeamento de relacionamento incompleto no DbContext impede o carregamento de coleções (Pessoa.Transacoes), resultando em saldo zero em consultas.

:warning: ***WARN-001*** - O log da API reporta que o Entity Framework criou propriedades de sombra para chaves estrangeiras (PessoaId1, CategoriaId1).

:red_circle: **BUG-006** - A API permite salvar transações com finalidade incompatível (ex: Receita em categoria de Despesa), gerando erro apenas na persistência.

---
## Teste E2E (Ponta a Ponta)
:red_circle: :triangular_flag_on_post: **BUG-007** - A versão hard-coded do Vite no código-fonte está configurado com a versão v1.0 nas URLs de requisição, enquanto o API esponde apenas pela versão v1.

:red_circle: **BUG-008** - Front-end está fixado na porta 5000(localhost:5000) e certas funções perdem funcionalidade, como o gráfico no dashboard.

:o: **BUG-009** - Ao violar a regra de "Menor de idade não pode ter receita", a API retorna HTTP 500 (Internal Server Error) em vez de um 400 (Bad Request).

:o: **BUG-010** - Após clicar em "Salvar", caso ocorra um erro de rede (404 ou 500), a interface não fecha o modal nem exibe um alerta mais especifíco, seja para o usuário ou no console.log.

:warning: **WARN-002** - Ocorrencia de net::ERR_CONNECTION_REFUSED indica falta de resiliência no servidor de desenvolvimento.
