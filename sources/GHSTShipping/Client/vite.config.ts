import react from '@vitejs/plugin-react';
import path from 'path';
import { defineConfig } from 'vite';
import vitePluginImp from 'vite-plugin-imp';
import svgrPlugin from 'vite-plugin-svgr';

// https://vitejs.dev/config/

export default defineConfig({
  esbuild: {
    logOverride: { 'this-is-undefined-in-esm': 'silent' }
  },
  resolve: {
    alias: {
      '@': path.join(__dirname, 'src'),
    },
  },
  server: {
    port: 5555,
    // proxy: {
    //   '/api': {
    //     target: `http://localhost:5001/api`,
    //     // changeOrigin: true,
    //     //rewrite: path => path.replace(/^\/api/, ''),
    //   },
    // },
  },
  plugins: [
    react({
      jsxImportSource: '@emotion/react',
      babel: {
        plugins: ['@emotion/babel-plugin'],
      },
    }),
    vitePluginImp({
      libList: [
        {
          libName: 'antd',
          //style: name => `antd/es/${name}/style/index.js`,
          style: name => name !== 'theme' && `antd/es/${name}/style`,
        },
        {
          libName: 'lodash',
          libDirectory: '',
          camel2DashComponentName: false,
          style: () => {
            return false;
          },
        },
      ],
    }),
    svgrPlugin({
      svgrOptions: {
        icon: true,
        // ...svgr options (https://react-svgr.com/docs/options/)
      },
    }),
  ],
});
