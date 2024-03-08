import { defineConfig } from 'vite';
import fs from 'fs';
import path from 'path';

// Provide the path to your certificate and key files
const certPath = path.resolve(__dirname, 'certs/localhost.pem');
const keyPath = path.resolve(__dirname, 'certs/localhost-key.pem');

export default defineConfig({
  server: {
    https: {
      key: fs.readFileSync(keyPath),
      cert: fs.readFileSync(certPath),
    },
    port: 7067, // Optional: Specify your port here
  },
});