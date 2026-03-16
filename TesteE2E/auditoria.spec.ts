import { test, expect } from '@playwright/test';

test.describe('Auditoria Completa do Sistema - Dia 4', () => {
  test('Verificação do Include em Pessoa', async ({ page }) => {
    await page.goto('/pessoas');
    await page.click('text=Adicionar Pessoa');

    page.on('request', request => {
      if (request.method() === 'POST') {
        console.log('>> PAYLOAD DO FRONTEND:', request.postData());
        console.log('>> HEADERS:', request.headers());
      }
    });

    await page.fill('input[name="nome"]', 'Pedro Ferreira');
    await page.fill('input[name="dataNascimento"]', '1990-01-01');
    await page.click('button:has-text("Salvar")');

  });
  test('Validação do destino da API e funcionamento geral', async ({ page }) => {
    await page.goto('/pessoas');
    await page.click('text=Adicionar Pessoa');

    const [request] = await Promise.all([
      page.waitForRequest(req => req.url().includes('api/v1.0')),
      await page.fill('input[name="nome"]', 'João Silva'),
      await page.fill('input[name="dataNascimento"]', '1990-01-01'),
      await page.click('button:has-text("Salvar")')
    ]);

  console.log(`EVIDÊNCIA: Requisição enviada para URL errada: ${request.url()}`);
  expect(request.url()).toContain('5000');
});
});