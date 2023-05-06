const typescript = require('@rollup/plugin-typescript');

exports.default = {
    input: "./src/index.ts",
    output: {
        file: "./bundles/liyanjie.linq.umd.js",
        name: "liyanjie.linq",
        format: "umd",
    },
    plugins: [typescript()]
};
