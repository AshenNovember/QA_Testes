import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './',
  fullyParallel: true,
  reporter: 'html', // Gera um site com os resultados e fotos dos erros
  use: {
    baseURL: 'http://localhost:5000', // A porta onde está o seu Frontend
    trace: 'on',                     // Grava um registo detalhado de cada ação
    video: 'on',                     // Grava vídeo de todos os testes
    screenshot: 'on',
  },
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
  ],
});