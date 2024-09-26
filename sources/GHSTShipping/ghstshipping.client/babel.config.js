module.exports = {
  presets: [
    "@babel/preset-env", // Transforms modern JavaScript to support older browsers
    "@babel/preset-react", // Transforms React JSX into JavaScript
  ],
  plugins: [
    [
      "module-resolver",
      {
        root: ["."], // Root directory for module resolution
        alias: {
          "@/app": "./src/app", // Alias for your app folder
          "@/assets": "./src/assets", // Alias for assets folder
          "@/services": "./src/services", // Alias for services folder
          "@/pages": "./src/pages",
          "@/providers": "./src/providers",
          "@/components": "./src/components",
        },
        extensions: ['.js', '.jsx'], // Supported file extensions
      },
    ],
    "react-native-reanimated/plugin", // Reanimated plugin, useful for animations
  ],
  env: {
    production: {
      plugins: [
        "transform-remove-console", // Removes console logs in production builds
      ],
    },
  },
};
