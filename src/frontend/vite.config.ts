import { defineConfig, loadEnv } from 'vite';
import fs from 'fs';
import path from 'path';

const certPath = path.resolve(__dirname, 'certs/localhost.pem');
const keyPath = path.resolve(__dirname, 'certs/localhost-key.pem');

export default defineConfig(({command, mode}) => {
  const env = loadEnv(mode, process.cwd(), '')
  return {
    server: {
      https: {
        key: fs.readFileSync(keyPath),
        cert: fs.readFileSync(certPath),
      },
      port: Number.parseInt(env.PORT),
    },
    define: {
      __API_URL__: JSON.stringify(env.services__Api__1),
    },
  }
});