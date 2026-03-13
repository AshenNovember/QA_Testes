BUG-001 - Campos Nome e Descrição aceitam caracteres inválidos, como # e $.
BUG-002 - Campo DataNascimento aceita datas muito antigas e/ou futuras.
BUG-003 - O uso de double.MaxValue em uma variável decimal pode criar um erro de Overflow, já que decimal é mais    preciso que double ou float. 
BUG-004 - Teste inicial apresentou 2 erros devido ao uso de internal set referente a Pessoa em Transacao.cs.
WARN-001 - O log da API reporta que o Entity Framework criou propriedades de sombra para chaves estrangeiras (PessoaId1, CategoriaId1). 