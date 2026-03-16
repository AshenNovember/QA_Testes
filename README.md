# Relatório de Testes
**Tester Responsável:** Rodrigo Correa \
**Data:** 16 de Março de 2026 

Este repositório contém os arquivos de testes e o registro de bugs encontrados no código-fonte **MP**. Cada teste está separado em sua devida pasta com seus devidos programas.

---

## 1. Como executar os testes

Para reproduzir os resultados da auditoria, siga as instruções abaixo:

### **A. Testes Unitários e de Integração**
Em suas devidas pastas (TesteIntegracao e TesteUnitario), execute:
```powershell
dotnet test
```

Alguns dos testes tiveram que ser feitos utilizando o Swagger, um manual interativo da API, que nos permite testar as chamadas diretamente pelo navegador.
```powershell
  dotnet run --urls "http://localhost:5135
```
**Importante**: Certifique-se de que a API está rodando em **localhost:5135** e o Vite em **localhost:5000**.

### **B. Testes E2E**
> **Nota do relator:** Os testes de interface apresentarão falhas sistemáticas de rede (Erro 404). Durante a perícia, identificou-se que o código-fonte do Frontend utiliza a versão de rota `api/v1.0/`, enquanto o Backend responde exclusivamente por `api/v1/`. Além disso, há um conflito de portas (Host `:5000` vs Backend `:5135`).
```powershell
cd QA/TesteE2E
bunx playwright test
```
Para visualizar o laudo visual (vídeos e traces):
```powershell
bunx playwright show-report
```

---

## 2. Estrutura da Pirâmide de Testes

A estratégia seguiu a pirâmide de testes para garantir a máxima cobertura com o menor custo de manutenção:

**Unitários** (Base): Testes focados em métodos isolados, como cálculos de saldo, sanitização de strings e limites de tipos numéricos (ex: prevenção de overflow de double.MaxValue para decimal).  

**Integração** (Meio): São testadas as integrações entre os componentes do sistema, sendo estes componentes classes, funções ou trechos de código. Ex: Foco em garantir que o relacionamento WithMany esteja configurado, evitando shadow properties e carregamento nulo de entidades.  

**E2E** (Topo): Testes de ponta a ponta simulando o usuário. Foco em realizar testes do funcionamento de determinada parte do software. Ex: Validar se as rotas de API e as regras de segurança (como o bloqueio de menores) estão operantes na interface.  

---

## 3. Justificativa das Escolhas de Testes e Bugs encontrados

Devido à **falha crítica de comunicação entre as camadas**, o CRUD via interface foi invalidado e não foi possível avaliar diversos dos pontos marcados no escopo. Usando os testes de integração e o Swagger, foi possível fazer algumas análises, como por exemplo: 
<ul>
<li>Falha de Mapeamento: O teste TesteSomaTotal apresentou erro crítico. Esperado: 800,00 | Obtido: 0,00. Isso prova que o relacionamento entre Pessoa e Transação está quebrado no código, impedindo o cálculo de saldos.</li>
<li>Robustez de Regras: Via Swagger e Integração, confirmou-se que a regra de "Menor de Idade" está ativa, porém causa um erro 500 (crash) por falta de tratamento de exceção.</li>
<li>Prioridade em Tipagem e Limites: Em sistemas financeiros, a precisão numérica é crítica. Justificou-se o teste de overflow (double→decimal) para evitar interrupções de serviço em grandes volumes de dados.</li>

<li>Investigação de ORM: O uso de testes de integração foi escolhido especificamente para diagnosticar falhas silenciosas do Entity Framework, como o mapeamento incompleto de coleções que resultava em saldos zerados.</li>

<li>E2E com Foco em Rede: Devido à inoperância do sistema via interface, o Playwright foi configurado para monitorar o tráfego de rede (Network logs). Isso permitiu identificar que o erro 404 não era um bug de código, mas uma falha de configuração de rota (v1.0).</li>

<li>Segurança Preventiva: A escolha de testar sanitização de inputs visou cumprir os requisitos de segurança do sistema, prevenindo que caracteres de escape corrompam a base de dados ou facilitem ataques de script.</li>
</ul>

Para ver todos bugs encontrados, visitar o [BUGS.md](./BUGS.md)